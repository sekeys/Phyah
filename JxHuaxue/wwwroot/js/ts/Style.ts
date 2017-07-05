/// <reference path="color.ts" />

import { zonic as color } from "./Color"
import box from "./Box"

export module zonic {

    export class TransformProperty {
        constructor(value) {
        }
    }
    export class AnimationProperty {
        constructor(value) { }
    }
    export interface IStyleProperty {
        key: string;
        value: string;
        color(): color.IColor;
        transform(): TransformProperty;
        Animation(): AnimationProperty;
    }
    export class StyleProperty implements IStyleProperty {
        key: string;
        value: string;
        color() {
            return new color.Color(this.value);
        }
        transform() {
            return new TransformProperty(this.value);
        }
        Animation() {
            return new AnimationProperty(this.value);
        }
        constructor(key, value) {
            this.key = key;
            this.value = value;
        }
    }
    class StylePropertyMap {
        private Properties: IStyleProperty[];
        toString() {
            var sr = "";
            this.Properties.forEach(function (value) {
                sr += value.toString();
            });
            return sr;
        }
        constructor() {
            this.Properties = [];
        }
        set(key, value) {
            var property = this.get(key);
            if (property === null) {
                property = new StyleProperty(key, value);
                this.Properties.push(property);
            }
            else {
                property.value = value;
            }
            return this;
        }
        forEach(fn) {
            this.Properties.forEach(fn)
        }
        static parse(cssText) {
            var map = new StylePropertyMap();
            map.merge(cssText);
            return map;
        }
        merge(str) {
            if (str instanceof String) {
                var proMap = StylePropertyMap.parse(str.toString());
                proMap.forEach(element => {
                    this.set(element.key, element.value);
                });
            }
            else if (str instanceof Array) {
                proMap.forEach(element => {
                    this.set(element.key, element.value);
                });
            }
            else if (str instanceof Object) {
                for (var i in str) {
                    this.set(i, str[i] instanceof Function ? str[i]() : str[i]);
                }
            }
            return this;
        }
        remove(key) {
            for (var i = 0; i < this.Properties.length; i++) {
                if (this.Properties[i].key === key) {
                    this.Properties.slice(0, i).concat(this.Properties.slice(i + 1));
                }
            }
            return this;
        }
        get(key): IStyleProperty {
            for (var i = 0; i < this.Properties.length; i++) {
                if (this.Properties[i].key === key) {
                    return this.Properties[i];
                }
            }
            return null;
        }
    }
    export interface IStyleRule {
        set(key, value);
        remove(key);
        get(key): IStyleProperty;
        //apply(dom: HTMLElement);
        apply(component);
        merge(rule: Object);
        initialize(component);
    }
    export class StyleRule implements IStyleRule {
        private map = new StylePropertyMap();
        name = "stylerule" + new Date().valueOf();
        suffix = "normal";
        DOM: box.Box
        constructor(dom?, name?) {
            if (name) {
                this.name = name;
            }
            this.DOM = dom;
            this.initialize(this.DOM);
        }
        initialize(component?) {
            if (component instanceof box.Box) {
                return this.initialize(this.DOM.DOM);
            }
            else if (component instanceof HTMLElement) {
                var style = component.style, length = style.length;
                for (var i = 0; i < length; i++) {
                    var property = style.item(i), val = style.getPropertyValue(property);
                    this.map.set(property, val);
                }
            } else if ((this.DOM instanceof HTMLDocument)) {
                return this;
            } else if (this.DOM) {
                return this.initialize(this.DOM);
            }
            return this;
        }
        static parse(css) {
            var s = new StyleRule();
            s.map.merge(css);
            return s;
        }
        set(key, value) {
            this.map.set(key, value);
            return this;
        }
        remove(key) {
            this.map.remove(key);
            return this;
        }
        get(key) {
            return this.map.get(key);
        }
        apply(component) {
            //box.Box["css"](component, this.map);
            if (typeof component === "string") {
                component = StyleRule.parse(component);
            }
            if (component instanceof box.Box) {
                this.DOM = component;
                this.css(this.map);
            } else if (component instanceof HTMLElement) {
                this.DOM = new box.Box(component);
                this.css(this.map);
            } else if (component && this.DOM) {
                this.css(component instanceof StyleRule? component.map:component);
            }
            return this;
        }

