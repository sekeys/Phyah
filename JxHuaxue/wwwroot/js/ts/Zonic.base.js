define(["require", "exports"], function (require, exports) {
    "use strict";
    exports.__esModule = true;
    var zonic;
    (function (zonic) {
        var base = {}, op = Object.prototype, ostring = op.toString, hasOwn = op.hasOwnProperty, _isBrowser = !!(typeof window !== 'undefined' && typeof navigator !== 'undefined' && window.document), _isWebWorker = !isBrowser && typeof importScripts !== 'undefined';
        function eachProp(obj, func) {
            var prop;
            for (prop in obj) {
                if (hasProp(obj, prop)) {
                    if (func(obj[prop], prop)) {
                        break;
                    }
                }
            }
        }
        function calling(fn, pos, args) {
            if (this.selected.length == 1) {
                pos = 0;
            }
            if (pos != null && pos > -1) {
                return fn.call(this.at(pos), args);
            }
            else {
                return this.eaching(fn, args);
            }
        }
        function Window(top) {
            return top ? (window.top || window) : window;
        }
        zonic.Window = Window;
        function Document(top) {
            return Window(top).document;
        }
        zonic.Document = Document;
        function if_(exp, fn1, fn2) {
            if (arguments.length == 0) {
                return this;
            }
            if (arguments.length == 1) {
                return exp;
            }
            else {
                if (exp) {
                    return fn1.call(this, null);
                }
                else {
                    return fn2.call(this, null);
                }
            }
        }
        zonic.if_ = if_;
        function slice(list) {
            return [].slice.call(list);
        }
        zonic.slice = slice;
        ;
        function isBrowser() {
            return (_isBrowser = !!(typeof window !== 'undefined' && typeof navigator !== 'undefined' && window.document));
        }
        zonic.isBrowser = isBrowser;
        function isWebWorker() {
            return !_isBrowser && typeof importScripts !== 'undefined';
        }
        zonic.isWebWorker = isWebWorker;
        function isFunction(it) {
            return ostring.call(it) === '[object Function]';
        }
        zonic.isFunction = isFunction;
        function isArray(it) {
            return ostring.call(it) === '[object Array]';
        }
        zonic.isArray = isArray;
        function isObject(it) {
            return ostring.call(it) === '[object Object]';
        }
        zonic.isObject = isObject;
        function merge() {
            var obj = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                obj[_i] = arguments[_i];
            }
            var i = 1, target, key, obj1 = obj[0];
            for (; i < obj.length; i++) {
                target = obj[i];
                for (key in target) {
                    if (Object.prototype.hasOwnProperty.call(obj1, key)) {
                        if (isObject(target[key]) && (isObject(obj1[key]))) {
                            merge(obj1[key], target[key]);
                        }
                        else {
                            obj1[key] = target[key];
                        }
                    }
                    else {
                        obj1[key] = target[key];
                    }
                }
            }
            return obj1;
        }
        zonic.merge = merge;
        function clone(ob, deep) {
            if (!deep) {
                for (var i in ob) {
                    this[i] = ob[i];
                }
            }
            else {
                for (var i in ob) {
                    if (isObject(ob[i]) && !isFunction(ob[i])) {
                        this[i] = new ob[i].constructor();
                        clone.call(this[i], ob[i]);
                    }
                    else {
                        this[i] = ob[i];
                    }
                }
            }
        }
        zonic.clone = clone;
        function eachReverse(ary, func) {
            if (ary) {
                var i;
                for (i = ary.length - 1; i > -1; i -= 1) {
                    if (ary[i] && func(ary[i], i, ary)) {
                        break;
                    }
                }
            }
        }
        zonic.eachReverse = eachReverse;
        function hasProp(obj, prop) {
            return hasOwn.call(obj, prop);
        }
        zonic.hasProp = hasProp;
        function getOwnfunction(obj, prop) {
            return hasProp(obj, prop) && obj[prop];
        }
        zonic.getOwnfunction = getOwnfunction;
        function mixin(target, source, force, deepStringMixin) {
            if (source) {
                eachProp(source, function (value, prop) {
                    if (force || !hasProp(target, prop)) {
                        if (deepStringMixin && typeof value === 'object' && value &&
                            !isArray(value) && !isFunction(value) &&
                            !(value instanceof RegExp)) {
                            if (!target[prop]) {
                                target[prop] = {};
                            }
                            mixin(target[prop], value, force, deepStringMixin);
                        }
                        else {
                            target[prop] = value;
                        }
                    }
                });
            }
            return target;
        }
        zonic.mixin = mixin;
        function bind(obj, fn) {
            return function () {
                return fn.apply(obj, arguments);
            };
        }
        zonic.bind = bind;
        function logicAnd() {
            if (arguments.length) {
                var param = true;
                for (var i = 0; i < arguments.length; i++) {
                    param = param && !!(arguments[i] instanceof Function ? arguments[i]() : arguments[i]);
                }
                return param;
            }
            else {
                return false;
            }
        }
        zonic.logicAnd = logicAnd;
        function logicOr() {
            if (arguments.length) {
                var param = false;
                for (var i = 0; i < arguments.length; i++) {
                    param = param || !!(arguments[i] instanceof Function ? arguments[i]() : arguments[i]);
                }
                return param;
            }
            else {
                return false;
            }
        }
        zonic.logicOr = logicOr;
        function sequence() {
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
        zonic.sequence = sequence;
        function querystring() {
            var qs = {};
            var href = window.location.href;
            var indexofPos = href.lastIndexOf('?'), indexofj = href.indexOf("#");
            if (indexofj < indexofPos) {
                href = href.substr(indexofPos);
            }
            else {
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
        zonic.querystring = querystring;
        function guid(seqchar) {
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
        zonic.guid = guid;
        function requote(s) {
            return s.replace(/[\\\^\$\*\+\?\|\[\]\(\)\.\{\}]/g, "\\$&");
        }
        zonic.requote = requote;
        ;
        function rebind(target, source, arg) {
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
        zonic.rebind = rebind;
    })(zonic = exports.zonic || (exports.zonic = {}));
});
