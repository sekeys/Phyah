/// <reference path="color.ts" />
define(["require", "exports", "./Color", "./Box"], function (require, exports, Color_1, Box_1) {
    "use strict";
    exports.__esModule = true;
    var zonic;
    (function (zonic) {
        var TransformProperty = (function () {
            function TransformProperty(value) {
            }
            return TransformProperty;
        }());
        zonic.TransformProperty = TransformProperty;
        var AnimationProperty = (function () {
            function AnimationProperty(value) {
            }
            return AnimationProperty;
        }());
        zonic.AnimationProperty = AnimationProperty;
        var StyleProperty = (function () {
            function StyleProperty(key, value) {
                this.key = key;
                this.value = value;
            }
            StyleProperty.prototype.color = function () {
                return new Color_1.zonic.Color(this.value);
            };
            StyleProperty.prototype.transform = function () {
                return new TransformProperty(this.value);
            };
            StyleProperty.prototype.Animation = function () {
                return new AnimationProperty(this.value);
            };
            return StyleProperty;
        }());
        zonic.StyleProperty = StyleProperty;
        var StylePropertyMap = (function () {
            function StylePropertyMap() {
                this.Properties = [];
            }
            StylePropertyMap.prototype.toString = function () {
                var sr = "";
                this.Properties.forEach(function (value) {
                    sr += value.toString();
                });
                return sr;
            };
            StylePropertyMap.prototype.set = function (key, value) {
                var property = this.get(key);
                if (property === null) {
                    property = new StyleProperty(key, value);
                    this.Properties.push(property);
                }
                else {
                    property.value = value;
                }
                return this;
            };
            StylePropertyMap.prototype.forEach = function (fn) {
                this.Properties.forEach(fn);
            };
            StylePropertyMap.parse = function (cssText) {
                var map = new StylePropertyMap();
                map.merge(cssText);
                return map;
            };
            StylePropertyMap.prototype.merge = function (str) {
                var _this = this;
                if (str instanceof String) {
                    var proMap = StylePropertyMap.parse(str.toString());
                    proMap.forEach(function (element) {
                        _this.set(element.key, element.value);
                    });
                }
                else if (str instanceof Array) {
                    proMap.forEach(function (element) {
                        _this.set(element.key, element.value);
                    });
                }
                else if (str instanceof Object) {
                    for (var i in str) {
                        this.set(i, str[i] instanceof Function ? str[i]() : str[i]);
                    }
                }
                return this;
            };
            StylePropertyMap.prototype.remove = function (key) {
                for (var i = 0; i < this.Properties.length; i++) {
                    if (this.Properties[i].key === key) {
                        this.Properties.slice(0, i).concat(this.Properties.slice(i + 1));
                    }
                }
                return this;
            };
            StylePropertyMap.prototype.get = function (key) {
                for (var i = 0; i < this.Properties.length; i++) {
                    if (this.Properties[i].key === key) {
                        return this.Properties[i];
                    }
                }
                return null;
            };
            return StylePropertyMap;
        }());
        var StyleRule = (function () {
            function StyleRule(dom, name) {
                this.map = new StylePropertyMap();
                this.name = "stylerule" + new Date().valueOf();
                this.suffix = "normal";
                if (name) {
                    this.name = name;
                }
                this.DOM = dom;
                this.initialize(this.DOM);
            }
            StyleRule.prototype.initialize = function (component) {
                if (component instanceof Box_1["default"].Box) {
                    return this.initialize(this.DOM.DOM);
                }
                else if (component instanceof HTMLElement) {
                    var style = component.style, length = style.length;
                    for (var i = 0; i < length; i++) {
                        var property = style.item(i), val = style.getPropertyValue(property);
                        this.map.set(property, val);
                    }
                }
                else if (this.DOM) {
                    return this.initialize(this.DOM);
                }
                return this;
            };
            StyleRule.parse = function (css) {
                var s = new StyleRule();
                s.map.merge(css);
                return s;
            };
            StyleRule.prototype.set = function (key, value) {
                this.map.set(key, value);
                return this;
            };
            StyleRule.prototype.remove = function (key) {
                this.map.remove(key);
                return this;
            };
            StyleRule.prototype.get = function (key) {
                return this.map.get(key);
            };
            StyleRule.prototype.apply = function (component) {
                //box.Box["css"](component, this.map);
                if (component instanceof Box_1["default"].Box) {
                    this.style(component, this.map);
                }
                return this;
            };
            StyleRule.prototype.parseProperty = function (property) {
                var s = property.split("-"), i = 0, l = s.length, p = "";
                for (; i < l; i++) {
                    if (i === 0) {
                        p = s[i].toLowerCase();
                    }
                    else {
                        p += s[i].charAt(0).toUpperCase() + s[i].slice(1);
                    }
                }
                return p;
            };
            StyleRule.prototype._getStyle = function (elem) {
                if (elem.ownerDocument && elem.ownerDocument.defaultView.opener) {
                    return elem.ownerDocument.defaultView.getComputedStyle(elem, null);
                }
                return window.getComputedStyle(elem, null);
            };
            StyleRule.prototype.css = function (prop, value) {
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
                    try {
                        if (p in curElSty) {
                            try {
                                curElSty[p] = value;
                            }
                            catch (ex) {
                            }
                        }
                        curElSty.setProperty(p, value);
                    }
                    catch (ex) {
                        try {
                            if (p in elSty) {
                                try {
                                    elSty[p] = value;
                                }
                                catch (ex) {
                                }
                            }
                            elSty.setProperty(p, value);
                        }
                        catch (ex) {
                            try {
                                elSty.cssText += ";" + prop + ":" + value + ";";
                            }
                            catch (ex) {
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
                        return cssgetProperty.call(this.DOM.DOM, this.parseProperty(prop));
                    }
                    else {
                        for (var p in prop)
                            cssProperty.call(this.DOM.DOM, p.trim(), prop[p]);
                    }
                }
                else if (arguments.length === 2) {
                    if (typeof prop === "string") {
                        cssProperty.call(this.DOM.DOM, prop, value);
                    }
                    else if (typeof value === "function") {
                        for (var p in prop)
                            value.call(this.DOM.DOM, [p.trim(), prop[p]]);
                    }
                }
                return this;
            };
            StyleRule.prototype.style = function (bom, prop, value) {
                //elSty.cssText += ";" + prop + ":" + value + ";";
                var style = bom.attr("style") || "";
                if (arguments.length === 1) {
                    if (typeof prop === "string") {
                        style += ";" + prop;
                    }
                    else {
                        var c = "";
                        for (var p in prop)
                            c += p + ":" + prop[p] + ";";
                        style += ";" + c;
                    }
                }
                else if (arguments.length === 2) {
                    style += ";" + prop + ":" + value;
                }
                bom.attr("style", style);
                return this;
            };
            StyleRule.prototype.merge = function (rule) {
                this.map.merge(rule);
            };
            return StyleRule;
        }());
        zonic.StyleRule = StyleRule;
        var StyleRuleManager = (function () {
            function StyleRuleManager(dom) {
                this.applied = [];
                this.DOM = dom;
                this.addValue({
                    rule: new StyleRule().initialize(this.DOM),
                    mode: "origin"
                });
            }
            StyleRuleManager.prototype.addValue = function (value) {
                this.applied.push(value);
                return this;
            };
            StyleRuleManager.prototype.current = function () {
                return new StyleRule().initialize(this.DOM);
            };
            StyleRuleManager.prototype.apply = function (rule) {
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
            };
            StyleRuleManager.prototype.unApply = function (rule) {
            };
            return StyleRuleManager;
        }());
        zonic.StyleRuleManager = StyleRuleManager;
    })(zonic = exports.zonic || (exports.zonic = {}));
});