        parseProperty(property) {
            var s = property.split("-"), i = 0, l = s.length, p = "";
            for (; i < l; i++) {
                if (i === 0) {
                    p = s[i].toLowerCase();
                } else {
                    p += s[i].charAt(0).toUpperCase() + s[i].slice(1);
                }
            }
            return p;
        }
        _getStyle(elem) {
            if (elem.ownerDocument && elem.ownerDocument.defaultView.opener) {
                return elem.ownerDocument.defaultView.getComputedStyle(elem, null);
            }
            return window.getComputedStyle(elem, null);
        }
        css(prop: any, value?): any {
            var me = this, cssProperty = function (prop, value) {
                var p = prop;
                if (prop === "float") {
                    p = this.style.cssFloat ? "cssFloat" : "styleFloat";
                }
                if (prop === "opacity") {
                    p = "opacity";
                    cssProperty.call(this, "filter", "filter:alpha(opacity=" + parseFloat(value) * 100 + ")");
                }
                var curElSty = this.currentStyle || me._getStyle(this), elSty = this.style;
                p = me.parseProperty(p);
                if (p.toLowerCase() === "zindex") {
                    me.style(me.DOM, "z-index", value);
                    return;
                }
                try {
                    if (p in curElSty) {
                        try {
                            curElSty[p] = value;
                        } catch (ex) {
                        }
                    }
                    curElSty.setProperty(p, value);
                } catch (ex) {
                    try {
                        if (p in elSty) {
                            try {
                                elSty[p] = value;
                            } catch (ex) {
                            }
                        }
                        elSty.setProperty(p, value);
                    } catch (ex) {
                        try {
                            elSty.cssText += ";" + prop + ":" + value + ";";
                        } catch (ex) {
                            console.log("don't allow to modify the css");
                        }
                    }
                }
            }, cssgetProperty = function (prop) {
                var curElSty = this.currentStyle || window.getComputedStyle(this, null), elSty = this.style;
                if (prop === "float") {
                    var p = this.style.cssFloat ? "cssFloat" : "styleFloat";
                }
                if (prop === "opacity") {
                    return elSty.opacity || curElSty.opacity || (elSty.filters && elSty.filters.alpha ? elSty.filters.alpha.opacity : 100) / 100;
                }
                return elSty[prop] || curElSty[prop];
            };
            if (arguments.length === 1) {
                if (typeof prop === "string") {
                    return cssgetProperty.call(new box.Box(this.DOM).DOM, this.parseProperty(prop));
                } else if (prop instanceof StylePropertyMap) {
                    var dom = new box.Box(this.DOM).DOM;
                    prop.forEach(function (v,index) {
                        cssProperty.call(dom, v.key, v.value);
                    });
                }else {
                    for (var p in prop)
                        cssProperty.call(new box.Box(this.DOM).DOM, p.trim(), prop[p]);
                }
            } else if (arguments.length === 2) {
                if (typeof prop === "string") {
                    cssProperty.call(new box.Box(this.DOM).DOM, prop, value);
                } else if (typeof value === "function") {
                    if (prop instanceof StylePropertyMap) {
                        var dom = new box.Box(this.DOM).DOM;
                        prop.forEach(function (v, index) {
                            value.call(dom, v.key, v.value);
                        });
                    } else {

                        for (var p in prop)
                            value.call(new box.Box(this.DOM).DOM, [p.trim(), prop[p]]);
                    }
                }
            }
            return this;
        }
        style(bom: any, prop, value?) {
            if (!(bom instanceof box.Box)) {
                bom = new box.Box(bom);
            }
            //elSty.cssText += ";" + prop + ":" + value + ";";
            var style = bom.attr("style") || "";
            if (arguments.length === 2) {
                if (typeof prop === "string") {
                    style += ";" + prop;
                } else {
                    var c = "";
                    for (var p in prop)
                        c += p + ":" + prop[p] + ";";
                    style += ";" + c;
                }
            } else if (arguments.length === 3) {
                style += ";" + prop + ":" + value;
            }
            bom.attr("style", style);
            return this;
        }
        merge(rule) {
            this.map.merge(rule);
        }
    }
    export class StyleRuleManager {
        applied = [];
        DOM: HTMLElement
        constructor(dom) {
            this.DOM = dom;
            this.addValue({
                rule: new StyleRule().initialize(this.DOM),
                mode: "origin"
            });
        }
        private addValue(value) {
            this.applied.push(value);
            return this;
        }
        current() {
            return new StyleRule().initialize(this.DOM);
        }
        apply(rule) {
            if (rule instanceof StyleRule) {
                this.applied.push(rule);
                rule.apply(this.DOM);
            }
            else {
                var merule = StyleRule.parse(rule);
                this.applied.push(merule);
                merule.apply(this.DOM);
            }
            return this;
        }
        unApply(rule) {

        }
    }
}