(function (window, undefined) {
    var base = {}, op = Object.prototype,
        ostring = op.toString,
        hasOwn = op.hasOwnProperty,
        isBrowser = !!(typeof window !== 'undefined' && typeof navigator !== 'undefined' && window.document),
        isWebWorker = !isBrowser && typeof importScripts !== 'undefined'
        ;
    function eachProp(obj, func) {
        var prop;
        for (prop in obj) {
            if (base.hasProp(obj, prop)) {
                if (func(obj[prop], prop)) {
                    break;
                }
            }
        }
    }
    base.if = function (exp, fn1, fn2) {
        if (arguments.length == 0) { return this; }
        if (arguments.length == 1) { return exp; }
        else { if (exp) { return fn1.call(this, null); } else { return fn2.call(this, null); } }
    }
    base.slice = function (list) {
        return [].slice.call(list);
    };
    base.isBrowser = !!(typeof window !== 'undefined' && typeof navigator !== 'undefined' && window.document);
    base.isWebWorker = !isBrowser && typeof importScripts !== 'undefined';
    base.isFunction = function (it) {
        return ostring.call(it) === '[object Function]';
    }
    base.isArray = function (it) {
        return ostring.call(it) === '[object Array]';
    }
    base.isObject = function (it) {
        return ostring.call(it) === '[object Object]';
    }
    base.merge = function (obj) {
        var i = 1
            , target
            , key;

        for (; i < arguments.length; i++) {
            target = arguments[i];
            for (key in target) {
                if (Object.prototype.hasOwnProperty.call(target, key)) {
                    obj[key] = target[key];
                }
            }
        }
        return obj;
    }
    base.clone = function (ob, deep) {
        if (!deep) {
            for (var i in ob) { this[i] = ob[i]; }
        } else {
            for (var i in ob) {
                if (base.isObject(ob[i]) && !base.isFunction(ob[i])) {
                    this[i] = new ob[i].constructor();
                    base.clone.call(this[i], ob[i]);
                } else {
                    this[i] = ob[i];
                }
            }
        }
    }
    base.eachReverse = function (ary, func) {
        if (ary) {
            var i;
            for (i = ary.length - 1; i > -1; i -= 1) {
                if (ary[i] && func(ary[i], i, ary)) {
                    break;
                }
            }
        }
    }
    base.hasProp = function (obj, prop) {
        return hasOwn.call(obj, prop);
    }
    base.getOwn = function (obj, prop) {
        return base.hasProp(obj, prop) && obj[prop];
    }
    base.mixin = function (target, source, force, deepStringMixin) {
        if (source) {
            eachProp(source, function (value, prop) {
                if (force || !base.hasProp(target, prop)) {
                    if (deepStringMixin && typeof value === 'object' && value &&
                        !isArray(value) && !isFunction(value) &&
                        !(value instanceof RegExp)) {

                        if (!target[prop]) {
                            target[prop] = {};
                        }
                        base.mixin(target[prop], value, force, deepStringMixin);
                    } else {
                        target[prop] = value;
                    }
                }
            });
        }
        return target;
    }
    base.bind = function (obj, fn) {
        return function () {
            return fn.apply(obj, arguments);
        };
    }
    base.logicAnd = function () {
        if (arguments.length) {
            var param = true;
            for (var i = 0; i < arguments.length; i++) {
                param &= !!(arguments[i] instanceof Function ? arguments[i]() : arguments[i]);
            }
            return param;
        }
        else {
            return false;
        }
    }
    base.logicOr = function () {
        if (arguments.length) {
            var param = false;
            for (var i = 0; i < arguments.length; i++) {
                param |= !!(arguments[i] instanceof Function ? arguments[i]() : arguments[i]);
            }
            return param;
        }
        else {
            return false;
        }
    }
    base.sequence = function () {
        if (arguments.length) {
            var param = null;
            for (var i = 0; i < arguments.length; i++) {
                param = arguments[i].call(this, param);
            }
            return param;
        }
        else {
            return;
        }
    }
    base.querystring = function () {
        var qs = {};
        var href = window.location.href;
        var indexofPos = href.lastIndexOf('?'), indexofj = href.indexOf("#");
        if (indexofj < indexofPos) {
            href = href.substr(indexofPos);
        } else {
            href = href.substr(indexofPos, indexofj - indexofPos);
        }
        var hrefs = href.split('&');
        for (var i = 0; i < hrefs.length; i++) {
            if (hrefs[i]) {
                var kv = hrefs[i].split("=");
                qs[kv[0]] = kv[1];
            }
        }
        return qs;
    }
    base.guid = function (seqchar) {
        var s = [];
        seqchar = seqchar == null ? "-" : seqchar;
        var hexDigits = "0123456789abcdef";

        for (var i = 0; i < 36; i++) {
            s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
        }

        s[14] = "4"; // bits 12-15 of the time_hi_and_version field to 0010

        s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1); // bits 6-7 of the clock_seq_hi_and_reserved to 01

        s[8] = s[13] = s[18] = s[23] = seqchar;

        var uuid = s.join("");

        return uuid;
    }
    base.requote = function (s) {
        return s.replace(/[\\\^\$\*\+\?\|\[\]\(\)\.\{\}]/g, "\\$&");
    };
    base.rebind = function (target, source, arg) {
        function rebind(target, source, method) {
            return function () {
                var value = method.apply(source, arguments);
                return value === source ? target : value;
            };
        }
        var i = 1, n = arguments.length, method;
        while (++i < n)
            target[method = arguments[i]] = rebind(target, source, source[method]);
        return target;
    }
    function emitter(obj) {
        if (obj) return emittermixin(obj);
    };

    function emittermixin(obj) {
        for (var key in emitter.prototype) {
            obj[key] = emitter.prototype[key];
        }
        return obj;
    }

    emitter.prototype.on =
        emitter.prototype.addEventListener = function (event, fn) {
            this._callbacks = this._callbacks || {};
            (this._callbacks['$' + event] = this._callbacks['$' + event] || [])
                .push(fn);
            return this;
        };

    emitter.prototype.once = function (event, fn) {
        function on() {
            this.off(event, on);
            fn.apply(this, arguments);
        }

        on.fn = fn;
        this.on(event, on);
        return this;
    };

    emitter.prototype.off =
        emitter.prototype.removeListener =
        emitter.prototype.removeAllListeners =
        emitter.prototype.removeEventListener = function (event, fn) {
            this._callbacks = this._callbacks || {};

            // all
            if (0 == arguments.length) {
                this._callbacks = {};
                return this;
            }

            // specific event
            var callbacks = this._callbacks['$' + event];
            if (!callbacks) return this;

            // remove all handlers
            if (1 == arguments.length) {
                delete this._callbacks['$' + event];
                return this;
            }

            // remove specific handler
            var cb;
            for (var i = 0; i < callbacks.length; i++) {
                cb = callbacks[i];
                if (cb === fn || cb.fn === fn) {
                    callbacks.splice(i, 1);
                    break;
                }
            }
            return this;
        };

    emitter.prototype.emit = function (event) {
        this._callbacks = this._callbacks || {};
        var args = [].slice.call(arguments, 1)
            , callbacks = this._callbacks['$' + event];

        if (callbacks) {
            callbacks = callbacks.slice(0);
            for (var i = 0, len = callbacks.length; i < len; ++i) {
                callbacks[i].apply(this, args);
            }
        }
        return this;
    };

    emitter.prototype.listeners = function (event) {
        this._callbacks = this._callbacks || {};
        return this._callbacks['$' + event] || [];
    };

    emitter.prototype.hasListeners = function (event) {
        return !!this.listeners(event).length;
    };
    base.emitter = emitter;
    function element(exp, context) {
        this.selected = [];
        if (typeof exp === "string") {
            return this.select(exp, context);
        } else if (base.isFunction(exp)) {
            return exp.call(this, base.slice(arguments).slice(1));
        } else if (exp instanceof element) {
            return exp;
        } else if (exp && exp.nodeType) {
            this.selected.push(exp);
        } else if (base.isArray(exp)) {
            exp.forEach(function () {
                if (exp.nodeType) {
                    return this.selected.push(exp);
                }
            });
        }
        return this;
    }
    base.element = element;
    function selectionMatchs(n, s) {
        var obj = document.createElement("div"), selectMatcher = obj["matches"] || obj[temp_vendorSymbolFunc(obj, "matchesSelector")], selectMatches = function (n, s) {
            return selectMatcher.call(n, s);
        };
        return selectMatches(n, s);
    };
    element.prototype.getBoundingHeight = function () {
        return this.getBoundingRect().height;
    };
    element.prototype.getBoundingWidth = function () {
        return this.getBoundingRect().width;
    };
    element.prototype.getBoundingRect = function () {
        return this.first().node().getBoundingClientRect() || { width: 0, top: 0, bottom: 0, height: 0, left: 0, right: 0 };
    };
    element.prototype.show = function () {
        return this.each(function (item) { element.select(this).css({ "opacity": 1, display: element.DOM.getDisplayType(this) }); });
    };
    element.prototype.hide = function () {
        return this.css({ "opacity": "0", display: "none" });
    };
    element.prototype.select = function (exp, context) {
        this.selected = [];
        if (typeof exp === "string") {
            if (/^</.test(exp)) {
                return this.select(element.parseHtml(exp));
            } else if (/create:/i.test(exp)) {
                return this.select(element.parseHtml(exp.substr(7)));
            } else {
                return this.queriesSelection(exp, context);
            }
        } else if (exp instanceof element) {
            this.selected = exp.selected;
            return this;
        } else if (exp.selected && exp.selected.length) {
            this.selected = exp.selected;
            return this;
        } else if (exp.nodeType) {
            this.selected.push(exp);
            return this;
        } else if (base.isArray(exp)) {
            var self = this;
            exp.forEach(function (x) { x.nodeType ? self.selected.push(x) : ""; });
        }
        return this;
    }
    element.prototype.parseNodeList = function (nl) {
        if (!this.selected) { this.selected = []; }
        for (var i = 0; i < nl.length; i++) {
            this.selected.push(nl[i]);
        }
    };
    element.prototype.queriesSelection = function (exp, context) {
        if (!context || !context.querySelectorAll) {
            if (context instanceof element
                || context instanceof ui) {
                var me = this;
                context.each(function () {
                    me.parseNodeList(this.querySelectorAll(exp));
                });
                return this;
            }
            this.parseNodeList(document.querySelectorAll(exp));
            return this;
        }
        this.parseNodeList(context.querySelectorAll(exp));
        return this;
    };
    element.prototype.each = function (fn, args) {
        this.selected.forEach(function (val, index, arry) {
            fn.apply(val, [val, index, arry].concat(args));
        });
        return this;
    };
    element.prototype.eaching = function (fn, args) {
        var r = [];
        this.selected.forEach(function (val, index, arry) {
            var rs = fn.apply(this, [val, index, arry].concat(args));
            if (rs) {
                r.push(rs);
            }
        });
        return r;
    };
    element.prototype.call = function (fn, pos, args) {
        if (pos != null && pos > 0) {
            return fn.call(this.at(pos), args);
        } else {
            return this.each(fn, args);
        }
    };
    element.prototype.calling = function (fn, pos, args) {
        if (this.selected.length == 1) {
            pos = 0;
        }
        if (pos != null && pos > -1) {
            return fn.call(this.at(pos), args);
        } else {
            return this.eaching(fn, args);
        }
    };
    element.prototype.parseHtml = function (tag) {
        this.selected = [];
        this.selected.push(element.parseHtml(tag));
        return tag;
    };
    element.prototype.bind =
        element.prototype.on = function (evn, fn, args) {
            if (!evn) {
                return this;
            }
            return this.each(function () {
                try {
                    this.addEventListener(evn, fn, false);
                } catch (e) {
                    try {
                        this.attachEvent('on' + evn, fn);
                    } catch (e) {
                        this['on' + evn] = fn;
                    }
                }
            });
        };
    element.prototype.unbind =
        element.prototype.off = function (evn, fn, args) {
            if (!evn) {
                return this;
            }
            return this.each(function () {
                try {
                    this.removeEventListener(evn, fn, false);
                } catch (e) {
                    try {
                        this.detachEvent('on' + evn, fn);
                    } catch (e) {
                        delete this['on' + evn];
                    }
                }
            });
        };
    element.prototype.live =
        element.prototype.once = function (evn, fn, args) {
            if (!evn) {
                return this;
            }
            var self = this, tempFn = function () {
                fn.apply(this, base.slice(arguments));
                self.off(evn, tempFn);
            };
            this.on(evn, tempFn);
            return this;
        };
    element.prototype.trigger =
        element.prototype.emit = function (evn) {
            if (!evn) {
                return this;
            }
            return this.each(function () {
                if (this.dispatchEvent && document.createEvent) {
                    var e = document.createEvent("MouseEvents");
                    e.initEvent(evn, true, true);
                    this.dispatchEvent(e);
                } else if (document.all) {
                    this[evn]();
                }
            });
        };
    element.prototype.if = function (exp, fn1, fn2) {
        if (arguments.length == 0) { return this; }
        if (arguments.length == 1) { return exp; }
        else { if (exp) { return fn1.call(this, null); } else { return base.isFunction(fn2) ? fn2.call(this, null) : this; } }
    }
    element.prototype.removeClass = function (cls) {
        var r = false, j = 0, value, clses = cls.split(/\s+/), i = 0, n = clses.length;
        this.each(function () {
            if (value = this.classList) {
                while (i < n) {
                    if (value.contains(clses[i])) {
                        value.remove(clses[i]);
                    }
                    i++;
                }
            } else {
                value = this.getAttribute("class");
                while (i < n) {
                    value = value.replace(new RegExp("(?:^|\\s+)" + base.requote(clses[i]) + "(?:\\s+|$)", "g"), "");
                    i++;
                }
                this.setAttribute("class", value);
            }
            j++;
        });
        return this;
    };
    element.prototype.hasClass = function (cls) {
        var r = true, j = 0, value, clses = cls.split(/\s+/), i = 0, n = clses.length;
        this.each(function () {
            if (value = this.classList) {
                while (i < n) {
                    if (!value.contains(clses[i])) {
                        r = false;
                        return true;
                    }
                    i++;
                }
            } else {
                value = this.getAttribute("class");
                while (i < n) {
                    //value = value.replace(, "");
                    if (!new RegExp("(?:^|\\s+)" + base.requote(clses[i]) + "(?:\\s+|$)", "g").test(value)) {
                        r = false;
                        break;
                    }
                    i++;
                }
                this.setAttribute("class", value);
            }
            j++;
        });
        return r;
    };
    element.prototype.remove = function () {
        this.each(function () {
            this.parentNode ? this.parentNode.removeChild(this) : null;
        });
        return this;
    };
    element.prototype.class = function (cls) {
        return this.attr("class", cls);
    };
    element.prototype.addClass = function (cls) {
        if (!cls) {
            return this;
        }
        var r = false, j = 0, value;
        this.each(function () {
            if ((value = this.classList) && value.add) {
                value.add(cls);
            } else {
                value = this.getAttribute("class");

                this.setAttribute("class", value + " " + cls);
            }
            j++;
        });
        return this;
    };
    element.prototype.css = function (prop, value) {
        var cssProperty = function (prop, value) {
            var p = prop;
            if (prop === "float") {
                p = this.style.cssFloat ? "cssFloat" : "styleFloat";
            }
            if (prop === "opacity") {
                //"opacity:" + p + ";filter:alpha(opacity=" + p * 100 + ");";
                p = "opacity";

                cssProperty.call(this, "filter", "filter:alpha(opacity=" + parseFloat(value) * 100 + ")");
            }
            var curElSty = this.currentStyle || element.DOM._getStyle(this), elSty = this.style;
            p = element.DOM.parseProperty(p);
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
        }, me = this;

        if (arguments.length === 1) {
            if (typeof prop === "string") {
                return cssgetProperty.call(this.node(), element.DOM.parseProperty(prop));
            } else {
                for (var p in prop)
                    this.each(function () {
                        cssProperty.call(this, p.trim(), prop[p]);
                    });
            }
        } else if (arguments.length === 2) {
            if (typeof prop === "string") {
                cssProperty.call(this.node(0), prop, value);
            } else if (typeof value === "function") {
                for (var p in prop)
                    this.each(function () {
                        value.call(this, [p.trim(), prop[p]]);
                    });
            }
        }
        return this;
    };
    element.prototype.style = function (prop, value) {
        //elSty.cssText += ";" + prop + ":" + value + ";";
        var style = this.attr("style") || "";
        if (arguments.length === 1) {
            if (typeof prop === "string") {
                style += ";" + prop;
            } else {
                var c = "";
                for (var p in prop)
                    c += p + ":" + prop[p] + ";";
                style += ";" + c;
            }
        } else if (arguments.length === 2) {
            style += ";" + prop + ":" + value;
        }
        this.attr("style", style);
        return this;
    }
    element.prototype.attr = function (name, value) {
        if (arguments.length < 2) {
            if (typeof name === "string") {
                var node = this.node();
                name = element.DOM.qualify(name);
                if (!node.getAttribute) { return ""; }
                return name.local ? node.getAttributeNS(name.space, name.local) : node.getAttribute(name);
            }
            for (value in name)
                this.each(element.DOM.attr(value, name[value]));
            return this;
        }
        return this.each(element.DOM.attr(name, value));
    };
    element.prototype.prop = function (name, value) {
        if (!value) {
            var node = this.node();
            return node[name];
        }
        node[name] = value;
        return this;
    };
    element.prototype.html = function (html, pos) {
        if (html != null) {
            this.call(function (d) {
                this.innerHTML = html;
                var items = this.getElementsByTagName("script");
                if (items.length) {
                    for (var i = 0; i < items.length; i++) {
                        if (items[i].innerHTML) { eval(items[i].innerHTML); }
                    };
                }
            }, pos);
            return this;
        }
        return this.calling(function (d) {
            return this.node(0).innerHTML;
        }, this.selected.length > 1 ? pos : 0);
    };
    element.prototype.outHtml = function (pos) {
        return this.calling(function (d) {
            return this.node().outerHTML;
        }, pos);
    };
    element.prototype.value = function (val, pos) {
        if (val != null) {
            this.call(function (d) {
                if (this.node instanceof Function) {
                    this.node().value = val;
                } else {
                    this.value = val;
                }
            }, pos);
            return this;
        }
        return this.calling(function (d) {
            return this.node().value;
        }, pos);
    };
    element.prototype.text = function (txt, pos) {
        if (txt != null) {
            this.call(function (d) {
                if (this.node instanceof Function) {
                    this.node(0).innerText = txt;
                } else {
                    this.innerText = txt;
                }
            }, pos);
            return this;
        }
        return this.calling(function (d) {
            return this.node(0).innerText;
        }, this.selected.length > 1 ? pos : 0);
    };
    element.prototype.find = function (exp) {
        if (typeof exp === "string") {
            if (/^</.test(exp)) {
                return element.select(element.parseHtml(exp));
            } else {
                return new element().queriesSelection(exp, this);
            }
        } else if (exp instanceof element) {
            return new element().select(exp);
        } else if (exp.nodeType) {
            return new element().select(exp);
        }
    };
    element.prototype.tag = function (pos) {
        return this.calling(function (d) {
            return this.node().tagName;
        }, pos);
    };
    element.prototype.node = function (pos) {
        if (pos != null) {
            return this.selected[pos];
        } else {
            if (this.selected.length == 1) {
                return this.selected[0];
            }
            return this.selected;
        }
    };
    element.prototype.is = function (exp, pos) {
        if (pos != null) {
            return element.selectionMatchs(this.node(pos), exp);
        }
        var b = true;
        this.selected.forEach(function (d) {
            b = b && element.selectionMatchs(d, exp);
        });
        return b;
    };
    element.prototype.first = function () {
        return this.at(0);
    };
    element.prototype.lt = function (pos) {
        this.selected = this.selected.slice(0, pos);
        return this;
    };
    element.prototype.gt = function (pos) {
        this.selected = this.selected.slice(pos);
        return this;
    };
    element.prototype.odd = function () {
        var r = [];
        this.selected.forEach(function (d, i) {
            if (i % 2 !== 0) {
                r.push(d);
            }
        });
        this.selected = r;
        return this;
    };
    element.prototype.eve = function () {
        var r = [];
        this.selected.forEach(function (d, i) {
            if (i % 2 === 0) {
                r.push(d);
            }
        });
        this.selected = r;
        return this;
    };
    element.prototype.last = function () {
        return this.at(this.length - 1);
    };
    element.prototype.at = function (pos) {
        this.selected = [this.selected[pos]];
        return this;
    };
    element.prototype.eq = function (eq) {
        if (typeof eq === "number") {
            return this.at(eq);
        } else if (typeof eq === "string") {
            return this.filter(eq);
        } else {
            return eq.call(this, null);
        }
    };
    element.prototype.filter = function (exp) {
        if (exp instanceof Function) {
            return this.each(exp);
        }
        var b = [];
        this.selected.forEach(function (d) {
            if (selectionMatchs(d, exp)) {
                b.push(d);
            }
        });
        this.selected = b;
        return this;
    };
    element.prototype.next = function (exp, pos) {
        var rs = [];
        this.call(function (d) {
            element.next(d).forEach(function (d) {
                if (d)
                    rs.push(d);
            });
        }, pos);
        this.selected = rs;
        return exp ? this.filter(exp) : this;
    };
    element.prototype.prev = function (exp, pos) {
        var rs = [];
        this.call(function (d) {
            element.prev(d).forEach(function (d) {
                if (d)
                    rs.push(d);
            });
        }, pos);
        this.selected = rs;
        return exp ? this.filter(exp) : this;
    };
    element.prototype.children = function (exp, pos) {
        var rs = [];
        var rns = new element();
        this.call(function (d) {
            element.children(d).forEach(function (d) {
                if (d)
                    rs.push(d);
            });
        }, pos);
        rns.selected = rs;
        return exp ? rns.filter(exp) : rns;
    };
    element.prototype.parent = function (exp, pos) {
        var z = new base.element();
        this.call(function (d) {
            z.selected.push(d.parentNode);
        }, pos);
        //this.selected = rs;
        return exp ? z.filter(exp) : z;
    };
    element.prototype.insertBefore = function (dom, after) {
        var node = dom instanceof element ? dom.node() : element.parseHtml(dom);
        this.each(function () {
            if (this.insertBefore) {
                this.insertBefore(node, after);
            }
        });
        return this;
    };
    element.prototype.append = function (name) {
        if (name instanceof element) {
            var me = this;
            name.each(function () {
                var self = this;
                me.each(function () {
                    this.appendChild(self);
                });
            });
            return this;
        } else if (name.nodeType) {
            this.each(function () {
                this.appendChild(name);
            });
        } else if (name.length && name[0].length && "string" !== typeof name) {
            var me = this;
            name.each(function () {
                var self = this;
                me.each(function () {
                    this.appendChild(self);
                });
            });
            return this;
        } else {
            name = element.parseHtml(name);
            return this.each(function () {
                return this.appendChild(name);
            });
        }
    };
    element.prototype.appendTo = function (name) {
        var me = this;
        if (name instanceof element) {
            name.each(function () {
                var self = this;
                me.each(function () {
                    self.appendChild(this);
                });
            });
            return this;
        } else {
            name = element.select(name);
            name.each(function () {
                var self = this;
                me.each(function () {
                    self.appendChild(this);
                });
            });
        }
        return this;
    };
    element.prototype.empty = function () {
        this.html("");
        return this;
    };
    element.prototype.has = function (node) {
        if (!arguments.length || !this.selected.length) { return false; }
        if (node.forEach) {
            var self = this;
            var b = true;
            node.forEach(function (item) {
                self.each(function (j) {
                    b = b && item.isEqualNode ? item.isEqualNode(j) : item === j;
                });
            });
            return b;
        } else {
            var b = false;
            for (var i = 0; i < this.selected.length; i++) {
                b = b || node.isEqualNode ? node.isEqualNode(this.selected[i]) : node === this.selected[i];
                if (b) { break }
            }
            return b;
        }
    }
    element.select = function (exp, context) {
        return new element().select(exp, context);
    }
    element.parseHtml = function (tag) {
        if (typeof tag !== "string") {
            return tag;
        }
        var obj = null;
        tag = tag.trim();
        if (/^\S*<tr/i.test(tag)) {
            obj = document.createElement("tbody");
            obj.innerHTML = tag;
        } else if (/^\S*<td/i.test(tag)) {
            obj = document.createElement("tr");
            obj.innerHTML = tag;
        } else if (/^\S*<li/i.test(tag)) {
            obj = document.createElement("ul");
            obj.innerHTML = tag;
        } else if (/^(altGlyph|altGlyphDef|altGlyphItem|animate|animateColor|animateMotion|animateTransform|circle|clipPath|color\-profile|cursor|definition\-src|defs|desc|ellipse|feBlend|feColorMatrix|feComponentTransfer|feComposite|feConvolveMatrix|feDiffuseLighting|feDisplacementMap|feDistantLight|feFlood|feFuncA|feFuncB|feFuncG|feFuncR|feGaussianBlur|feImage|feMerge|feMergeNode|feMorphology|feOffset|fePointLight|feSpecularLighting|feSpotLight|feTile|feTurbulence|filter|font|font\-face|font\-face\-format|font\-face\-name|font\-face\-src|font\-face\-uri|foreignObject|g|glyph|glyphRef|hkern|image|line|linearGradient|marker|mask|metadata|missing\-glyph|mpath|path|pattern|polygon|polyline|radialGradient|rect|set|stop|svg|switch|symbol|text|textPath|title|tref|tspan|use|view|vkern)/i.test(tag)) {
            var xmlnames = {
                ns: 'http://www.w3.org/2000/svg',
                xmlns: 'http://www.w3.org/2000/xmlns/',
                xlink: 'http://www.w3.org/1999/xlink'
            };
            var element = document.createElementNS(xmlnames.ns, tag);
        } else if (/^\S*</.test(tag)) {
            obj = document.createElement("div");
            obj.innerHTML = tag;
        } else {
            return document.createElement(tag);
        }
        //var items = obj.getElementsByTagName("script");
        //if (items.length) {
        //    for (var i = 0; i < items.length; i++) {
        //        if (items[0].innerHTML) { eval(items[0].innerHTML); }
        //    };
        //}
        return obj.children[0];
    }
    element.next = function (d) {
        var r = d, rs = [];
        while ((r = r.nextSibling) != null) {
            rs.push(r);
        }
        return rs;
    };
    element.children = function (d) {
        var rs = [];
        var cs = d.children;//window.document.children
        for (var i = 0; i < cs.length; i++) {
            rs.push(cs[i]);
        }
        return rs;
    };
    element.prev = function (d) {
        var r = d, rs = [];
        while ((r = r.previousSibling) != null) {
            rs.push(r);
        }
        return rs;
    };
    element.DOM = {
        getUnitType: function (property) {
            if (/^(rotate|skew)/i.test(property)) {
                return "deg";
            } else if (/(^(scale|scaleX|scaleY|scaleZ|alpha|flexGrow|flexHeight|zIndex|fontWeight)$)|((opacity|red|green|blue|alpha)$)/i.test(property)) {
                return "";
            } else {
                return "px";
            }
        },
        getUnitFormatType: function (property) {
            if (/^(rotate|skew)/i.test(property)) {
                return property + "({0}deg)";
            } else if (/(^(alpha|flexGrow|flexHeight|zIndex|fontWeight)$)|((opacity|red|green|blue|alpha)$)/i.test(property)) {
                return "{0}";
            } else if (/^(scale|scaleX|scaleY|scaleZ)/.test(property)) {
                return property + "({0})";
            } else {
                return "{0}px";
            }
        },
        getDisplayType: function (element) {
            var tagName = element && element.tagName.toString().toLowerCase();

            if (/^(b|big|i|small|tt|abbr|acronym|cite|code|dfn|em|kbd|strong|samp|var|a|bdo|br|img|map|object|q|script|span|sub|sup|button|input|label|select|textarea)$/i.test(tagName)) {
                return "inline";
            } else if (/^(li)$/i.test(tagName)) {
                return "list-item";
            } else if (/^(tr)$/i.test(tagName)) {
                return "table-row";
            } else if (/^(table)$/i.test(tagName)) {
                return "table";
            } else if (/^(tbody)$/i.test(tagName)) {
                return "table-row-group";
                /* Default to "block" when no match is found. */
            } else {
                return "block";
            }
        },
        parseProperty: function (property) {
            var s = property.split("-"), i = 0, l = s.length, p = "";
            for (; i < l; i++) {
                if (i === 0) {
                    p = s[i].toLowerCase();
                } else {
                    p += s[i].charAt(0).toUpperCase() + s[i].slice(1);
                }
            }
            return p;
        },
        extractValue: function (value, property) {
            return parseFloat(value);
        },
        getHeightWidth: function (dom, prop) {
            var v = "", p;
            if ((v = dom.css(prop)) == "auto" && (v = dom.css("max-" + prop), v != "auto" ? dom.property("_animation_toggle_fix", (p = {} || dom.property("_animation_toggle_fix"))["max-" + prop] = v != "auto" ? v : "") : "") == "auto") {
                this.getHeightWidth(dom.parent(), prop);
            } else {
                return v;
            }
        },
        nsPrefix: {
            svg: "http://www.w3.org/2000/svg",
            xhtml: "http://www.w3.org/1999/xhtml",
            xlink: "http://www.w3.org/1999/xlink",
            xml: "http://www.w3.org/XML/1998/namespace",
            xmlns: "http://www.w3.org/2000/xmlns/"
        },
        qualify: function (name) {
            var i = name.indexOf(":"), prefix = name;
            if (i >= 0 && (prefix = name.slice(0, i)) !== "xmlns")
                name = name.slice(i + 1);
            return this.nsPrefix.hasOwnProperty(prefix) ? {
                space: this.nsPrefix[prefix],
                local: name
            } : name;
        },
        camelCase: function (string) {
            return string.replace(/^-ms-/, "ms-").replace(/-([\da-z])/gi, fcamelCase);
        },
        _getStyle: function (elem) {
            // Support: IE<=11+, Firefox<=30+ (#15098, #14150)
            // IE throws on elements created in popups
            // FF meanwhile throws on frame elements through "defaultView.getComputedStyle"

            if (elem.ownerDocument && elem.ownerDocument.defaultView.opener) {
                return elem.ownerDocument.defaultView.getComputedStyle(elem, null);
            }
            return window.getComputedStyle(elem, null);
        },
        attr: function (name, value) {
            name = element.DOM.qualify(name);
            function attrNull() {
                this.removeAttribute(name);
            }
            function attrNullNS() {
                this.removeAttributeNS(name.space, name.local);
            }
            function attrConstant() {
                this.setAttribute(name, value);
            }
            function attrConstantNS() {
                this.setAttributeNS(name.space, name.local, value);
            }
            function attrFunction() {
                var x = value.apply(this, arguments);
                if (x === null)
                    this.removeAttribute(name);
                else
                    this.setAttribute(name, x);
            }
            function attrFunctionNS() {
                var x = value.apply(this, arguments);
                if (x === null)
                    this.removeAttributeNS(name.space, name.local);
                else
                    this.setAttributeNS(name.space, name.local, x);
            }
            return value === null ? name.local ? attrNullNS : attrNull : typeof value === "function" ? name.local ? attrFunctionNS : attrFunction : name.local ? attrConstantNS : attrConstant;
        }
    };
    element.prototype.removeAttr = function (name) {
        return this.each(function () {
            this.removeAttribute(name);
        });
    }
    element.scrollTop = function () {
        var scrollPos;
        if (window.pageYOffset) {
            scrollPos = window.pageYOffset;
        } else if (document.compatMode && document.compatMode != 'BackCompat') {
            scrollPos = document.documentElement.scrollTop;
        } else if (document.body) {
            scrollPos = document.body.scrollTop;
        }
        return scrollPos;
    };
    element.scrollLeft = function () {
        var scrollPos;
        if (window.pageXOffset) {
            scrollPos = window.pageXOffset;
        } else if (document.compatMode && document.compatMode != 'BackCompat') {
            scrollPos = document.documentElement.scrollLeft;
        } else if (document.body) {
            scrollPos = document.body.scrollLeft;
        }
        return scrollPos;
    };
    element.drag = function (moveDom, bindDom, opacity) {
        var bindDom = typeof bindDom === "string" ? element.select(bindDom) : bindDom || this._ns;
        var opacity = opacity ? opacity : 1;
        var moveDom = moveDom ? typeof moveDom === "string" ? element.select(moveDom) : moveDom : bindDom;
        var resumPointer = "";
        function parseInt1(o) {
            var i = parseInt(o);
            return isNaN(i) ? 0 : i;
        }
        var me = this;
        var listen = function () {
            element.select(bindDom).on("mousedown", function (a) {
                var o = moveDom.node ? moveDom.node() : moveDom;
                var d = document; if (!a) a = window.event;
                if (!a.pageX) a.pageX = a.clientX;
                if (!a.pageY) a.pageY = a.clientY;
                var x = a.pageX, y = a.pageY;
                if (o.setCapture)
                    o.setCapture();
                else if (window.captureEvents)
                    window.captureEvents.call(window, Event["MOUSEMOVE"] | Event["MOUSEUP"]);
                var backData = { x: moveDom.css("top"), y: moveDom.css("left") };
                resumPointer = moveDom.css("cursor");
                moveDom.css({ "cursor": "pointer" });
                function move(a) {
                    if (!a) a = window.event;
                    if (!a.pageX) a.pageX = a.clientX;
                    if (!a.pageY) a.pageY = a.clientY;
                    var tx = a.pageX - x + parseInt1(moveDom.css("left")), ty = a.pageY - y + parseInt1(moveDom.css("top"));
                    moveDom.css({ left: tx + "px", top: ty + "px" });
                    x = a.pageX;
                    y = a.pageY;
                }
                function up(a) {
                    if (!a) a = window.event;
                    if (o.releaseCapture)
                        o.releaseCapture();
                    else if (window.captureEvents)
                        window.captureEvents.call(window, Event["MOUSEMOVE"] | Event["MOUSEUP"]);
                    d.onmousemove = null;
                    d.onmouseup = null;
                    if (!a.pageX) a.pageX = a.clientX;
                    if (!a.pageY) a.pageY = a.clientY;
                    if (!document.body["pageWidth"]) document.body["pageWidth"] = document.body.clientWidth;
                    if (!document.body["pageHeight"]) document.body["pageHeight"] = document.body.clientHeight;
                    setTimeout(function () { moveDom.css({ "cursor": resumPointer }); }, 0);
                    element.select(d).off("mousemove", move);
                    element.select(d).off("mouseup", up);
                }
                element.select(d).on("mousemove", move);
                element.select(d).on("mouseup", up);
            });
        }
        if (bindDom) {
            listen();
        }
    }
    element.layer = function (conf) {
        if (!conf) {
            conf = { title: "Popup Layer 弹出框", iconClass: ".fa .fa-dialog", loading: false, left: "13.5%", top: "240px", height: "280px;", width: "60%", titleCss: "" };
        }
        var layer = {
            title: conf.title || "Popup Layer 弹出框",
            css: {
                left: conf.left || "13.5%",
                top: conf.top || "240px",
                height: conf.height || "280px;",
                width: conf.width || "60%",
                bottom: conf.bottom || "auto",
                right: conf.right || "auto",
            },
            icon: conf.icon || "fa-vcard",
            titleCss: conf.titleCss || {},
            loading: conf.loading == null || conf.loading == undefined ? true : !!conf.loading,
            drag: conf.drag == null || conf.drag == undefined ? true : conf.drag,
            enableLanding: conf.enableLanding == null || conf.enableLanding == undefined ? true : !!conf.enableLanding,
            //dispatch: emitter({}),//.dispatch("modechanged", "drag"),
            status: conf.status || "simple"
        }
        emitter(this);
        if (conf.css) {
            layer.css = element.merge(layer.css, conf.css);
        }
        var htmltemplate = '<div class="pop-lay" style="display:none;  position:fixed; min-height:280px; background-color:white;  border-color:#e0e0e0; border-width:0.1px;border-style:solid;box-shadow:gray 0 0 30px;"><div class="layer-landing" style=" margin-bottom:1px;"></div><div class="info-bar" style="cursor:pointer;border-left-color:rgb(0,156,204); border-left-width:3px; border-left-style:solid; margin-left:0px; cursor:pointer; height:19px;"><div class="info-title" style="cursor:pointer; text-overflow:ellipsis; line-height:18px; font-size:12px; cursor:auto; margin-left:1px; float:left; padding-left:3px; color:#808080;margin-top:3px;"><span class="info-title-icon fa" style="font-size:14px; width:15px; height:15px; line-height:15px;"></span><span class="info-title-content" style=" font-size:14pxheight:15px; line-height:15px;margin-left:5px;"></span></div><div class="top_layer_menu_icon" style="float:left;font-size:11px;"><div class="hover-color screen-close" style="height:15px;width:15px;float:right;font-size:12px; margin-right:5px; cursor:pointer;"><i class="fa fa-close" style="width:15px;height:15px;text-align:center;line-height:15px;"></i></div><div class="hover-color window-maximize" style="display:none; height:15px;width:15px;float:right; margin-right:5px; cursor:pointer;"><i class="fa fa-expand" style="width:15px;height:15px;text-align:center;line-height:15px;"></i></div><div class="hover-color window-minimize" style=" display:none; height:15px;width:15px;float:right; margin-right:5px; cursor:pointer;"><i class="fa fa-compress" style="width:15px;height:15px;text-align:center;line-height:15px;"></i></div></div><div style=" clear:both;"></div></div><div class="layer-content"></div></div>';

        layer.render = function (fn) {

            var context = element.select(element.parseHtml(htmltemplate));
            if (layer.drag) {
                element.drag(context, context.find(".info-title"));
            }
            if (layer.enableLanding) {
                layer.landing = element.landingline({ parent: context.find(".layer-landing") });
                if (!layer.loading) {
                    layer.landing.hide();
                }
            }
            layer.mode = "normal";
            context.css(layer.css);
            if (layer.title) {
                context.find(".info-title-content").html(layer.title);
            }
            if (fn instanceof Function) {
                fn.call(context, layer, context.find(".layer-content"));
            }
            else if (fn instanceof String) {
                context.find(".layer-content").html(fn);
            }
            context.find(".screen-close").show().on("click", function () {
                layer.close();
            });
            if (layer.status != "simple") {
                context.find(".window-minimize").on("click", function () {
                    layer.min();
                });
                context.find(".window-maximize").show().on("click", function () {
                    layer.max();
                });
            }
            if (layer.icon) {
                context.find(".info-title-icon").addClass(layer.icon);
            }
            context.appendTo("body"); layer.mode = "hided";
            layer.context = context;
            if (layer._content) {
                layer.context.find(".layer-content").append(element.parseHtml(layer._content));
            }
            return layer;
        }
        layer.min = function () {
            var premode = layer.mode;
            layer.mode = "min";

            if (premode == "closed") {
                layer.render();
            }
            layer.hide();
            //layer.dispatch.modechanged.call(layer, layer.mode, premode);
            layer.emit("modechanged", layer.mode, premode);
            return layer;
        }
        layer.restore = function () {
            var premode = layer.mode;
            if (premode == "closed") {
                layer.render();
                layer.mode = "normal";
                layer.show();
            } else if (premode == "hided") {
                layer.show();
                layer.mode = "normal";
            }
            layer.context.css(layer.css);
            //layer.dispatch.modechanged.call(layer, layer.mode, premode);
            layer.emit("modechanged", layer.mode, premode);
            return layer;
        }
        layer.max = function () {
            var premode = layer.mode;
            layer.mode = "normal";
            if (premode == "closed") {
                layer.render();
            }
            layer.context.css({ width: "100%", height: "100%", left: 0, top: 0, right: 0, bottom: 0 });
            //layer.dispatch.modechanged.call(layer, layer.mode, premode);
            layer.emit("modechanged", layer.mode, premode);
            return layer;
        }
        layer.close = function () {
            var premode = layer.mode;
            layer.mode = "closed";
            //if (premode == "closed") {
            //    layer.render();
            //}
            layer.context.remove();
            //delete layer.context;

            //layer.dispatch.modechanged.call(layer, layer.mode, premode);
            layer.emit("modechanged", layer.mode, premode);
            return layer;
        }
        layer.show = function () {
            var premode = layer.mode;
            if (premode == "closed") {
                layer.render();
            }
            layer.context.show();
            layer.mode = "showed";

            var w = layer.context.find(".info-bar").getBoundingWidth();
            var w2 = layer.context.find(".top_layer_menu_icon").getBoundingWidth();
            layer.context.find(".info-title").css({ width: (w - w2 - 10) + "px" });
            //top_layer_menu_icon
            //layer.dispatch.modechanged.call(layer, layer.mode, premode);
            layer.emit("modechanged", layer.mode, premode);
            return layer;
        }
        layer.hide = function () {
            var premode = layer.mode;
            layer.mode = "hided";
            layer.context.hide();
            //layer.dispatch.modechanged.call(layer, "hided", premode);
            layer.emit("modechanged", layer.mode, premode);
            return layer;
        }
        layer.html = function (html) {
            if (html) {
                layer._content = html;
                if (layer._content) {
                    layer.context.find(".layer-content").append(element.parseHtml(layer._content));
                }
                return this;
            }
            return this;
        }
        layer.mode = "hided";
        return layer;
    };
    /**
  * Tween.js - Licensed under the MIT license
  * https://github.com/tweenjs/tween.js
  * ----------------------------------------------
  * See https://github.com/tweenjs/tween.js/graphs/contributors for the full list of contributors.
  * Thank you all, you're awesome!
  */
    (function () {
        var lastTime = 0;
        var vendors = ['ms', 'moz', 'webkit', 'o'];
        for (var x = 0; x < vendors.length && !window.requestAnimationFrame; ++x) {
            window.requestAnimationFrame = window[vendors[x] + 'RequestAnimationFrame'];
            window.cancelAnimationFrame = window[vendors[x] + 'CancelAnimationFrame']
                || window[vendors[x] + 'CancelRequestAnimationFrame'];
        }
        if (!window.requestAnimationFrame)
            window.requestAnimationFrame = function (callback, element) {
                var currTime = new Date().getTime();
                var timeToCall = Math.max(0, 16 - (currTime - lastTime));
                var id = window.setTimeout(function () { callback(currTime + timeToCall); },
                    timeToCall);
                lastTime = currTime + timeToCall;
                return id;
            };
        if (!window.cancelAnimationFrame)
            window.cancelAnimationFrame = function (id) {
                clearTimeout(id);
            };
    }());
    base.animateFrame = window.requestAnimationFrame;
    base.cancelAnimationFrame = window.cancelAnimationFrame;
    base.Base64 = function () {

        // private property
        _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

        // public method for encoding
        this.encode = function (input) {
            var output = "";
            var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
            var i = 0;
            input = _utf8_encode(input);
            while (i < input.length) {
                chr1 = input.charCodeAt(i++);
                chr2 = input.charCodeAt(i++);
                chr3 = input.charCodeAt(i++);
                enc1 = chr1 >> 2;
                enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
                enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
                enc4 = chr3 & 63;
                if (isNaN(chr2)) {
                    enc3 = enc4 = 64;
                } else if (isNaN(chr3)) {
                    enc4 = 64;
                }
                output = output +
                    _keyStr.charAt(enc1) + _keyStr.charAt(enc2) +
                    _keyStr.charAt(enc3) + _keyStr.charAt(enc4);
            }
            return output;
        }

        // public method for decoding
        this.decode = function (input) {
            var output = "";
            var chr1, chr2, chr3;
            var enc1, enc2, enc3, enc4;
            var i = 0;
            input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");
            while (i < input.length) {
                enc1 = _keyStr.indexOf(input.charAt(i++));
                enc2 = _keyStr.indexOf(input.charAt(i++));
                enc3 = _keyStr.indexOf(input.charAt(i++));
                enc4 = _keyStr.indexOf(input.charAt(i++));
                chr1 = (enc1 << 2) | (enc2 >> 4);
                chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
                chr3 = ((enc3 & 3) << 6) | enc4;
                output = output + String.fromCharCode(chr1);
                if (enc3 != 64) {
                    output = output + String.fromCharCode(chr2);
                }
                if (enc4 != 64) {
                    output = output + String.fromCharCode(chr3);
                }
            }
            output = _utf8_decode(output);
            return output;
        }

        // private method for UTF-8 encoding
        _utf8_encode = function (string) {
            string = string.replace(/\r\n/g, "\n");
            var utftext = "";
            for (var n = 0; n < string.length; n++) {
                var c = string.charCodeAt(n);
                if (c < 128) {
                    utftext += String.fromCharCode(c);
                } else if ((c > 127) && (c < 2048)) {
                    utftext += String.fromCharCode((c >> 6) | 192);
                    utftext += String.fromCharCode((c & 63) | 128);
                } else {
                    utftext += String.fromCharCode((c >> 12) | 224);
                    utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                    utftext += String.fromCharCode((c & 63) | 128);
                }

            }
            return utftext;
        }

        // private method for UTF-8 decoding
        _utf8_decode = function (utftext) {
            var string = "";
            var i = 0;
            var c = c1 = c2 = 0;
            while (i < utftext.length) {
                c = utftext.charCodeAt(i);
                if (c < 128) {
                    string += String.fromCharCode(c);
                    i++;
                } else if ((c > 191) && (c < 224)) {
                    c2 = utftext.charCodeAt(i + 1);
                    string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                    i += 2;
                } else {
                    c2 = utftext.charCodeAt(i + 1);
                    c3 = utftext.charCodeAt(i + 2);
                    string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                    i += 3;
                }
            }
            return string;
        }
    }
    var TWEEN = TWEEN || (function () {

        var _tweens = [];

        return {

            getAll: function () {

                return _tweens;

            },

            removeAll: function () {

                _tweens = [];

            },

            add: function (tween) {

                _tweens.push(tween);

            },

            remove: function (tween) {

                var i = _tweens.indexOf(tween);

                if (i !== -1) {
                    _tweens.splice(i, 1);
                }

            },

            update: function (time, preserve) {

                if (_tweens.length === 0) {
                    return false;
                }

                var i = 0;

                time = time !== undefined ? time : TWEEN.now();

                while (i < _tweens.length) {

                    if (_tweens[i].update(time) || preserve) {
                        i++;
                    } else {
                        _tweens.splice(i, 1);
                    }

                }

                return true;

            }
        };

    })();

    // Include a performance.now polyfill.
    // In node.js, use process.hrtime.
    if (typeof (window) === 'undefined' && typeof (process) !== 'undefined') {
        TWEEN.now = function () {
            var time = process.hrtime();

            // Convert [seconds, nanoseconds] to milliseconds.
            return time[0] * 1000 + time[1] / 1000000;
        };
    }
    // In a browser, use window.performance.now if it is available.
    else if (typeof (window) !== 'undefined' &&
        window.performance !== undefined &&
        window.performance.now !== undefined) {
        // This must be bound, because directly assigning this function
        // leads to an invocation exception in Chrome.
        TWEEN.now = window.performance.now.bind(window.performance);
    }
    // Use Date.now if it is available.
    else if (Date.now !== undefined) {
        TWEEN.now = Date.now;
    }
    // Otherwise, use 'new Date().getTime()'.
    else {
        TWEEN.now = function () {
            return new Date().getTime();
        };
    }
    TWEEN.Tween = function (object) {

        var _object = object;
        var _valuesStart = {};
        var _valuesEnd = {};
        var _valuesStartRepeat = {};
        var _duration = 1000;
        var _repeat = 0;
        var _repeatDelayTime;
        var _yoyo = false;
        var _isPlaying = false;
        var _reversed = false;
        var _delayTime = 0;
        var _startTime = null;
        var _easingFunction = TWEEN.Easing.Linear.None;
        var _interpolationFunction = TWEEN.Interpolation.Linear;
        var _chainedTweens = [];
        var _onStartCallback = null;
        var _onStartCallbackFired = false;
        var _onUpdateCallback = null;
        var _onCompleteCallback = null;
        var _onStopCallback = null;

        this.to = function (properties, duration) {

            _valuesEnd = properties;

            if (duration !== undefined) {
                _duration = duration;
            }

            return this;

        };

        this.start = function (time) {

            TWEEN.add(this);

            _isPlaying = true;

            _onStartCallbackFired = false;

            _startTime = time !== undefined ? time : TWEEN.now();
            _startTime += _delayTime;

            for (var property in _valuesEnd) {

                // Check if an Array was provided as property value
                if (_valuesEnd[property] instanceof Array) {

                    if (_valuesEnd[property].length === 0) {
                        continue;
                    }

                    // Create a local copy of the Array with the start value at the front
                    _valuesEnd[property] = [_object[property]].concat(_valuesEnd[property]);

                }

                // If `to()` specifies a property that doesn't exist in the source object,
                // we should not set that property in the object
                if (_object[property] === undefined) {
                    continue;
                }

                // Save the starting value.
                _valuesStart[property] = _object[property];

                if ((_valuesStart[property] instanceof Array) === false) {
                    _valuesStart[property] *= 1.0; // Ensures we're using numbers, not strings
                }

                _valuesStartRepeat[property] = _valuesStart[property] || 0;

            }

            return this;

        };

        this.stop = function () {

            if (!_isPlaying) {
                return this;
            }

            TWEEN.remove(this);
            _isPlaying = false;

            if (_onStopCallback !== null) {
                _onStopCallback.call(_object, _object);
            }

            this.stopChainedTweens();
            return this;

        };

        this.end = function () {

            this.update(_startTime + _duration);
            return this;

        };

        this.stopChainedTweens = function () {

            for (var i = 0, numChainedTweens = _chainedTweens.length; i < numChainedTweens; i++) {
                _chainedTweens[i].stop();
            }

        };

        this.delay = function (amount) {

            _delayTime = amount;
            return this;

        };

        this.repeat = function (times) {

            _repeat = times;
            return this;

        };

        this.repeatDelay = function (amount) {

            _repeatDelayTime = amount;
            return this;

        };

        this.yoyo = function (yoyo) {

            _yoyo = yoyo;
            return this;

        };


        this.easing = function (easing) {

            _easingFunction = easing;
            return this;

        };

        this.interpolation = function (interpolation) {

            _interpolationFunction = interpolation;
            return this;

        };

        this.chain = function () {

            _chainedTweens = arguments;
            return this;

        };

        this.onStart = function (callback) {

            _onStartCallback = callback;
            return this;

        };

        this.onUpdate = function (callback) {

            _onUpdateCallback = callback;
            return this;

        };

        this.onComplete = function (callback) {

            _onCompleteCallback = callback;
            return this;

        };

        this.onStop = function (callback) {

            _onStopCallback = callback;
            return this;

        };

        this.update = function (time) {

            var property;
            var elapsed;
            var value;

            if (time < _startTime) {
                return true;
            }

            if (_onStartCallbackFired === false) {

                if (_onStartCallback !== null) {
                    _onStartCallback.call(_object, _object);
                }

                _onStartCallbackFired = true;
            }

            elapsed = (time - _startTime) / _duration;
            elapsed = elapsed > 1 ? 1 : elapsed;

            value = _easingFunction(elapsed);

            for (property in _valuesEnd) {

                // Don't update properties that do not exist in the source object
                if (_valuesStart[property] === undefined) {
                    continue;
                }

                var start = _valuesStart[property] || 0;
                var end = _valuesEnd[property];

                if (end instanceof Array) {

                    _object[property] = _interpolationFunction(end, value);

                } else {

                    // Parses relative end values with start as base (e.g.: +10, -3)
                    if (typeof (end) === 'string') {

                        if (end.charAt(0) === '+' || end.charAt(0) === '-') {
                            end = start + parseFloat(end);
                        } else {
                            end = parseFloat(end);
                        }
                    }

                    // Protect against non numeric properties.
                    if (typeof (end) === 'number') {
                        _object[property] = start + (end - start) * value;
                    }

                }

            }

            if (_onUpdateCallback !== null) {
                _onUpdateCallback.call(_object, value);
            }

            if (elapsed === 1) {

                if (_repeat > 0) {

                    if (isFinite(_repeat)) {
                        _repeat--;
                    }

                    // Reassign starting values, restart by making startTime = now
                    for (property in _valuesStartRepeat) {

                        if (typeof (_valuesEnd[property]) === 'string') {
                            _valuesStartRepeat[property] = _valuesStartRepeat[property] + parseFloat(_valuesEnd[property]);
                        }

                        if (_yoyo) {
                            var tmp = _valuesStartRepeat[property];

                            _valuesStartRepeat[property] = _valuesEnd[property];
                            _valuesEnd[property] = tmp;
                        }

                        _valuesStart[property] = _valuesStartRepeat[property];

                    }

                    if (_yoyo) {
                        _reversed = !_reversed;
                    }

                    if (_repeatDelayTime !== undefined) {
                        _startTime = time + _repeatDelayTime;
                    } else {
                        _startTime = time + _delayTime;
                    }

                    return true;

                } else {

                    if (_onCompleteCallback !== null) {

                        _onCompleteCallback.call(_object, _object);
                    }

                    for (var i = 0, numChainedTweens = _chainedTweens.length; i < numChainedTweens; i++) {
                        // Make the chained tweens start exactly at the time they should,
                        // even if the `update()` method was called way past the duration of the tween
                        _chainedTweens[i].start(_startTime + _duration);
                    }

                    return false;

                }

            }

            return true;

        };

    };

    TWEEN.Easing = {

        Linear: {

            None: function (k) {

                return k;

            }

        },

        Quadratic: {

            In: function (k) {

                return k * k;

            },

            Out: function (k) {

                return k * (2 - k);

            },

            InOut: function (k) {

                if ((k *= 2) < 1) {
                    return 0.5 * k * k;
                }

                return - 0.5 * (--k * (k - 2) - 1);

            }

        },

        Cubic: {

            In: function (k) {

                return k * k * k;

            },

            Out: function (k) {

                return --k * k * k + 1;

            },

            InOut: function (k) {

                if ((k *= 2) < 1) {
                    return 0.5 * k * k * k;
                }

                return 0.5 * ((k -= 2) * k * k + 2);

            }

        },

        Quartic: {

            In: function (k) {

                return k * k * k * k;

            },

            Out: function (k) {

                return 1 - (--k * k * k * k);

            },

            InOut: function (k) {

                if ((k *= 2) < 1) {
                    return 0.5 * k * k * k * k;
                }

                return - 0.5 * ((k -= 2) * k * k * k - 2);

            }

        },

        Quintic: {

            In: function (k) {

                return k * k * k * k * k;

            },

            Out: function (k) {

                return --k * k * k * k * k + 1;

            },

            InOut: function (k) {

                if ((k *= 2) < 1) {
                    return 0.5 * k * k * k * k * k;
                }

                return 0.5 * ((k -= 2) * k * k * k * k + 2);

            }

        },

        Sinusoidal: {

            In: function (k) {

                return 1 - Math.cos(k * Math.PI / 2);

            },

            Out: function (k) {

                return Math.sin(k * Math.PI / 2);

            },

            InOut: function (k) {

                return 0.5 * (1 - Math.cos(Math.PI * k));

            }

        },

        Exponential: {

            In: function (k) {

                return k === 0 ? 0 : Math.pow(1024, k - 1);

            },

            Out: function (k) {

                return k === 1 ? 1 : 1 - Math.pow(2, - 10 * k);

            },

            InOut: function (k) {

                if (k === 0) {
                    return 0;
                }

                if (k === 1) {
                    return 1;
                }

                if ((k *= 2) < 1) {
                    return 0.5 * Math.pow(1024, k - 1);
                }

                return 0.5 * (- Math.pow(2, - 10 * (k - 1)) + 2);

            }

        },

        Circular: {

            In: function (k) {

                return 1 - Math.sqrt(1 - k * k);

            },

            Out: function (k) {

                return Math.sqrt(1 - (--k * k));

            },

            InOut: function (k) {

                if ((k *= 2) < 1) {
                    return - 0.5 * (Math.sqrt(1 - k * k) - 1);
                }

                return 0.5 * (Math.sqrt(1 - (k -= 2) * k) + 1);

            }

        },

        Elastic: {

            In: function (k) {

                if (k === 0) {
                    return 0;
                }

                if (k === 1) {
                    return 1;
                }

                return -Math.pow(2, 10 * (k - 1)) * Math.sin((k - 1.1) * 5 * Math.PI);

            },

            Out: function (k) {

                if (k === 0) {
                    return 0;
                }

                if (k === 1) {
                    return 1;
                }

                return Math.pow(2, -10 * k) * Math.sin((k - 0.1) * 5 * Math.PI) + 1;

            },

            InOut: function (k) {

                if (k === 0) {
                    return 0;
                }

                if (k === 1) {
                    return 1;
                }

                k *= 2;

                if (k < 1) {
                    return -0.5 * Math.pow(2, 10 * (k - 1)) * Math.sin((k - 1.1) * 5 * Math.PI);
                }

                return 0.5 * Math.pow(2, -10 * (k - 1)) * Math.sin((k - 1.1) * 5 * Math.PI) + 1;

            }

        },

        Back: {

            In: function (k) {

                var s = 1.70158;

                return k * k * ((s + 1) * k - s);

            },

            Out: function (k) {

                var s = 1.70158;

                return --k * k * ((s + 1) * k + s) + 1;

            },

            InOut: function (k) {

                var s = 1.70158 * 1.525;

                if ((k *= 2) < 1) {
                    return 0.5 * (k * k * ((s + 1) * k - s));
                }

                return 0.5 * ((k -= 2) * k * ((s + 1) * k + s) + 2);

            }

        },

        Bounce: {

            In: function (k) {

                return 1 - TWEEN.Easing.Bounce.Out(1 - k);

            },

            Out: function (k) {

                if (k < (1 / 2.75)) {
                    return 7.5625 * k * k;
                } else if (k < (2 / 2.75)) {
                    return 7.5625 * (k -= (1.5 / 2.75)) * k + 0.75;
                } else if (k < (2.5 / 2.75)) {
                    return 7.5625 * (k -= (2.25 / 2.75)) * k + 0.9375;
                } else {
                    return 7.5625 * (k -= (2.625 / 2.75)) * k + 0.984375;
                }

            },

            InOut: function (k) {

                if (k < 0.5) {
                    return TWEEN.Easing.Bounce.In(k * 2) * 0.5;
                }

                return TWEEN.Easing.Bounce.Out(k * 2 - 1) * 0.5 + 0.5;

            }

        }

    };

    TWEEN.Interpolation = {

        Linear: function (v, k) {

            var m = v.length - 1;
            var f = m * k;
            var i = Math.floor(f);
            var fn = TWEEN.Interpolation.Utils.Linear;

            if (k < 0) {
                return fn(v[0], v[1], f);
            }

            if (k > 1) {
                return fn(v[m], v[m - 1], m - f);
            }

            return fn(v[i], v[i + 1 > m ? m : i + 1], f - i);

        },

        Bezier: function (v, k) {

            var b = 0;
            var n = v.length - 1;
            var pw = Math.pow;
            var bn = TWEEN.Interpolation.Utils.Bernstein;

            for (var i = 0; i <= n; i++) {
                b += pw(1 - k, n - i) * pw(k, i) * v[i] * bn(n, i);
            }

            return b;

        },

        CatmullRom: function (v, k) {

            var m = v.length - 1;
            var f = m * k;
            var i = Math.floor(f);
            var fn = TWEEN.Interpolation.Utils.CatmullRom;

            if (v[0] === v[m]) {

                if (k < 0) {
                    i = Math.floor(f = m * (1 + k));
                }

                return fn(v[(i - 1 + m) % m], v[i], v[(i + 1) % m], v[(i + 2) % m], f - i);

            } else {

                if (k < 0) {
                    return v[0] - (fn(v[0], v[0], v[1], v[1], -f) - v[0]);
                }

                if (k > 1) {
                    return v[m] - (fn(v[m], v[m], v[m - 1], v[m - 1], f - m) - v[m]);
                }

                return fn(v[i ? i - 1 : 0], v[i], v[m < i + 1 ? m : i + 1], v[m < i + 2 ? m : i + 2], f - i);

            }

        },

        Utils: {

            Linear: function (p0, p1, t) {

                return (p1 - p0) * t + p0;

            },

            Bernstein: function (n, i) {

                var fc = TWEEN.Interpolation.Utils.Factorial;

                return fc(n) / fc(i) / fc(n - i);

            },

            Factorial: (function () {

                var a = [1];

                return function (n) {

                    var s = 1;

                    if (a[n]) {
                        return a[n];
                    }

                    for (var i = n; i > 1; i--) {
                        s *= i;
                    }

                    a[n] = s;
                    return s;

                };

            })(),

            CatmullRom: function (p0, p1, p2, p3, t) {

                var v0 = (p2 - p0) * 0.5;
                var v1 = (p3 - p1) * 0.5;
                var t2 = t * t;
                var t3 = t * t2;

                return (2 * p1 - 2 * p2 + v0 + v1) * t3 + (- 3 * p1 + 3 * p2 - 2 * v0 - v1) * t2 + v0 * t + p1;

            }

        }

    };
    base.tween = function (target, props) {
        return new new TWEEN.Tween(target, props);
    };
    // UMD (Universal Module Definition)
    (function (root) {

        if (typeof define === 'function' && define.amd) {

            // AMD
            define([], function () {
                return TWEEN;
            });

        } else if (typeof module !== 'undefined' && typeof exports === 'object') {

            // Node.js
            module.exports = TWEEN;

        } else if (root !== undefined) {

            // Global variable
            root.TWEEN = TWEEN;

        }

    })(this);


    /*!
 * @overview es6-promise - a tiny implementation of Promises/A+.
 * @copyright Copyright (c) 2014 Yehuda Katz, Tom Dale, Stefan Penner and contributors (Conversion to ES6 API by Jake Archibald)
 * @license   Licensed under MIT license
 *            See https://raw.githubusercontent.com/stefanpenner/es6-promise/master/LICENSE
 * @version   4.0.5
 */

    (function (global, factory) {
        typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory() :
            typeof define === 'function' && define.amd ? define(factory) :
                (global.ES6Promise = factory());
    }(this, (function () {
        'use strict';

        function objectOrFunction(x) {
            return typeof x === 'function' || typeof x === 'object' && x !== null;
        }

        function isFunction(x) {
            return typeof x === 'function';
        }

        var _isArray = undefined;
        if (!Array.isArray) {
            _isArray = function (x) {
                return Object.prototype.toString.call(x) === '[object Array]';
            };
        } else {
            _isArray = Array.isArray;
        }

        var isArray = _isArray;

        var len = 0;
        var vertxNext = undefined;
        var customSchedulerFn = undefined;

        var asap = function asap(callback, arg) {
            queue[len] = callback;
            queue[len + 1] = arg;
            len += 2;
            if (len === 2) {
                // If len is 2, that means that we need to schedule an async flush.
                // If additional callbacks are queued before the queue is flushed, they
                // will be processed by this flush that we are scheduling.
                if (customSchedulerFn) {
                    customSchedulerFn(flush);
                } else {
                    scheduleFlush();
                }
            }
        };

        function setScheduler(scheduleFn) {
            customSchedulerFn = scheduleFn;
        }

        function setAsap(asapFn) {
            asap = asapFn;
        }

        var browserWindow = typeof window !== 'undefined' ? window : undefined;
        var browserGlobal = browserWindow || {};
        var BrowserMutationObserver = browserGlobal.MutationObserver || browserGlobal.WebKitMutationObserver;
        var isNode = typeof self === 'undefined' && typeof process !== 'undefined' && ({}).toString.call(process) === '[object process]';

        // test for web worker but not in IE10
        var isWorker = typeof Uint8ClampedArray !== 'undefined' && typeof importScripts !== 'undefined' && typeof MessageChannel !== 'undefined';

        // node
        function useNextTick() {
            // node version 0.10.x displays a deprecation warning when nextTick is used recursively
            // see https://github.com/cujojs/when/issues/410 for details
            return function () {
                return process.nextTick(flush);
            };
        }

        // vertx
        function useVertxTimer() {
            if (typeof vertxNext !== 'undefined') {
                return function () {
                    vertxNext(flush);
                };
            }

            return useSetTimeout();
        }

        function useMutationObserver() {
            var iterations = 0;
            var observer = new BrowserMutationObserver(flush);
            var node = document.createTextNode('');
            observer.observe(node, { characterData: true });

            return function () {
                node.data = iterations = ++iterations % 2;
            };
        }

        // web worker
        function useMessageChannel() {
            var channel = new MessageChannel();
            channel.port1.onmessage = flush;
            return function () {
                return channel.port2.postMessage(0);
            };
        }

        function useSetTimeout() {
            // Store setTimeout reference so es6-promise will be unaffected by
            // other code modifying setTimeout (like sinon.useFakeTimers())
            var globalSetTimeout = setTimeout;
            return function () {
                return globalSetTimeout(flush, 1);
            };
        }

        var queue = new Array(1000);
        function flush() {
            for (var i = 0; i < len; i += 2) {
                var callback = queue[i];
                var arg = queue[i + 1];

                callback(arg);

                queue[i] = undefined;
                queue[i + 1] = undefined;
            }

            len = 0;
        }

        function attemptVertx() {
            try {
                var r = require;
                var vertx = r('vertx');
                vertxNext = vertx.runOnLoop || vertx.runOnContext;
                return useVertxTimer();
            } catch (e) {
                return useSetTimeout();
            }
        }

        var scheduleFlush = undefined;
        // Decide what async method to use to triggering processing of queued callbacks:
        if (isNode) {
            scheduleFlush = useNextTick();
        } else if (BrowserMutationObserver) {
            scheduleFlush = useMutationObserver();
        } else if (isWorker) {
            scheduleFlush = useMessageChannel();
        } else if (browserWindow === undefined && typeof require === 'function') {
            scheduleFlush = attemptVertx();
        } else {
            scheduleFlush = useSetTimeout();
        }

        function then(onFulfillment, onRejection) {
            var _arguments = arguments;

            var parent = this;

            var child = new this.constructor(noop);

            if (child[PROMISE_ID] === undefined) {
                makePromise(child);
            }

            var _state = parent._state;

            if (_state) {
                (function () {
                    var callback = _arguments[_state - 1];
                    asap(function () {
                        return invokeCallback(_state, child, callback, parent._result);
                    });
                })();
            } else {
                subscribe(parent, child, onFulfillment, onRejection);
            }

            return child;
        }
        function resolve(object) {
            /*jshint validthis:true */
            var Constructor = this;

            if (object && typeof object === 'object' && object.constructor === Constructor) {
                return object;
            }

            var promise = new Constructor(noop);
            _resolve(promise, object);
            return promise;
        }

        var PROMISE_ID = Math.random().toString(36).substring(16);

        function noop() { }

        var PENDING = void 0;
        var FULFILLED = 1;
        var REJECTED = 2;

        var GET_THEN_ERROR = new ErrorObject();

        function selfFulfillment() {
            return new TypeError("You cannot resolve a promise with itself");
        }

        function cannotReturnOwn() {
            return new TypeError('A promises callback cannot return that same promise.');
        }

        function getThen(promise) {
            try {
                return promise.then;
            } catch (error) {
                GET_THEN_ERROR.error = error;
                return GET_THEN_ERROR;
            }
        }

        function tryThen(then, value, fulfillmentHandler, rejectionHandler) {
            try {
                then.call(value, fulfillmentHandler, rejectionHandler);
            } catch (e) {
                return e;
            }
        }

        function handleForeignThenable(promise, thenable, then) {
            asap(function (promise) {
                var sealed = false;
                var error = tryThen(then, thenable, function (value) {
                    if (sealed) {
                        return;
                    }
                    sealed = true;
                    if (thenable !== value) {
                        _resolve(promise, value);
                    } else {
                        fulfill(promise, value);
                    }
                }, function (reason) {
                    if (sealed) {
                        return;
                    }
                    sealed = true;

                    _reject(promise, reason);
                }, 'Settle: ' + (promise._label || ' unknown promise'));

                if (!sealed && error) {
                    sealed = true;
                    _reject(promise, error);
                }
            }, promise);
        }

        function handleOwnThenable(promise, thenable) {
            if (thenable._state === FULFILLED) {
                fulfill(promise, thenable._result);
            } else if (thenable._state === REJECTED) {
                _reject(promise, thenable._result);
            } else {
                subscribe(thenable, undefined, function (value) {
                    return _resolve(promise, value);
                }, function (reason) {
                    return _reject(promise, reason);
                });
            }
        }

        function handleMaybeThenable(promise, maybeThenable, then$$) {
            if (maybeThenable.constructor === promise.constructor && then$$ === then && maybeThenable.constructor.resolve === resolve) {
                handleOwnThenable(promise, maybeThenable);
            } else {
                if (then$$ === GET_THEN_ERROR) {
                    _reject(promise, GET_THEN_ERROR.error);
                } else if (then$$ === undefined) {
                    fulfill(promise, maybeThenable);
                } else if (isFunction(then$$)) {
                    handleForeignThenable(promise, maybeThenable, then$$);
                } else {
                    fulfill(promise, maybeThenable);
                }
            }
        }

        function _resolve(promise, value) {
            if (promise === value) {
                _reject(promise, selfFulfillment());
            } else if (objectOrFunction(value)) {
                handleMaybeThenable(promise, value, getThen(value));
            } else {
                fulfill(promise, value);
            }
        }

        function publishRejection(promise) {
            if (promise._onerror) {
                promise._onerror(promise._result);
            }

            publish(promise);
        }

        function fulfill(promise, value) {
            if (promise._state !== PENDING) {
                return;
            }

            promise._result = value;
            promise._state = FULFILLED;

            if (promise._subscribers.length !== 0) {
                asap(publish, promise);
            }
        }

        function _reject(promise, reason) {
            if (promise._state !== PENDING) {
                return;
            }
            promise._state = REJECTED;
            promise._result = reason;

            asap(publishRejection, promise);
        }

        function subscribe(parent, child, onFulfillment, onRejection) {
            var _subscribers = parent._subscribers;
            var length = _subscribers.length;

            parent._onerror = null;

            _subscribers[length] = child;
            _subscribers[length + FULFILLED] = onFulfillment;
            _subscribers[length + REJECTED] = onRejection;

            if (length === 0 && parent._state) {
                asap(publish, parent);
            }
        }

        function publish(promise) {
            var subscribers = promise._subscribers;
            var settled = promise._state;

            if (subscribers.length === 0) {
                return;
            }

            var child = undefined,
                callback = undefined,
                detail = promise._result;

            for (var i = 0; i < subscribers.length; i += 3) {
                child = subscribers[i];
                callback = subscribers[i + settled];

                if (child) {
                    invokeCallback(settled, child, callback, detail);
                } else {
                    callback(detail);
                }
            }

            promise._subscribers.length = 0;
        }

        function ErrorObject() {
            this.error = null;
        }

        var TRY_CATCH_ERROR = new ErrorObject();

        function tryCatch(callback, detail) {
            try {
                return callback(detail);
            } catch (e) {
                TRY_CATCH_ERROR.error = e;
                return TRY_CATCH_ERROR;
            }
        }

        function invokeCallback(settled, promise, callback, detail) {
            var hasCallback = isFunction(callback),
                value = undefined,
                error = undefined,
                succeeded = undefined,
                failed = undefined;

            if (hasCallback) {
                value = tryCatch(callback, detail);

                if (value === TRY_CATCH_ERROR) {
                    failed = true;
                    error = value.error;
                    value = null;
                } else {
                    succeeded = true;
                }

                if (promise === value) {
                    _reject(promise, cannotReturnOwn());
                    return;
                }
            } else {
                value = detail;
                succeeded = true;
            }

            if (promise._state !== PENDING) {
                // noop
            } else if (hasCallback && succeeded) {
                _resolve(promise, value);
            } else if (failed) {
                _reject(promise, error);
            } else if (settled === FULFILLED) {
                fulfill(promise, value);
            } else if (settled === REJECTED) {
                _reject(promise, value);
            }
        }

        function initializePromise(promise, resolver) {
            try {
                resolver(function resolvePromise(value) {
                    _resolve(promise, value);
                }, function rejectPromise(reason) {
                    _reject(promise, reason);
                });
            } catch (e) {
                _reject(promise, e);
            }
        }

        var id = 0;
        function nextId() {
            return id++;
        }

        function makePromise(promise) {
            promise[PROMISE_ID] = id++;
            promise._state = undefined;
            promise._result = undefined;
            promise._subscribers = [];
        }

        function Enumerator(Constructor, input) {
            this._instanceConstructor = Constructor;
            this.promise = new Constructor(noop);

            if (!this.promise[PROMISE_ID]) {
                makePromise(this.promise);
            }

            if (isArray(input)) {
                this._input = input;
                this.length = input.length;
                this._remaining = input.length;

                this._result = new Array(this.length);

                if (this.length === 0) {
                    fulfill(this.promise, this._result);
                } else {
                    this.length = this.length || 0;
                    this._enumerate();
                    if (this._remaining === 0) {
                        fulfill(this.promise, this._result);
                    }
                }
            } else {
                _reject(this.promise, validationError());
            }
        }

        function validationError() {
            return new Error('Array Methods must be provided an Array');
        };

        Enumerator.prototype._enumerate = function () {
            var length = this.length;
            var _input = this._input;

            for (var i = 0; this._state === PENDING && i < length; i++) {
                this._eachEntry(_input[i], i);
            }
        };

        Enumerator.prototype._eachEntry = function (entry, i) {
            var c = this._instanceConstructor;
            var resolve$$ = c.resolve;

            if (resolve$$ === resolve) {
                var _then = getThen(entry);

                if (_then === then && entry._state !== PENDING) {
                    this._settledAt(entry._state, i, entry._result);
                } else if (typeof _then !== 'function') {
                    this._remaining--;
                    this._result[i] = entry;
                } else if (c === Promise) {
                    var promise = new c(noop);
                    handleMaybeThenable(promise, entry, _then);
                    this._willSettleAt(promise, i);
                } else {
                    this._willSettleAt(new c(function (resolve$$) {
                        return resolve$$(entry);
                    }), i);
                }
            } else {
                this._willSettleAt(resolve$$(entry), i);
            }
        };

        Enumerator.prototype._settledAt = function (state, i, value) {
            var promise = this.promise;

            if (promise._state === PENDING) {
                this._remaining--;

                if (state === REJECTED) {
                    _reject(promise, value);
                } else {
                    this._result[i] = value;
                }
            }

            if (this._remaining === 0) {
                fulfill(promise, this._result);
            }
        };

        Enumerator.prototype._willSettleAt = function (promise, i) {
            var enumerator = this;

            subscribe(promise, undefined, function (value) {
                return enumerator._settledAt(FULFILLED, i, value);
            }, function (reason) {
                return enumerator._settledAt(REJECTED, i, reason);
            });
        };
        function all(entries) {
            return new Enumerator(this, entries).promise;
        }
        function race(entries) {
            /*jshint validthis:true */
            var Constructor = this;

            if (!isArray(entries)) {
                return new Constructor(function (_, reject) {
                    return reject(new TypeError('You must pass an array to race.'));
                });
            } else {
                return new Constructor(function (resolve, reject) {
                    var length = entries.length;
                    for (var i = 0; i < length; i++) {
                        Constructor.resolve(entries[i]).then(resolve, reject);
                    }
                });
            }
        }
        function reject(reason) {
            /*jshint validthis:true */
            var Constructor = this;
            var promise = new Constructor(noop);
            _reject(promise, reason);
            return promise;
        }

        function needsResolver() {
            throw new TypeError('You must pass a resolver function as the first argument to the promise constructor');
        }

        function needsNew() {
            throw new TypeError("Failed to construct 'Promise': Please use the 'new' operator, this object constructor cannot be called as a function.");
        }
        function Promise(resolver) {
            this[PROMISE_ID] = nextId();
            this._result = this._state = undefined;
            this._subscribers = [];

            if (noop !== resolver) {
                typeof resolver !== 'function' && needsResolver();
                this instanceof Promise ? initializePromise(this, resolver) : needsNew();
            }
        }

        Promise.all = all;
        Promise.race = race;
        Promise.resolve = resolve;
        Promise.reject = reject;
        Promise._setScheduler = setScheduler;
        Promise._setAsap = setAsap;
        Promise._asap = asap;

        Promise.prototype = {
            constructor: Promise,
            then: then,
            'catch': function _catch(onRejection) {
                return this.then(null, onRejection);
            }
        };

        function polyfill() {
            var local = undefined;

            if (typeof global !== 'undefined') {
                local = global;
            } else if (typeof self !== 'undefined') {
                local = self;
            } else {
                try {
                    local = Function('return this')();
                } catch (e) {
                    throw new Error('polyfill failed because global object is unavailable in this environment');
                }
            }

            var P = local.Promise;

            if (P) {
                var promiseToString = null;
                try {
                    promiseToString = Object.prototype.toString.call(P.resolve());
                } catch (e) {
                    // silently ignored
                }

                if (promiseToString === '[object Promise]' && !P.cast) {
                    return;
                }
            }

            local.Promise = Promise;
        }

        // Strange compat..
        Promise.polyfill = polyfill;
        Promise.Promise = Promise;

        return Promise;

    })));
    base.Promise = window.Promise;
    base.loadJSON = function (url) {
        return new base.Promise(function (resolve, reject) {
            var xhr = new XMLHttpRequest();
            if (window["XDomainRequest"] && !("withCredentials" in xhr) && /^(http(s)?:)?\/\//.test(url))
                xhr = new window["XDomainRequest"]();
            xhr.open('GET', url);
            xhr.onreadystatechange = handler;
            xhr.responseType = 'json';
            xhr.setRequestHeader('Accept', 'application/json');
            xhr.send();
            var self = this;
            function handler() {
                if (this.readyState === this.DONE) {
                    if (this.status === 200) {
                        resolve(this.response);
                    } else {
                        reject(new Error('getJSON: `' + url + '` failed with status: [' + this.status + ']'));
                    }
                }
            };
        });
    }
    function xhr(url, mimeType, response, callback) {
        this.url = "";
        this.contentType = "application/json";
        this.type = "";
        this.data = "";
        this.headers = {};
        this._mimeType = "";
        this._responseType = "";
        this.method = "get";
        emitter(this);
        this.url = url;
        var me = this;
        function xhrHasResponse(request) {
            var type = request.responseType;
            return type && type !== "text" ? request.response : request.responseText;
        }
        function respond() {
            me.promise = new base.Promise(function (resolve, reject) {
                var status = me.request.status, result;
                if (!status && xhrHasResponse(me.request) || status >= 200 && status < 300 || status === 304) {
                    try {
                        var param = /xml/.test(me._mimeType) ? me.request : /json/.test(me._mimeType) ? me.request : me.request.responseText;
                        me.emit("success", param, me.request);
                        result = param;
                        resolve(param);
                    } catch (e) {
                        me.emit("error", e);
                        reject(new Error('getJSON: `' + url + '` failed with status: [' + me.status + ']'));
                        console.error(e);
                        return;
                    }
                    me.emit("load", result);
                } else {
                    me.emit("load", me.request);
                    result = me.request;
                }
                if (callback instanceof Function) {
                    callback(result);
                }
            });
        }
        this.request = new XMLHttpRequest();
        this.request.onreadystatechange = function () {
            //me._dispatch.completed.call(me, me.request);
            //me.emit("completed", me.request);
            me.request.readyState > 3 && respond();
        };
        if (window["XDomainRequest"] && !("withCredentials" in this.request) && /^(http(s)?:)?\/\//.test(this.url))
            this.request = new window["XDomainRequest"]();
        if (mimeType) {
            this._mimeType = mimeType;
        }
        if (response instanceof Function) {
            //this._dispatch.on("success", response);
            me.emit("success", response);
        }
    }
    xhr.prototype.header = function (name, value) {
        name = (name + "").toLowerCase();
        if (arguments.length < 2)
            return this.headers[name];
        if (value == null)
            delete this.headers[name];
        else
            this.headers[name] = value + "";
        return this;
    };
    xhr.prototype.onprogress = function (event) {
        try {
            //this._dispatch.progress.call(this, this.request);
            this.emit("progress", event, this.request);
        } finally {
        }
    };
    xhr.prototype.mimeType = function (value) {
        if (!arguments.length)
            return this._mimeType;
        this._mimeType = value == null ? null : value + "";
        return this;
    };
    xhr.prototype.send = function (method, data, callback) {
        if (arguments.length === 2 && typeof data === "function")
            callback = data, data = null;
        this.method = method || this.method;
        if (typeof data === "object") {
            var d = "";
            for (var i in data) {
                d += i + "=" + encodeURIComponent(data[i]) + "&";
            }
            data = d;
        }
        this.url = this.method.toLowerCase() == "get" ? this.url.indexOf("?") > 0 ? this.url + "&" + data : this.url + "?" + data : this.url;
        this.request.open(this.method, this.url, true);
        if (this._mimeType != null && !("accept" in this.headers))
            this.headers["accept"] = this._mimeType + ",*/*";
        if (this.request.setRequestHeader)
            if (!/head|get/i.test(method)) {
                this.request.setRequestHeader("content-type", "application/x-www-form-urlencoded; charset=UTF-8");
            }
        for (var name in this.headers)
            this.request.setRequestHeader(name, this.headers[name]);
        if (this.mimeType != null && this.request.overrideMimeType)
            this.request.overrideMimeType(this._mimeType);
        if (this._responseType != null)
            this.request.responseType = this._responseType;

        //if (callback != null) xhr.on("error", callback).on("load", function (request) {
        //    callback(null, request);
        //});
        //this._dispatch.beforesend.call(this, this.request);
        this.emit("beforesend", this.request);
        this.request.send(data == null ? null : data);
        return this.promise;
    };
    xhr.prototype.abort = function () {
        this.request.abort();
        return this;
    };
    xhr.prototype.get = function (d, callback) {
        var s = "";
        if (typeof d === "object") {
            for (var i in d) {
                s += i + "=" + d[i] + "&";
            }
        } else {
            s = d;
        }
        return this.send.apply(this, ["get", s, callback]);
    };
    xhr.prototype.post = function (d, callback) {
        return this.send.apply(this, ["post", d, callback]);
    };
    base.xhr = xhr;
    base.loadJson = function (url, method, data, callback) {
        return base.ajax(url, method, data, callback, "application/json");
    };
    base.ajax = function (url, method, data, callback, mimeType) {
        if (typeof mimeType === "undefined") { mimeType = "text/html"; }
        if (typeof callback === "function") {
            var xhr = new base.xhr(url, mimeType, null);
            xhr.on("success", callback);
            if (method && "get" == method.toLowerCase()) {
                return xhr.get(data);
            } else if (method && "post" == method.toLowerCase()) {
                return xhr.post(data);
            } else {
                return xhr.send.apply(xhr, [method, data, callback]);
            }
        } else {
            var xhr = new ajax(url, mimeType);
            callback.beforesend ? xhr.on("beforesend", callback.beforesend) : null;
            callback.progress ? xhr.on("progress", callback.progress) : null;
            callback.load ? xhr.on("load", callback.load) : null;
            callback.error ? xhr.on("error", callback.error) : null;
            callback.completed ? xhr.on("completed", callback.completed) : null;
            callback.success ? xhr.on("success", callback.success) : null;
            if (method && "get" == method.toLowerCase()) {
                return xhr.get(data);
            } else if (method && "post" == method.toLowerCase()) {
                return xhr.post(data);
            } else {
                return xhr.send.apply(xhr, [method, data, callback]);
            }
        }
    };
    base.post = function (url, data, callback) {
        return base.ajax(url, "post", data, callback, "application/json");
    };
    base.loadHtml = function (url, data, callback) {
        return base.ajax(url, "get", data, callback, "text/html");
    };
    base.isURL = function (url) {
        return /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(url)
    }
    var createNode = function (config, url) {
        var node = config.xhtml ?
            document.createElementNS('http://www.w3.org/1999/xhtml', 'html:script') :
            document.createElement('script');
        node.type = config.scriptType || 'text/javascript';
        node.charset = 'utf-8';
        node.async = true;
        return node;
    };
    base.loadScript = function (url, loaded, error) {
        var node;
        if (isBrowser) {
            head = s.head = document.getElementsByTagName('head')[0];
            //If BASE tag is in play, using appendChild is a problem for IE6.
            //When that browser dies, this can be removed. Details in this jQuery bug:
            //http://dev.jquery.com/ticket/2709
            baseElement = document.getElementsByTagName('base')[0];
            if (baseElement) {
                head = s.head = baseElement.parentNode;
            }
            //In the browser so use a script tag
            node = req.createNode({}, url);


            if (node.attachEvent &&
                !(node.attachEvent.toString && node.attachEvent.toString().indexOf('[native code') < 0) &&
                !isOpera) {
                node.attachEvent('onreadystatechange', function () {
                    loaded.call(this, base.slice(arguments));
                });
            } else {
                node.addEventListener('load', function () {
                    loaded.call(this, base.slice(arguments));
                }, false);
                node.addEventListener('error', function () {
                    error.call(this, base.slice(arguments));
                }, false);
            }
            node.src = url;
            currentlyAddingScript = node;
            if (baseElement) {
                head.insertBefore(node, baseElement);
            } else {
                head.appendChild(node);
            }

            return node;
        }
        console.error("can't load the script by js dynamic");
    };
    window.zonic = function (arg1, arg2) {
        if (!arguments.length) { return this; }
        if (typeof arg1 === "string") {
            if (base.isURL(arg1)) {
                return base.ajax.apply(this, base.slice(arguments));
            } else if (/^script:/i.test(arg1)) {
                return new base.Promise(function (resolve, reject) {
                    base.loadScript(arg1.slice(7), resolve, reject);
                });
            } else {
                return new base.element(arg1, arg2);
            }
        } else if (arg1 instanceof Element) {
            return new base.element(arg1, arg2);
        } else if (arg1 instanceof ui) {
            return ui;
        } 
        else if (base.isFunction(arg1)) {
            return arg1.apply(this, base.slice(arguments));
        }
    }
    zonic.parseHtml = element.parseHtml;
    zonic.require = function (url, callback) {
        var script = document.createElement("script");
        script.type = "text/javascript";
        if (script.readyState) { // IE
            script.onreadystatechange = function () {
                if (script.readyState == "loaded" || script.readyState == "complete") {
                    script.onreadystatechange = null;
                    callback();
                }
            };
        } else { // FF, Chrome, Opera, ...
            script.onload = function () {
                callback();
            };
        }
        script.src = url;
        document.getElementsByTagName("head")[0].appendChild(script);
    }
    zonic.evalScript = function (scriptText) {
        var script = document.createElement("script");
        script.type = "text/javascript";
        script.text = scriptText;
        document.body.appendChild(script);
    }
    base.clone.call(zonic, base);
    function ui(exp, context) {
        base.mixin(this, element.prototype);
        this.selected = [];
        if (/^create:/i.test(exp)) {
            this.select(element.parseHtml(exp.substr(7)));
        } else {
            this.select(exp, context);
        }
        this.render = function () { };
    }
    ui.landingline = function (config) {
        if (typeof config === "function") { config = { callback: config }; }
        if (!config) { config = {}; }
        if (config instanceof String) { config = { parent: config }; }
        config.time = config.time || 3;
        config.width = config.width || 5;
        config.background = config.backround || "#886ed7";
        config.height = config.height || 1;
        config.delay = config.delay || 0;
        var thtmlStyle = " <style>@-webkit-keyframes landingline_animate { 0% { margin-left: 0;} 5% { margin-left: 4%;} 50% { margin-left:" + (50 - config.width) + "%;}100% { margin-left: " + (100 - config.width) + "%;}} @keyframes landingline_animate { 0% { margin-left: 0;}5% { margin-left: 4%;} 50% { margin-left:" + (50 - config.width) + "%;}100% { margin-left: " + (100 - config.width) + "%;}}</style>";
        var thtml = "<div style='height: " + config.height + "px; width: " + config.width + "%; margin: 0px;, float: left; background-color: " + config.background + "; animation: landingline_animate " + config.time + "s infinite; -webkit-animation: landingline_animate " + config.time + "s infinite animation-delay:" + config.delay + "s; -webkit-animation-delay: " + config.delay + "s: " + (config.csstext || ";") + "\'></div>"
        var t = element.select(element.parseHtml("<div style='height:1px;'></div>"));
        t.append(element.parseHtml(thtmlStyle)); t.append(element.parseHtml(thtml));
        t.isappended = false;
        config.callback ? config.callback.call(t, config.parent) : null;
        if (config.parent && !t.isappended) {
            t.appendTo(config.parent);
        }
        return t;
    }
    ui.toCssString = function (name, so) {
        var s = name + "{";
        for (var i in so) {
            if (typeof so[i] === "string") {
                s += i + ":" + so[i] + ";";
            }
        }
        return s + "}";
    }

    ui.appendCss = function (name, style) {
        var appendcss = "";
        if (arguments.length == 2 && base.isObject(style)) {
            appendcss += ui.toCssString(name, style);
        } else if (arguments.length == 1 && typeof name === "string") {
            appendcss += name;
        } else if (arguments.length == 1 && base.isObject(name)) {
            for (var i in name) {
                appendcss += ui.toCssString(i, name[i]);
            }
        }
        var style = document.createElement("style");
        element.select(style).text(appendcss).attr({ "zonic-source": "auto", "zonic-append": "true" }).appendTo(element.select("head"));
    }
    ui.section = function (exp) {
        var u = new ui(exp || element.parseHtml("section"));

        u.addClass("zonic-section-box");
        return u;
    }
    ui.button = function (exp) {
        var u = new ui(exp || element.parseHtml("button"));

        return u;
    }
    ui.layer = function (conf) {
        conf = conf || {};
        var u = new ui(element.parseHtml("div"));

        u.layout = function (css) {
            return u.css(css);
        }
        u.css({ top: "0px", left: "0px", opacity: "0", "background-color": conf.background || "#fff;", position: conf.position || "absolute;", "z-index": 10, width: conf.width || "40%", height: conf.height || "300px" })
            .addClass(conf.mode == "hover" ? "zonic-section-layer-hover" : (conf.mode == "mask" ? "zonic-section-mask" : "zonic-section-layer")).appendTo(element.select("body"));
        
        if (conf.center) {
            window.setTimeout(function () {
                var r = u.getBoundingRect(), width = r.width, height = r.height;
                u.css({ opacity: 1, top: ((conf.top || window.screen.availHeight / 2) - height/2) + "px", left: ((conf.left || window.screen.availWidth / 2) - width / 2) + "px" });
            }, 0);
        }
        else { window.setTimeout(function () { u.css({ opacity: 1 }); }, 0); }
        if (!base.hasProp(conf, "dragable") || conf.dragable) element.drag(u, u, 0.3);
        //emitter(u);
        return u;
    }
    ui.switch = function () {
        var u = new ui(element.parseHtml("span"));
        //<span class="ant-switch ant-switch-checked" tabindex="0"><span class="ant-switch-inner">\u5F00</span></span>

        u.addClass("zonic-switch");
        u.bind("click", function (e) {
            if (u.hasClass("zonic-disabled")) { return true; }
            else if (u.hasClass("zonic-switch-checked")) { u.removeClass("zonic-switch-checked"); }
            else { u.addClass("zonic-switch-checked"); }
            u.emit("checkedchanged", e, u.checked);
        });
        u.checked = function () {
            return u.hasClass("zonic-switch-checked");
        }
        emitter(u);

        //emitter(u);
        return u;
    }
    ui.list = function (conf) {
        conf = conf || {};
        var u = new ui(element.parseHtml("ul"));

        u.addClass("zonic-list");
        emitter(u);
        u.multi = false;
        u.additem = function (data) {
            if (base.isArray(data)) { data.forEach(function (i) { u.additem(i); }); return this; }
            return (function () {
                var d = data;
                var item = new ui(element.parseHtml("li"));
                item.uiParent = u;
                item.bind("click", function (e) {
                    if (item.attr("disabled")) { return; }
                    if (!u.multi) {
                        u.find(".zonic-list-item[selected='true']").attr({ "selected": "false" });
                    }
                    if (item.attr("selected") == "true") {
                        item.attr({ "selected": "false" });
                    } else {
                        item.attr({ "selected": "true" });
                    }
                    u.emit("selectchanged", e, data, item);
                }).bind("mouseenter", function (e) {
                    u.emit("hoveritem", e, data, item);
                }).addClass("zonic-list-item").attr({ "selected": "false", }).appendTo(u).if(typeof data !== "object", function () {
                    return item.html(data);
                }).if(data.text || data.html, function () {
                    return item.html(data.text || data.html);
                }).css(u.itemCss || {});
                return item;
            })();
        }
        u.binddata = function (conf, data) {
            if (arguments.length = 1) {
                data = conf.data;
                conf = conf.conf;
            }
            return this;
        }
        u.itemCss = null;
        u.dragable = base.hasProp(conf, "dragable") ? conf.dragable : false;
        return u;
    }
    ui.text = function (conf) {
        conf = conf || {};
        conf.labelColor = conf.labelColor || "rgba(0, 0, 0, 0.298039)";
        var height = conf.height || 72;
        var width = conf.width || 256;
        var u = new ui(element.parseHtml("div"));
        u.style("font-size: 16px; line-height: " + (height * 0.39) + "px; width: " + width + "px; height: " + height + "px; display: inline-block; position: relative; background-color: transparent; font-family: Roboto, sans-serif; transition: height 200ms cubic-bezier(0.23, 1, 0.32, 1) 0ms; cursor: auto;")
            .append(new element(element.parseHtml("label")).style("position: absolute; line-height: " + (height * 0.45) + "px; top: " + (height * 0.45) + "px; transition: all 450ms cubic-bezier(0.23, 1, 0.32, 1) 0ms; z-index: 1; transform: scale(1) translate(0px, 0px); transform-origin: left top 0px; pointer-events: none; user-select: none; color: rgba(0, 0, 0, 0.298039);").addClass("label").html(conf.label || ""))
            .append(new element(element.parseHtml("div")).style("position: absolute; opacity: 0; color: rgba(0, 0, 0, 0.298039); transition: all 450ms cubic-bezier(0.23, 1, 0.32, 1) 0ms; bottom: " + (height * 0.17) + "px;").addClass("placeholder").html(conf.placeholder || ""))
            .append(new element(element.parseHtml("input")).attr({ type: conf.type || "text" }).style("padding: 0px; position: relative; width: 100%; border: none; outline: none; background-color: rgba(0, 0, 0, 0); color: rgba(0, 0, 0, 0.870588); cursor: inherit; font-style: inherit; font-variant: inherit; font-weight: inherit; font-stretch: inherit; font-size: inherit; line-height: inherit; font-family: inherit; -webkit-tap-highlight-color: rgba(0, 0, 0, 0); height: 100%; box-sizing: border-box; margin-top: " + (height * 0.19) + "px;").bind("focus", function () {
                u.find("hr.hover-for-text").css({ transform: "scaleX(1)" });
                if (!u.getValue() && conf.hint) {
                    u.find("div.placeholder").css({ opacity: 1 });
                    u.find("label.label").css({ transform: "scale(0.75) translate(0px, -" + (conf.height * 0.56) + "px)", color: conf.hintColor || conf.labelColor });
                }
            }).bind("blur", function () {
                u.find("hr.hover-for-text").css({ transform: "scaleX(0)" });
                if (!u.getValue() && conf.hint) {
                    u.find("div.placeholder").css({ opacity: 0 });
                    u.find("label.label").css({ transform: "scale(1) translate(0px, 0px)", color: conf.labelColor });
                }
            }).bind("keydown", function () {
                if (u.getValue()) {
                    u.find("div.placeholder").css({ opacity: 0 });
                }
            }).bind("keyup", function () {
                if (u.getValue()) {
                    u.find("div.placeholder").css({ opacity: 0 });
                }
            }).value(conf.value || ""))
            .append((function () {
                return new element(element.parseHtml("div"))
                    .append(new element(element.parseHtml("hr")).style("border-top: none rgb(224, 224, 224); border-left: none rgb(224, 224, 224); border-right: none rgb(224, 224, 224); border-bottom: 1px solid rgb(224, 224, 224); bottom: " + (conf.height * 0.01) + "px; box-sizing: content-box; margin: 0px; position: absolute; width: 100%;"))
                    .append(new element(element.parseHtml("hr")).style("border-top: none rgb(0, 188, 212); border-left: none rgb(0, 188, 212); border-right: none rgb(0, 188, 212); border-bottom: 2px solid rgb(0, 188, 212); bottom: " + (conf.height * 0.01) + "px; box-sizing: content-box; margin: 0px; position: absolute; width: 100%; transform: scaleX(0); transition: all 450ms cubic-bezier(0.23, 1, 0.32, 1) 0ms;").addClass("hover-for-text"))
                    ;
            })())
            ;
        u.getValue = function () {
            return u.find("input").value();
        }
        if (u.getValue() || !conf.hint) {
            u.find("div.placeholder").css({ opacity: 0 });
            u.find("label.label").css({ transform: "scale(0.75) translate(0px, -" + (conf.height * 0.56) + "px)", color: conf.hintColor || conf.labelColor });
        }
        return u;
    }
    base.whichTransitionEvent =
        (function () {
            var t;
            var el = document.createElement('fakeelement');
            var transitions = {
                'transition': 'transitionend',
                'OTransition': 'oTransitionEnd',
                'MozTransition': 'transitionend',
                'WebkitTransition': 'webkitTransitionEnd',
                'MsTransition': 'msTransitionEnd'
            }
            for (t in transitions) {
                if (el.style[t] !== undefined) {
                    return transitions[t];
                }
            }
        })();
    base.whichAdnimationEvent =
        (function () {
            var t;
            var el = document.createElement('fakeelement');
            var transitions = {
                'animation': 'animationend',
                'OAnimation': 'oAnimationEnd',
                'MozAnimation': 'animationend',
                'WebkitAnimation': 'webkitAnimationEnd',
                'MsAnimation': 'msAnimationEnd'
            }
            for (t in transitions) {
                if (el.style[t] !== undefined) {
                    return transitions[t];
                }
            }
        })();
    ui.effacee = function (conf) {
        if (!conf) { conf = {}; }
        var u = new ui("create:span")
        u.css({ "height": "100%", "width": "100%", "z-index": "1", "position": "absolute", "top": "0px", "left": "0px", "overflow": "hidden", "pointer-events": "none" })
            .addClass("zonic-effacee");
        u.cascade = function (ob) {
            ob = ob instanceof ui || ob instanceof element ? ob : new element(ob);
            u.find("div").remove();
            u.appendTo(ob);
            ob.bind("mousedown", function () {
                var el = element.select(this),
                    height = el.getBoundingHeight();
                w = el.getBoundingWidth();
                el.append(new element(element.parseHtml("div")).css({ "position": "absolute", "top": -w + "px", "left": "-2.26525px", "height": (w * 3) + "px", "width": (w * 3) + "px" || "100%", "border-radius": "100%", "background-color": conf.background || "rgba(0, 0, 0, 0.870588)", "opacity": "1", "transform": "scale(0)", "transition": "all 1s cubic-bezier(0.23, 1, 0.32, 1) 0ms" })
                    .bind(base.whichTransitionEvent, function () {
                        element.select(this).remove();
                    }));
                setTimeout(function () {
                    el.find("div").css({ opacity: 0, "transform": "scale(1)" });
                }, 0);

            });
        }
        if (conf.cascade) {
            u.cascade(conf.cascade);
        }
        return u;
    }
    ui.select = function (conf) {
        conf = conf || {};
        var height = conf.height || 72;
        var u = new ui(element.parseHtml("div"));
        //u.listLayer = ui.layer();
        //u.list = ui.list();
        u.style("transition:height 200ms cubic-bezier(0.23, 1, 0.32, 1); width: " + (conf.width || 256) + "px; height: " + (conf.height || 256) + "px; line-height: " + (conf.height * 0.33) + "px; font-family: Roboto,sans-serif; font-size: 16px; display: inline-block; position: relative; cursor: auto; background-color: transparent;")
            .append(new element(element.parseHtml("label")).style("transition:450ms cubic-bezier(0.23, 1, 0.32, 1); top: " + (height * 0.55) + "px; color: rgba(0, 0, 0, 0.3); line-height: " + (height * 0.35) + "px; position: absolute; z-index: 1; pointer-events: none; transform: scale(1) translate(0px, 0px); transform-origin: left top 0px;").addClass("label").html(conf.label || "&nbsp;"))
            //.append(new element(element.parseHtml("label")).css({ "margin": (height * 0.1) + "px 0px 0px 1px","position": "absolute", "line-height": (height * 0.45) + "px", "top": (height * 0.385) + "px","padding":"0px 0px 0px 2px", "transition": "all 450ms cubic-bezier(0.23, 1, 0.32, 1) 0ms", "z-index": "1", "transform": "scale(1) translate(0px, 0px)", "transform-origin": "left top 0px", "pointer-events": "none", "user-select": " none", "color": "rgba(0, 0, 0, 1)" }).addClass("zonic-select-label").html(conf.text || ""))
            .append(new element(element.parseHtml("div")).style('font:inherit; padding: 0px; outline: invert; border: medium; transition:450ms cubic-bezier(0.23, 1, 0.32, 1); border-image: none; width: 100%; height: 100%; color: rgba(0, 0, 0, 0.87); margin-top: ' + (height * 0.2) + 'px; display: block; position: relative; cursor: inherit; box-sizing: border-box; font-size-adjust: none; font-stretch: inherit; -webkit-tap-highlight-color: rgba(0, 0, 0, 0); background-color: rgba(0, 0, 0, 0)')
                .call(function () {
                    new element(element.parseHtml("div")).css({ "padding-left": "5px", cursor: "pointer", height: "100%", position: "relative", width: "100%" })
                        .append(new element(element.parseHtml("div")).style("box-sizing:border-box;content: \" \";display:table;"))
                        .append(new element(element.parseHtml("div")).style("top: " + (height * 0.21) + "px; color: rgba(0, 0, 0, 0.87); line-height: " + (height * 0.8) + "px; overflow: hidden; padding-right: 56px; padding-left: 0px; white-space: nowrap; position: relative;text-overflow: ellipsis; opacity: 1;").addClass("zonic-select-area").html(conf.text || "&nbsp;"))
                        .append(new element(element.parseHtml("button")).attr({ type: "button" }).css({ "border": (height * 0.137) + "px", " box-sizing": "border-box", "display": "inline-block", "font-family": " Roboto, sans-serif", " -webkit-tap-highlight-color": " rgba(0, 0, 0, 0)", " cursor": " pointer", " text-decoration": " none", " margin": " 0px", "padding": (height * 0.167) + "px", "outline": " none", "font-size": " 0px", "font-weight": " inherit", "position": "absolute", "z-index": " 1", "overflow": " visible", "transition": " all 450ms cubic-bezier(0.23, 1, 0.32, 1) 0ms", " width": (height * 0.667) + "px", " height": (height * 0.667) + "px", " fill": " rgb(224, 224, 224)", " right": " 0px", " top": (height * 0.20) + "px", " background": " none" }).html('<div><svg viewBox="0 0 ' + (height * 0.33) + ' ' + (height * 0.33) + '" style="display: inline-block; color: rgba(0, 0, 0, 0.870588); fill: inherit; height: ' + (height * 0.33) + 'px; width: ' + (height * 0.33) + 'px; user-select: none; transition: all 450ms cubic-bezier(0.23, 1, 0.32, 1) 0ms;"><path d="M7 10l5 5 5-5z"></path></svg></div>'))
                        .append(new element(element.parseHtml("div")).css({ "border-top": "none", "bottom": "1px", left: "0px", margin: "-1px 24px", right: "0px", position: "absolute" }))
                        .append(new element(element.parseHtml("div")).style("box-sizing:border-box;content: \" \";display:table;clear: both;"))
                        .appendTo(this);
                }).bind("click", function () {
                    if (u.layer) { return; }
                    var rect = u.getBoundingRect();
                    var top = u.selected[0].offsetTop;
                    u.layer = ui.layer({ position: "fixed", dragable: false, width: "0px", height: "0px" })
                        .style("transition:height 1s cubic-bezier(0.23, 1, 0.32, 1) 0ms;").style("width: " + (conf.width || rect.width) + "px;height:" + (conf.layerHeight || 160) + "px;border-radius:2px; left: " + (rect.left + (conf.horizontal == "right" ? rect.width : (conf.horizontal == "middle" ? rect.width / 2 : 0))) + "px; top: " + (rect.top + height * 0.2 + (conf.vertical == "top" ? 0 : (conf.vertical == "center" ? rect.height / 2 : rect.height))) + "px;");

                    u.find("label.label").css({ transform: "scale(0.75) translate(0px, " + (-height * 0.40) + "px)", color: conf.labelColor || "rgba(0, 0, 0, 0.298039)" });
                    //u.find("label.")
                    u.list = ui.list().on("selectchanged", function (e, data, item) {
                        if (!base.isObject(data)) {
                            u.selectedValue = u.selectedText = data; u.find(".zonic-select-area").html(data).attr({ "zonic-selected-value": data });
                        } else {
                            u.selectedValue = data.value; u.selectedText = data.text; u.find(".zonic-select-area").html(data.text).attr({ "zonic-selected-value": data.value });
                        }
                        u.emit("selectchanged", base.slice(arguments));
                    });
                    u.list.additem([1, 2, 3]);
                    u.list.appendTo(u.layer);

                })
                .append((function () {
                    return new element(element.parseHtml("div")).html('<hr style="border-width: medium medium 1px; border-style: none none solid; border-color: rgb(224, 224, 224); margin: 0px; width: 100%; bottom:' + (height * 0.06) + 'px; position: absolute; box-sizing: content-box;">')
                        //.append(new element(element.parseHtml("hr")).css({ "border-width":"medium medium 1px","border-style":"none none solid","border-color":"rgb(224, 224, 224)","margin":"0px","width":"100%","bottom":(height*0.06)+"px","position":"absolute","box-sizing":"content-box" }))
                        ;
                })())
            );
        u.selectedValue = "";
        u.selectedText = "";
        u.additem = function () {
            u.list.additem.apply(u.list, base.slice(arguments))
            return u;
        }
        element.select("body").bind("click", function (e) {
            var e = e || window.event;
            if (u.selected[0].isEqualNode(e.target) ||
                u.find("*").has(e.target)) {
                return;
            }
            u.layer.remove();
            u.layer = null;
        });
        //if (u.getValue()) {
        //    u.find("div.placeholder").css({ opacity: 0 });
        //
        //}
        return u;
    }
    ui.initCss = function () {
        ui.appendCss({
            ".zonic-section-box:hover": {
                "box-shadow": " 0 0 6px rgba(0,0,0,.15);",
                "position": "relative;",
                "z-index": "10;",
                "background": "#fff;"
            }, ".zonic-section-box": {
                "border": " 1px solid #e9e9e9;",
                "border-radius": "6px;",
                "display": "inline-block;",
                " width": "100%",
                "position": "relative",
                "margin": "0 0 16px",
                "-webkit-transition": "all .2s ease",
                "transition": "all .2s ease"
            }
        });
        ui.appendCss({
            ".zonic-button": {
                "display": "inline-block;",
                "margin-bottom": "0"
                , "font-weight": "500;",
                "text-align": " center;",
                "vertical-align": " middle;",
                "-ms-touch-action": " manipulation;",
                "touch-action": " manipulation;",
                "cursor": " pointer;",
                "background-image": " none;",
                "border": " 1px solid transparent;",
                "white-space": " nowrap;",
                "line-height": " 1.5;",
                "padding": " 4px 15px;",
                "font-size": " 12px;",
                "border-radius": " 6px;",
                "-webkit-user-select": " none;",
                "-moz-user-select": " none;",
                "-ms-user-select": " none;",
                "user-select": " none;",
                "-webkit-transition": " all .3s cubic- bezier(.645, .045, .355, 1);",
                "transition": " all .3s cubic- bezier(.645, .045, .355, 1);",
                "position": " relative;",
                "color": " #666;",
                "background-color": " #f7f7f7;",
                "border-color": " #d9d9d9;"
            }
            , ".zonic-button,.zonic-button:active,.zonic-button:focus ": { outline: "1" }//"{outline: 0;}"
            , ".zonic-list": {
                "padding": "0px;",
                "margin": "0px",
                "margin-bottom": " 0",
                "padding-left": " 0",
                "list-style": " none",
                "max-height": " 250px",
                "overflow": " auto"
            },
            ".zonic-list:after": {
                "outline": " none",
                "margin-bottom": " 0",
                "padding-left": " 0",
                "list-style": " none",
                "max-height": " 250px",
                "overflow": " auto"
            },
            ".zonic-list-item,.zonic-ist li": {
                "position": " relative",
                "display": " block",
                "padding": "0px",
                "font-weight": " 400",
                "color": " #666",
                "cursor": " pointer",
                "white-space": " nowrap",
                "overflow": " hidden",
                "-webkit-transition": " background .3s ease",
                "transition": " background .3s ease",
            },
            ".zonic-switch:after": {
                "position": " absolute",
                "width": " 18px",
                "height": " 18px",
                "left": " 2px",
                "top": " 1px",
                "border-radius": " 18px",
                "background-color": " #fff",
                "content": " \" \"",
                "cursor": " pointer",
                "-webkit-transition": " left .3s cubic- bezier(.78, .14, .15, .86), width .3s cubic- bezier(.78, .14, .15, .86)",
                "transition": " left .3s cubic- bezier(.78, .14, .15, .86), width .3s cubic- bezier(.78, .14, .15, .86)",

            }
            , ".zonic-switch": {
                "position": "relative",
                "display": " inline - block",
                "box-sizing": " border-box",
                "width": " 44px",
                "height": " 22px",
                "line-height": " 20px",
                "vertical-align": " middle",
                "border-radius": " 20px",
                "border": " 1px solid #ccc",
                "background-color": " #ccc",
                "cursor": " pointer",
                "-webkit-transition": " all .3s cubic- bezier(.78, .14, .15, .86)",
                "transition": " all .3s cubic-bezier(.78, .14, .15, .86)",
                "-webkit-user-select": " none",
                "-moz-user-select": " none",
                "-ms-user- select": " none",
                "user-select": " none",
            },
            ".zonic-switch-checked": {
                "border-color": "#2db7f5;",
                "background-color": "#2db7f5;"
            },
            ".zonic-disabled": {
                "cursor": "not-allowed;",
                "background": "#f4f4f4;",
                "border-color": "#f4f4f4;",
            },
            ".zonic-section-layer-hover:hover": {
                "box-shadow": " 0 0 6px rgba(0,0,0,.15);",
                "z-index": "10;",
                "background": "#fff;"
            },
            ".zonic-section-layer-hover": {
                "border": " 1px solid #e9e9e9;",
                "border-radius": "6px;",
                "display": "inline-block;",
                "position": "absolute;",
                "margin": "0 0 16px;",
                "-webkit-transition": "all 0.52s cubic-bezier(0, 0.55, 0.37, 1.06) 0s;",
                "transition": "all 0.52s cubic-bezier(0, 0.55, 0.37, 1.06) 0s;"//transition:left cubic-bezier(0.23, 1, 0.32, 1);
            },
            ".zonic-section-layer": {
                "border": " 1px solid #e9e9e9;",
                "border-radius": "6px;",
                "display": "inline-block;",
                "position": "absolute;",
                "margin": "0 0 16px;",
                "-webkit-transition": "all .2s cubic-bezier(0.23, 1, 0.32, 1) 100ms;",
                "transition": "all .2s cubic-bezier(0.23, 1, 0.32, 1) 100ms;",
                "z-index": "10;",
                "background": "#fff;"
            },
            ".zonic-section-mask": {
                "border": " 0px solid #e9e9e9;",
                "border-radius": "0px;",
                "display": "inline-block;",
                "position": "absolute;",
                "margin": "0px;",
                "-webkit-transition": "all .2s ease;",
                "transition": "all .2s ease;",
                "z-index": "10;",
                "background": "#fff;"
            },
            ".zonic-section-layer:hover": {
                "box-shadow": " 0 0 6px rgba(0,0,0,.15);",
                "background": "#fff;"
            }
        });
    }
    ui.initCss();
    ui.dialog = function (conf, okfn, cancelfn) {
        if (typeof conf === "string") {
            conf = { text: conf };
        }
        var u = new ui("create:div");
        if (conf.mask) {
            u.mask = ui.layer({ mode: "mask" }).appendTo(u).style("background:rgba(146, 146, 146, 0.42);z-index:" + ((conf.zindex || 10) + 1) + ";left:0px;height:100%;width:100%;top:0px;");
        }
        u.layer = ui.layer({ mode: "hover", center: true }).appendTo(u).style("background:#fff;z-index:" + ((conf.zindex || 10) + 1) + ";overflow:hidden;");
        if (conf.body) {
            u.layer.append(typeof conf.body === "string" ? conf.body : (base.isFunction(conf.body) ? conf.body.call(u, conf) : ""));
        }
        u.listen = function () {
            u.find("[ns-command]").bind("click", function (e) {
                var el = element.select(this), cmd = el.attr("ns-command");
                u.emit(cmd, e, this);
            });
        }
        u.listen();
        u._remove = u.remove;
        u.layer.bind(base.whichTransitionEvent, function (e) {
            setTimeout(function () {
                if (parseFloat(u.layer.css("opacity")) == 0) {
                    u._remove();
                }
            });
        });
        u.remove = function () {
            u.layer.css({ left: "0px;", top: "0px", opacity: "0" });
            u.mask.css({ opacity: "0" });
        }
        return u.appendTo("body");
    }
    ui.confirm = function (conf, okfn, cancelfn) {
        if (typeof conf === "string") {
            conf = { text: conf };
        }
        if (base.isFunction(okfn)) { conf.ok = okfn; }
        if (base.isFunction(cancelfn)) { conf.cancel = cancelfn; }
        conf.mask = true;
        var u = ui.dialog(conf);
        u.layer.css({ height: "180px" })
        u.layer.append(new element(element.parseHtml('<h3 style="margin: 0px; padding: 24px 24px 20px; color: rgba(0, 0, 0, 0.87); line-height: 32px; font-size: 22px; font-weight: 400; border-bottom-color: currentColor; border-bottom-width: medium; border-bottom-style: none;">' + (conf.title || "Confirm Dialog") + '</h3>')))
            .append(new element(element.parseHtml('<div style="padding: 0px 24px 24px; color: rgba(0, 0, 0, 0.6); font-size: 16px; overflow-y: hidden; max-height: 123px; box-sizing: border-box;">' + (conf.text || "") + '</div>')))
            .append(new element(element.parseHtml('<div style="padding: 8px; width: 100%; text-align: right; margin-top: 0px; border-top-color: currentColor; border-top-width: medium; border-top-style: none; box-sizing: border-box; -webkit-tap-highlight-color: rgba(0, 0, 0, 0);"><button tabindex="0" ns-command="cancel" style="margin: 0px; padding: 0px; outline: invert; border-radius: 2px; border: 10px; transition:450ms cubic-bezier(0.23, 1, 0.32, 1); border-image: none; height: 36px; text-align: center; color: rgb(0, 188, 212); line-height: 36px; overflow: hidden; font-family: Roboto,sans-serif; font-size: inherit; font-weight: inherit; text-decoration: none; display: inline-block; position: relative; z-index: 1; cursor: pointer; min-width: 88px; box-sizing: border-box; -webkit-tap-highlight-color: rgba(0, 0, 0, 0); background-color: rgba(0, 0, 0, 0);" type="button"><div><span style="text-transform: uppercase; letter-spacing: 0px; padding-right: 16px; padding-left: 16px; font-size: 14px; font-weight: 500; vertical-align: middle; position: relative;">' + (conf.cancelText || "CANCEL") + '</span></div></button><button tabindex="0" ns-command="ok" style="margin: 0px; padding: 0px; outline: invert; border-radius: 2px; border: 10px; transition:450ms cubic-bezier(0.23, 1, 0.32, 1); border-image: none; height: 36px; text-align: center; color: rgb(0, 188, 212); line-height: 36px; overflow: hidden; font-family: Roboto,sans-serif; font-size: inherit; font-weight: inherit; text-decoration: none; display: inline-block; position: relative; z-index: 1; cursor: pointer; min-width: 88px; box-sizing: border-box; -webkit-tap-highlight-color: rgba(0, 0, 0, 0); background-color: rgba(0, 0, 0, 0);" type="button"><div><span style="text-transform: uppercase; letter-spacing: 0px; padding-right: 16px; padding-left: 16px; font-size: 14px; font-weight: 500; vertical-align: middle; position: relative;">' + (conf.okText || "OK") + '</span></div></button></div>')))
            ;
        ui.effacee({ cascade: u.find("[ns-command] div") });
        u.on("ok", function () {
            if (base.isFunction(conf.ok)) { conf.ok.apply(u, base.slice(arguments)); }
            u.remove();
        }).on("cancel", function () {
            if (base.isFunction(conf.cancel)) { conf.cancel.apply(u, base.slice(arguments)); }
            u.remove();
        });
        u.listen();
        return u;
    }
    ui.alert = function (conf, okfn) {
        if (typeof conf === "string") {
            conf = { text: conf };
        }
        if (base.isFunction(okfn)) { conf.ok = okfn; }
        conf.mask = true;
        var u = ui.dialog(conf);
        u.layer.css({ height: "180px" })
        u.layer.append(new element(element.parseHtml('<h3 style="margin: 0px; padding: 24px 24px 20px; color: rgba(0, 0, 0, 0.87); line-height: 32px; font-size: 22px; font-weight: 400; border-bottom-color: currentColor; border-bottom-width: medium; border-bottom-style: none;">' + (conf.title || "Alert Dialog") + '</h3>')))
            .append(new element(element.parseHtml('<div style="padding: 0px 24px 24px; color: rgba(0, 0, 0, 0.6); font-size: 16px; overflow-y: hidden; max-height: 123px; box-sizing: border-box;">' + (conf.text || "") + '</div>')))
            .append(new element(element.parseHtml('<div style="padding: 8px; width: 100%; text-align: right; margin-top: 0px; border-top-color: currentColor; border-top-width: medium; border-top-style: none; box-sizing: border-box; -webkit-tap-highlight-color: rgba(0, 0, 0, 0);"><button tabindex="0" ns-command="ok" style="margin: 0px; padding: 0px; outline: invert; border-radius: 2px; border: 10px; transition:450ms cubic-bezier(0.23, 1, 0.32, 1); border-image: none; height: 36px; text-align: center; color: rgb(0, 188, 212); line-height: 36px; overflow: hidden; font-family: Roboto,sans-serif; font-size: inherit; font-weight: inherit; text-decoration: none; display: inline-block; position: relative; z-index: 1; cursor: pointer; min-width: 88px; box-sizing: border-box; -webkit-tap-highlight-color: rgba(0, 0, 0, 0); background-color: rgba(0, 0, 0, 0);" type="button"><div><span style="text-transform: uppercase; letter-spacing: 0px; padding-right: 16px; padding-left: 16px; font-size: 14px; font-weight: 500; vertical-align: middle; position: relative;">' + (conf.okText || "OK") + '</span></div></button></div>')))
            ;
        ui.effacee({ cascade: u.find("[ns-command] div") });
        u.on("ok", function () {
            if (base.isFunction(conf.ok)) { conf.ok.apply(u, base.slice(arguments)); }
            u.remove();

        });
        u.listen();
        return u;
    }
    ui.liner = function (conf) {
        conf = conf || {};
        var u = new ui("create:div")
            .style('left:' + (conf.left || "0px") + ';width:' + (conf.width || "60px") + ' ;display: block;background-color: ' + (conf.background || "rgb(255, 64, 129)") + ';height: ' + (conf.height || "2px") + ';margin-top: -' + (conf.height || "2px") + 'px;position: relative;transition: left 1s cubic-bezier(0.23, 1, 0.32, 1) 0ms;');
        u.left = function (w) {
            return u.style("width:" + w);
        }
        return u;
    }
    ui.tab = function (conf) {
        conf = conf || {};
        var u = new ui("create:div");
        emitter(u);
        u.append(new element("create:div").style('box-sizing: border-box; content: " "; display: table;'))
            .append(
            new element(element.parseHtml("div"))
                .append(new element("create:div").style("width: 100%; background-color:" + (conf.headBackground || "rgb(0, 188, 212)") + "; white-space: nowrap; display: flex;height:" + (conf.headHeight || "30px") + ";line-height:" + (conf.headHeight || "30px") + ";").addClass("zonic-tab-head-container"))
                .append(new element("create:div").style("width: 100%; ").call(function () { u.liner = ui.liner({ width: "0px", background: conf.linerBackground || "" }); u.liner.appendTo(this); return this; }))
                .append(new element("create:div").style("width: 100%; ").addClass("zonic-tab-item-container"))
            ).append(new element("create:div").style('box-sizing: border-box; content: " "; display: table;'))
            ;
        u._itemwidth = 100;
        u.itemwidth = conf.itemwidth || "100px";
        if (/px$/i.test(u.itemwidth)) {
            u._itemwidth = parseFloat(u.itemwidth.substr(0, u.itemwidth.length - 2)) || 100;
        }
        u.additem = function (data) {
            if (base.isArray(data)) { data.forEach(function (it) { u.additem(it); }); return this; }
            return (function () {
                var d = {};
                var children = u.find(".zonic-tab-head");
                if (u.itemwidth === "percent" || u.itemwidth === "%") {
                    u._itemwidth = 100 / (children.selected.length + 1);
                    children.style("width:" + (u._itemwidth * (children.selected.length + 1)) + "%;");
                }
                var item = new element("create:button").attr({ "tabindex": 0, "type": "button" }).style("width:" + (u.itemwidth === "percent" || u.itemwidth === "%" ? u._itemwidth * (children.selected.length + 1) + "%" : (u._itemwidth + "px")) + ";height:100%;border: 10px; box-sizing: border-box; display: inline-block; font-family: Roboto, sans-serif; -webki-tap-highlight-color: rgba(0, 0, 0, 0); cursor: pointer; text-decoration: none; margin: 0px; padding: 0px; outline: none; font-size: 14px; font-weight: 500; position: relative; z-index: 1; color: rgb(255, 255, 255); text-transform: uppercase; background: none; ").html('<div><div style="display: flex; flex-direction: column; align-items: center; justify-content: center; height: 48px; "></div></div>')
                var itembody = new element("create:div").addClass("zonic-tab-item").style("width: 100%; position: relative; text-align: initial;opacity:0;min-height:" + (conf.minHeight || "120px") + ";").call(function () {
                    if (base.isFunction(data.body)) { data.body.call(this, u, data); } else if (typeof data.body === "string") { element.select(this).html(data.body); } else if (data.body) { element.select(this).append(data.body); } return this;
                }).appendTo(u.find(".zonic-tab-item-container"));
                d.uiParent = u;
                d.head = item;
                d.item = itembody;
                u.liner.style("width:" + (u.itemwidth === "percent" || u.itemwidth === "%" ? (u._itemwidth) + "%" : (u._itemwidth + "px")));
                item.bind("click", function (e) {
                    if (item.attr("disabled")) { return; }
                    u.find(".zonic-tab-head[selected='true']").attr({ "selected": "false" });
                    item.attr({ "selected": "true" });
                    u.find(".zonic-tab-item").hide();
                    itembody.show();
                    u.emit("tabchanged", e, data, d, item);
                }).bind("mouseenter", function (e) {
                    var index = u.find(".zonic-tab-head").selected.indexOf(this);
                    u.liner.style("left:" + (u._itemwidth * index) + (u.itemwidth === "percent" || u.itemwidth === "%" ? "%" : "px"));
                    u.emit("hoverinitem", e, data, d, item);
                }).bind("mouseout", function (e) {
                    var index = u.find(".zonic-tab-head").selected.indexOf(u.find(".zonic-tab-head[selected='true']").selected[0]);
                    u.liner.style("left:" + (u._itemwidth * index) + (u.itemwidth === "percent" || u.itemwidth === "%" ? "%" : "px"));
                    u.emit("hoveroutitem", e, data, d, item);
                }).addClass("zonic-tab-head").attr({ "selected": "false", }).appendTo(u.find(".zonic-tab-head-container")).html(data.headText);
                return item;
            })();
        }
        if (conf.items) {
            u.additem(conf.items);
            u.find(".zonic-tab-head").eq(0).trigger("click");
        }
        return u;
    }

    zonic.ui = ui;

    zonic.serialize = function (exp) {
        var obj = {};
        zonic(exp).find("input,textarea,select").each(function () {
            var zt = zonic(this);
            if (/input|textarea|select/i.test(this.tagName)) {
                obj[zt.attr("name")] = zt.value();
            }
        });
        return obj;
    }
    zonic.deserialize = function (exp, data) {
        var obj = {};
        for (var i in data) {
            zonic(exp).find("[name=" + i + "],[name=" + i.toLowerCase() + "]").each(function () {
                var zt = zonic(this);
                if (/input|textarea|select/i.test(this.tagName)) {
                    zt.value(data[i]);
                } else {
                    zt.text(data[i]);
                }
            });
        }
        return obj;
    }

})(window, undefined);