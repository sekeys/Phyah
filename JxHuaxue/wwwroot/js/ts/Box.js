var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
define(["require", "exports", "./Zonic.base", "./Emitter", "./Style"], function (require, exports, Zonic_base_1, Emitter_1, Style_1) {
    "use strict";
    exports.__esModule = true;
    var Box;
    (function (Box_1) {
        function width() {
            return window.screen.width;
        }
        Box_1.width = width;
        function height() {
            return window.screen.height;
        }
        Box_1.height = height;
        function isFunction(obj) {
            return 'function' === typeof obj;
        }
        Box_1.isFunction = isFunction;
        function isArray(obj) {
            return Object.prototype.toString.call(obj) === "[object Array]";
        }
        Box_1.isArray = isArray;
        function isThenable(obj) {
            return obj && typeof obj['then'] == 'function';
        }
        Box_1.isThenable = isThenable;
        function transition(status, value) {
            var promise = this;
            if (promise._status !== Promise.PENDING)
                return;
            // 所以的执行都是异步调用，保证then是先执行的
            setTimeout(function () {
                promise._status = status;
                publish.call(promise, value);
            });
        }
        function publish(val) {
            var promise = this, fn, st = promise._status === Promise.FULFILLED, queue = promise[st ? '_resolves' : '_rejects'];
            while (fn = queue.shift()) {
                val = fn.call(promise, val) || val;
            }
            promise[st ? '_value' : '_reason'] = val;
            promise['_resolves'] = promise['_rejects'] = undefined;
        }
        function windowTop(isTop) {
            if (isTop) {
                return (window.top || window);
            }
            return window;
        }
        Box_1.windowTop = windowTop;
        function Document(isTop) {
            if (isTop) {
                return (window.top || window).document;
            }
            return window.document;
        }
        Box_1.Document = Document;
        function qualify(name) {
            var i = name.indexOf(":"), prefix = name;
            if (i >= 0 && (prefix = name.slice(0, i)) !== "xmlns")
                name = name.slice(i + 1);
            return this.nsPrefix.hasOwnProperty(prefix) ? {
                space: this.nsPrefix[prefix],
                local: name
            } : name;
        }
        Box_1.qualify = qualify;
        function attr(name, value) {
            name = qualify(name);
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
        Box_1.attr = attr;
        var XHR = (function () {
            function XHR(url, ajaxsetting) {
                this.contentType = "text/html";
                this.type = "";
                this.data = null;
                this.headers = {};
                this.method = "get";
                this.status = null;
                this.request = null;
                this.header = function (name, value) {
                    name = (name + "").toLowerCase();
                    if (arguments.length < 2)
                        return this.headers[name];
                    if (value == null)
                        delete this.headers[name];
                    else
                        this.headers[name] = value + "";
                    return this;
                };
                this.onprogress = function (event) {
                    try {
                        this.emit("progress", event, this.request);
                    }
                    finally {
                    }
                };
                this.MimeType = function (value) {
                    if (!arguments.length)
                        return this._mimeType;
                    this._mimeType = value == null ? null : value + "";
                    return this;
                };
                this.send = function (method, data, callback) {
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
                    this.emit("beforesend", this.request);
                    this.request.send(data == null ? null : data);
                    return this.promise;
                };
                this.url = "";
                this.contentType = "application/json";
                this.type = "";
                this.data = "";
                this.headers = {};
                this.method = "get";
                Emitter_1.zonic.Emitter.mixin(this);
                this.url = url;
                var me = this;
                this.promise = new Promise(function (resolve, reject) {
                });
                if (Zonic_base_1.zonic.isFunction(ajaxsetting)) {
                    ajaxsetting = { response: ajaxsetting };
                }
                Zonic_base_1.zonic.merge(this, ajaxsetting);
                function xhrHasResponse(request) {
                    var type = request.responseType;
                    return type && type !== "text" ? request.response : request.responseText;
                }
                function respond() {
                    var status = me.request.status, result;
                    //me.promise.deferred.done(v => {
                    //   console.log("request the " + me.url + " with status[" + status + "]");
                    //});
                    me["emit"]("load", status, me.request);
                    if (!status && xhrHasResponse(me.request) || status >= 200 && status < 300 || status === 304) {
                        try {
                            result = /xml/.test(me.mimeType) ? me.request : /json/.test(me.mimeType) ? me.request : me.request.responseText;
                            me["emit"]("success", result, me.request);
                            //d.resolve(param);
                            me.promise.resolve(result);
                        }
                        catch (e) {
                            me["emit"]("error", e);
                            var er = new Error('the processor has some error exist here:' + e);
                            me.promise.reject(er);
                            console.error(er);
                            return;
                        }
                    }
                    else {
                        me["emit"]("error", null, me.request);
                        me.promise.reject(new Error('getJSON: `' + url + '` failed with status: [' + me.status + ']'));
                    }
                }
                this.request = new XMLHttpRequest();
                if (window["XDomainRequest"] && !("withCredentials" in this.request) && /^(http(s)?:)?\/\//.test(this.url))
                    this.request = new window["XDomainRequest"]();
                this.request.onreadystatechange = function () {
                    me.request.readyState > 3 && respond();
                };
                if (ajaxsetting.mimeType) {
                    this.mimeType = ajaxsetting.mimeType;
                }
                if (ajaxsetting.response instanceof Function) {
                    me["emit"]("success", ajaxsetting.response);
                }
            }
            XHR.prototype.abort = function () {
                this.request.abort();
                this.promise.reject(new Error("custom abort request"));
                return this;
            };
            ;
            XHR.prototype.get = function (d, callback) {
                var s = "";
                if (typeof d === "object") {
                    for (var i in d) {
                        s += i + "=" + d[i] + "&";
                    }
                }
                else {
                    s = d;
                }
                return this.send.apply(this, ["get", s, callback]);
            };
            ;
            XHR.prototype.post = function (d, callback) {
                return this.send.apply(this, ["post", d, callback]);
            };
            ;
            return XHR;
        }());
        Box_1.XHR = XHR;
        function fetch(url, ajaxsetting) {
            var xhr = new XHR(url, ajaxsetting || {});
            xhr.send();
            //xhr.promise["send"] = function () { xhr.send(); };
            return xhr.promise;
        }
        Box_1.fetch = fetch;
        var Promise = (function () {
            function Promise(resolver) {
                this.then = function (onFulfilled, onRejected) {
                    var promise = this;
                    // 每次返回一个promise，保证是可thenable的
                    return new Promise(function (resolve, reject) {
                        function callback(value) {
                            var ret = isFunction(onFulfilled) && onFulfilled(value) || value;
                            if (isThenable(ret)) {
                                ret.then(function (value) {
                                    resolve(value);
                                }, function (reason) {
                                    reject(reason);
                                });
                            }
                            else {
                                resolve(ret);
                            }
                        }
                        function errback(reason) {
                            reason = isFunction(onRejected) && onRejected(reason) || reason;
                            reject(reason);
                        }
                        if (promise._status === Promise.PENDING) {
                            promise._resolves.push(callback);
                            promise._rejects.push(errback);
                        }
                        else if (promise._status === Promise.FULFILLED) {
                            callback(promise._value);
                        }
                        else if (promise._status === Promise.REJECTED) {
                            errback(promise._reason);
                        }
                    });
                };
                this["catch"] = function (onRejected) {
                    return this.then(undefined, onRejected);
                };
                if (!isFunction(resolver))
                    throw new TypeError('You must pass a resolver function as the first argument to the promise constructor');
                if (!(this instanceof Promise))
                    return new Promise(resolver);
                var promise = this;
                promise._value;
                promise._reason;
                promise._status = Promise.PENDING;
                promise._resolves = [];
                promise._rejects = [];
                var resolve = function (value) {
                    transition.apply(promise, [Promise.FULFILLED].concat([value]));
                };
                var reject = function (reason) {
                    transition.apply(promise, [Promise.REJECTED].concat([reason]));
                };
                resolver(resolve, reject);
            }
            Promise.prototype.resolve = function (value) {
                transition.apply(this, [Promise.FULFILLED].concat([value]));
                return this;
            };
            Promise.prototype.reject = function (reason) {
                transition.apply(this, [Promise.REJECTED].concat([reason]));
                return this;
            };
            Promise.prototype.delay = function (ms, val) {
                return this.then(function (ori) {
                    return Promise.delay(ms, val || ori);
                });
            };
            Promise.delay = function (ms, val) {
                return new Promise(function (resolve, reject) {
                    setTimeout(function () {
                        resolve(val);
                    }, ms);
                });
            };
            Promise.resolve = function (arg) {
                return new Promise(function (resolve, reject) {
                    resolve(arg);
                });
            };
            Promise.reject = function (arg) {
                return new Promise(function (resolve, reject) {
                    reject(arg);
                });
            };
            Promise.all = function (promises) {
                if (!isArray(promises)) {
                    throw new TypeError('You must pass an array to all.');
                }
                return new Promise(function (resolve, reject) {
                    var i = 0, result = [], len = promises.length, count = len;
                    function resolver(index) {
                        return function (value) {
                            resolveAll(index, value);
                        };
                    }
                    function rejecter(reason) {
                        reject(reason);
                    }
                    function resolveAll(index, value) {
                        result[index] = value;
                        if (--count == 0) {
                            resolve(result);
                        }
                    }
                    for (; i < len; i++) {
                        promises[i].then(resolver(i), rejecter);
                    }
                });
            };
            Promise.race = function (promises) {
                if (!isArray(promises)) {
                    throw new TypeError('You must pass an array to race.');
                }
                return new Promise(function (resolve, reject) {
                    var i = 0, len = promises.length;
                    function resolver(value) {
                        resolve(value);
                    }
                    function rejecter(reason) {
                        reject(reason);
                    }
                    for (; i < len; i++) {
                        promises[i].then(resolver, rejecter);
                    }
                });
            };
            return Promise;
        }());
        Promise.PENDING = undefined;
        Promise.FULFILLED = 1;
        Promise.REJECTED = 2;
        Box_1.Promise = Promise;
        var Box = (function () {
            function Box(dom, context) {
                if (typeof dom === "string") {
                    if (context) {
                        this.DOM = context;
                    }
                    else {
                        this.DOM = window.document;
                    }
                    this.find(dom);
                }
                else {
                    this.DOM = dom;
                }
                this.rule = new Style_1.zonic.StyleRule(this.DOM);
            }
            Box.prototype.extend = function (option) {
                return this;
            };
            Box.prototype.hasChild = function () {
                return !!this.DOM.children.length;
            };
            Box.prototype.equalsNode = function (box) {
                return box instanceof Box ? box.DOM.isEqualNode(this.DOM) : box.isEqualNode(this.DOM);
            };
            Box.prototype.prev = function () {
                return new Box(this.DOM.previousElementSibling);
            };
            Box.prototype.next = function () {
                return new Box(this.DOM.nextElementSibling);
            };
            Box.prototype.sibling = function () {
            };
            Box.prototype.children = function (exp) {
                return new Boxes(this).children(exp);
            };
            Box.prototype.appendTo = function (dom) {
                new Box(dom).append(this);
                return this;
            };
            Box.prototype.appendBefore = function (inserted) {
                this.parent().DOM.insertBefore(inserted, this.DOM);
                return this;
            };
            Box.prototype.appendAfter = function (inserted) {
                var dom = this.next().DOM;
                if (dom) {
                    this.parent().DOM.insertBefore(inserted, dom);
                }
                else {
                    this.parent().append(dom);
                }
                return this;
            };
            Box.prototype.append = function (dom) {
                if (dom instanceof Box) {
                    dom = dom;
                }
                else if (dom instanceof String) {
                    dom = new Box(dom).DOM;
                }
                this.DOM.appendChild(dom);
                return this;
            };
            Box.prototype.replace = function (old, newd) {
                return this;
            };
            Box.prototype.call = function (fn, args) {
                fn.call(this, args);
                return this;
            };
            Box.prototype.on = function (evn, fn, args) {
                if (!evn) {
                    return this;
                }
                try {
                    this.DOM.addEventListener(evn, fn, false);
                }
                catch (e) {
                    try {
                        this.DOM["attachEvent"]('on' + evn, fn);
                    }
                    catch (e) {
                        this['on' + evn] = fn;
                    }
                }
                return this;
            };
            Box.prototype.off = function (evn, fn) {
                if (!evn) {
                    return this;
                }
                try {
                    this.DOM.removeEventListener(evn, fn, false);
                }
                catch (e) {
                    try {
                        this.DOM["detachEvent"]('on' + evn, fn);
                    }
                    catch (e) {
                        delete this['on' + evn];
                    }
                }
                return this;
            };
            Box.prototype.bind = function (evn, fn, args) { return this.on(evn, fn, args); };
            Box.prototype.unbind = function (evn, fn) { return this.off(evn, fn); };
            Box.prototype.once = function (evn, fn) {
                var me = this;
                function temp() {
                    fn.apply(this, [].slice.call(arguments));
                    me.off(evn, temp);
                }
                this.on(evn, temp);
                return this;
            };
            Box.prototype.emit = function (evn, args) {
                if (!evn) {
                    return this;
                }
                if (this.DOM.dispatchEvent && document.createEvent) {
                    var e = document.createEvent("MouseEvents");
                    e.initEvent(evn, true, true);
                    this.DOM.dispatchEvent(e);
                }
                else if (document.all) {
                    this.DOM[evn]();
                }
                return this;
            };
            Box.prototype.removeClass = function (cls) {
                var r = false, j = 0, value, clses = cls.split(/\s+/), i = 0, n = clses.length;
                if (value = this.DOM.classList) {
                    while (i < n) {
                        if (value.contains(clses[i])) {
                            value.remove(clses[i]);
                        }
                        i++;
                    }
                }
                else {
                    value = this.DOM.getAttribute("class");
                    while (i < n) {
                        value = value.replace(new RegExp("(?:^|\\s+)" + Zonic_base_1.zonic.requote(clses[i]) + "(?:\\s+|$)", "g"), "");
                        i++;
                    }
                    this.DOM.setAttribute("class", value);
                }
                return this;
            };
            Box.prototype.hasClass = function (cls) {
                var r = true, j = 0, value, clses = cls.split(/\s+/), i = 0, n = clses.length;
                if (value = this.DOM.classList) {
                    while (i < n) {
                        if (!value.contains(clses[i])) {
                            r = false;
                        }
                    }
                }
                else {
                    value = this.DOM.getAttribute("class");
                    while (i < n) {
                        if (!new RegExp("(?:^|\\s+)" + Zonic_base_1.zonic.requote(clses[i]) + "(?:\\s+|$)", "g").test(value)) {
                            r = false;
                            break;
                        }
                        i++;
                    }
                    this.DOM.setAttribute("class", value);
                }
                return r;
            };
            Box.prototype.trigger = function (evn) { return this.emit(evn); };
            Box.prototype.select = function (exp) {
                var b = new Box(this.DOM);
                return b.find(exp);
            };
            Box.select = function (exp) {
                var b = new Box(window.document);
                return b.find(exp);
            };
            Box.prototype.find = function (exp) {
                //this.selected = [];
                if (!exp) {
                    return this;
                }
                if (typeof exp === "string") {
                    if (/^</.test(exp)) {
                        return this.DOM = Box.parseHtml(exp);
                    }
                    else if (/create:/i.test(exp)) {
                        return this.DOM = Box.parseHtml(exp.substr(7));
                    }
                    else {
                        return this.DOM = this.DOM.querySelector(exp.toString());
                    }
                }
                else if (exp instanceof Box) {
                    this.DOM = exp.DOM;
                    return this;
                }
                else if (exp.selected && exp.selected.length) {
                    this.DOM = exp.selected[0];
                    return this;
                }
                else if (exp.nodeType) {
                    this.DOM = exp;
                    return this;
                }
                return this;
            };
            Box.prototype.css = function (css) {
                if (css instanceof String && !/:/g.test(css.toString())) {
                    return this.DOM.get(css).value;
                }
                else {
                    var cssrule = Style_1.zonic.StyleRule.parse(css);
                    this.rule.apply(cssrule);
                    return this;
                }
            };
            Box.prototype.area = function () {
                return this.DOM.getBoundingClientRect();
            };
            Box.prototype.width = function () {
                return this.area().width || 0;
            };
            Box.prototype.height = function () {
                return this.area().height || 0;
            };
            Box.prototype.top = function () {
                return this.area().top || 0;
            };
            Box.prototype.left = function () {
                return this.area().left || 0;
            };
            Box.prototype.right = function () {
                return this.area().right || 0;
            };
            Box.prototype.bottom = function () {
                return this.area().bottom || 0;
            };
            Box.prototype.parent = function () {
                return new Box(this.DOM.parentElement);
            };
            Box.prototype.attr = function (attr, value) {
                if (value || value === "") {
                    this.DOM.setAttribute(attr, value);
                    return this;
                }
                return this.DOM.getAttribute(attr);
            };
            Box.prototype.removeAttr = function (attr) {
                this.DOM.removeAttribute(attr);
                return this;
            };
            Box.prototype.checked = function () {
                var val = "";
                return (val = this.attr("checked")) === "checked" ? true : (val === "true" ? true : false);
            };
            Box.prototype.remove = function () {
                this.DOM.parentNode ? this.DOM.parentNode.removeChild(this.DOM) : null;
                return this;
            };
            Box.prototype.value = function (value) {
                return this.prop("value", value);
            };
            Box.prototype.html = function (value) {
                return this.prop("innerHTML", value);
            };
            Box.prototype.outerHtml = function () {
                return this.prop("outerHTML");
            };
            Box.prototype.prop = function (name, value) {
                if (!value) {
                    return this.DOM[name];
                }
                this.DOM[name] = value;
                return this;
            };
            Box.prototype["class"] = function (cls) {
                if (!cls) {
                    return this;
                }
                var r = false, j = 0, value;
                if ((value = this.DOM.classList) && value.add) {
                    value.add(cls);
                }
                else {
                    value = this.DOM.getAttribute("class");
                    this.DOM.setAttribute("class", value + " " + cls);
                }
                return this;
            };
            return Box;
        }());
        Box.drag = function (moveDom, bindDom, opacity) {
            bindDom = new Box(bindDom);
            moveDom = new Box(moveDom);
            var opacity = opacity ? opacity : 1;
            var resumPointer = "";
            function parseInt1(o) {
                var i = parseInt(o);
                return isNaN(i) ? 0 : i;
            }
            var me = this;
            var listen = function () {
                new Box(bindDom).on("mousedown", function (a) {
                    var o = moveDom.node ? moveDom.node() : moveDom;
                    var d = document;
                    if (!a)
                        a = window.event;
                    if (!a.pageX)
                        a.pageX = a.clientX;
                    if (!a.pageY)
                        a.pageY = a.clientY;
                    var x = a.pageX, y = a.pageY;
                    if (o.setCapture)
                        o.setCapture();
                    else if (window.captureEvents)
                        window.captureEvents.call(window, Event["MOUSEMOVE"] | Event["MOUSEUP"]);
                    var backData = { x: moveDom.css("top"), y: moveDom.css("left") };
                    resumPointer = moveDom.css("cursor");
                    moveDom.css({ "cursor": "pointer" });
                    function move(a) {
                        if (!a)
                            a = window.event;
                        if (!a.pageX)
                            a.pageX = a.clientX;
                        if (!a.pageY)
                            a.pageY = a.clientY;
                        var tx = a.pageX - x + parseInt1(moveDom.css("left")), ty = a.pageY - y + parseInt1(moveDom.css("top"));
                        moveDom.css({ left: tx + "px", top: ty + "px" });
                        x = a.pageX;
                        y = a.pageY;
                    }
                    function up(a) {
                        if (!a)
                            a = window.event;
                        if (o.releaseCapture)
                            o.releaseCapture();
                        else if (window.captureEvents)
                            window.captureEvents.call(window, Event["MOUSEMOVE"] | Event["MOUSEUP"]);
                        d.onmousemove = null;
                        d.onmouseup = null;
                        if (!a.pageX)
                            a.pageX = a.clientX;
                        if (!a.pageY)
                            a.pageY = a.clientY;
                        if (!document.body["pageWidth"])
                            document.body["pageWidth"] = document.body.clientWidth;
                        if (!document.body["pageHeight"])
                            document.body["pageHeight"] = document.body.clientHeight;
                        setTimeout(function () { moveDom.css({ "cursor": resumPointer }); }, 0);
                        new Box(d).off("mousemove", move).off("mouseup", up);
                    }
                    new Box(d).on("mousemove", move).on("mouseup", up);
                });
            };
            if (bindDom) {
                listen();
            }
        };
        Box.parseHtml = function (tag) {
            if (typeof tag !== "string") {
                return tag;
            }
            var obj = null;
            tag = tag.trim();
            if (/^\S*<tr/i.test(tag)) {
                obj = document.createElement("tbody");
                obj.innerHTML = tag;
            }
            else if (/^\S*<td/i.test(tag)) {
                obj = document.createElement("tr");
                obj.innerHTML = tag;
            }
            else if (/^\S*<li/i.test(tag)) {
                obj = document.createElement("ul");
                obj.innerHTML = tag;
            }
            else if (/^(altGlyph|altGlyphDef|altGlyphItem|animate|animateColor|animateMotion|animateTransform|circle|clipPath|color\-profile|cursor|definition\-src|defs|desc|ellipse|feBlend|feColorMatrix|feComponentTransfer|feComposite|feConvolveMatrix|feDiffuseLighting|feDisplacementMap|feDistantLight|feFlood|feFuncA|feFuncB|feFuncG|feFuncR|feGaussianBlur|feImage|feMerge|feMergeNode|feMorphology|feOffset|fePointLight|feSpecularLighting|feSpotLight|feTile|feTurbulence|filter|font|font\-face|font\-face\-format|font\-face\-name|font\-face\-src|font\-face\-uri|foreignObject|g|glyph|glyphRef|hkern|image|line|linearGradient|marker|mask|metadata|missing\-glyph|mpath|path|pattern|polygon|polyline|radialGradient|rect|set|stop|svg|switch|symbol|text|textPath|title|tref|tspan|use|view|vkern)/i.test(tag)) {
                var xmlnames = {
                    ns: 'http://www.w3.org/2000/svg',
                    xmlns: 'http://www.w3.org/2000/xmlns/',
                    xlink: 'http://www.w3.org/1999/xlink'
                };
                var element = document.createElementNS(xmlnames.ns, tag);
            }
            else if (/^\S*</.test(tag)) {
                obj = document.createElement("div");
                obj.innerHTML = tag;
            }
            else {
                return document.createElement(tag);
            }
            if (obj.children.length > 1) {
                var boxes = new Boxes();
                boxes.selected = obj.children;
                var exval = boxes.find("script");
                if (exval.selected.length > 0) {
                    exval.each(function (d) {
                        var box = new Box(this);
                        var src = box.attr("src");
                        if (src) {
                            new Box("head").append("<script type='" + (box.attr("type") || "text/javascript") + "' src='" + src + "'></script>");
                        }
                        else {
                            var html = box.html();
                            window["eval"](html || "");
                        }
                    });
                }
                return boxes;
            }
            return new Box(obj.children[0]);
        };
        Box_1.Box = Box;
        var Boxes = (function () {
            function Boxes(exp) {
                if (exp instanceof Box) {
                    this.selected = [exp.DOM];
                }
                else if (exp instanceof HTMLElement) {
                    this.selected = [exp];
                }
                else if (exp instanceof String) {
                    this.select(exp);
                }
            }
            Boxes.prototype.first = function () {
                return this.at(0);
            };
            ;
            Boxes.prototype.lt = function (pos) {
                this.selected = this.selected.slice(0, pos);
                return this;
            };
            ;
            Boxes.prototype.gt = function (pos) {
                this.selected = this.selected.slice(pos);
                return this;
            };
            ;
            Boxes.prototype.odd = function () {
                var r = [];
                this.selected.forEach(function (d, i) {
                    if (i % 2 !== 0) {
                        r.push(d);
                    }
                });
                this.selected = r;
                return this;
            };
            ;
            Boxes.prototype.eve = function () {
                var r = [];
                this.selected.forEach(function (d, i) {
                    if (i % 2 === 0) {
                        r.push(d);
                    }
                });
                this.selected = r;
                return this;
            };
            ;
            Boxes.prototype.last = function () {
                return this.at(this.selected.length - 1);
            };
            ;
            Boxes.prototype.at = function (pos) {
                return new Box(this.selected[pos]);
            };
            ;
            Boxes.prototype.eq = function (eq) {
                if (typeof eq === "number") {
                    return this.at(eq);
                }
                else if (typeof eq === "string") {
                    return this.filter(eq);
                }
                else {
                    return eq.call(this, null);
                }
            };
            ;
            Boxes.prototype.node = function (pos) {
                if (pos != null) {
                    return this.selected[pos];
                }
                else {
                    if (this.selected.length == 1) {
                        return this.selected[0];
                    }
                    return this.selected;
                }
            };
            Boxes.prototype.attr = function (name, value) {
                if (arguments.length < 2) {
                    if (typeof name === "string") {
                        var node = this.node();
                        name = qualify(name);
                        if (!("getAttribute" in node)) {
                            return "";
                        }
                        return name.local ? node["getAttributeNS"](name.space, name.local) : node["getAttribute"](name);
                    }
                    for (value in name)
                        this.each(attr(value, name[value]));
                    return this;
                }
                return this.each(attr(name, value));
            };
            Boxes.prototype.filter = function (exp) {
                if (exp instanceof Function) {
                    return this.each(exp);
                }
                var b = [];
                var me = this;
                this.selected.forEach(function (d) {
                    if (me.selectionMatchs(d, exp)) {
                        b.push(d);
                    }
                });
                this.selected = b;
                return this;
            };
            ;
            Boxes.prototype.appendTo = function (name) {
                var me = this;
                if (name instanceof Boxes) {
                    name.each(function () {
                        var self = this;
                        me.each(function () {
                            self.appendChild(this);
                        });
                    });
                    return this;
                }
                else {
                    name = Boxes.select(name);
                    name.each(function () {
                        var self = this;
                        me.each(function () {
                            self.appendChild(this);
                        });
                    });
                }
                return this;
            };
            ;
            Boxes.prototype.empty = function () {
                this.html("");
                return this;
            };
            ;
            Boxes.prototype.has = function (node) {
                if (!arguments.length || !this.selected.length) {
                    return false;
                }
                if (node.forEach) {
                    var self = this;
                    var b = true;
                    node.forEach(function (item) {
                        self.each(function (j) {
                            b = b && item.isEqualNode ? item.isEqualNode(j) : item === j;
                        });
                    });
                    return b;
                }
                else {
                    var b = false;
                    for (var i = 0; i < this.selected.length; i++) {
                        b = b || node.isEqualNode ? node.isEqualNode(this.selected[i]) : node === this.selected[i];
                        if (b) {
                            break;
                        }
                    }
                    return b;
                }
            };
            Boxes.prototype.each = function (fn, args) {
                this.selected.forEach(function (val, index, arry) {
                    fn.apply(val, [val, index, arry].concat(args));
                });
                return this;
            };
            Boxes.prototype.eaching = function (fn, args) {
                var r = [];
                this.selected.forEach(function (val, index, arry) {
                    var rs = fn.apply(this, [val, index, arry].concat(args));
                    if (rs) {
                        r.push(rs);
                    }
                });
                return r;
            };
            Boxes.prototype.call = function (fn, pos, args) {
                if (pos != null && pos > 0) {
                    return fn.call(this.at(pos), args);
                }
                else {
                    return this.each(fn, args);
                }
            };
            Boxes.prototype.find = function (exp) {
                if (typeof exp === "string") {
                    if (/^</.test(exp)) {
                        return Boxes.select(Box.parseHtml(exp));
                    }
                    else {
                        return new Boxes().queriesSelection(exp, this);
                    }
                }
                else if (exp instanceof Boxes) {
                    return new Boxes().select(exp);
                }
                else if (exp.nodeType) {
                    return new Boxes().select(exp);
                }
            };
            Boxes.prototype.on = function (evn, fn, args) {
                if (!evn) {
                    return this;
                }
                return this.each(function () {
                    try {
                        this.addEventListener(evn, fn, false);
                    }
                    catch (e) {
                        try {
                            this.attachEvent('on' + evn, fn);
                        }
                        catch (e) {
                            this['on' + evn] = fn;
                        }
                    }
                });
            };
            Boxes.prototype.off = function (evn, fn) {
                if (!evn) {
                    return this;
                }
                return this.each(function () {
                    try {
                        this.removeEventListener(evn, fn, false);
                    }
                    catch (e) {
                        try {
                            this.detachEvent('on' + evn, fn);
                        }
                        catch (e) {
                            delete this['on' + evn];
                        }
                    }
                });
            };
            Boxes.prototype.text = function (txt, pos) {
                if (txt != null) {
                    this.call(function (d) {
                        if (this.node instanceof Function) {
                            this.node(0).innerText = txt;
                        }
                        else {
                            this.innerText = txt;
                        }
                    }, pos);
                    return this;
                }
                return this.calling(function (d) {
                    return this.node(0).innerText;
                }, this.selected.length > 1 ? pos : 0);
            };
            Boxes.prototype.value = function (val, pos) {
                if (val != null) {
                    this.call(function (d) {
                        if (this.node instanceof Function) {
                            this.node().value = val;
                        }
                        else {
                            this.value = val;
                        }
                    }, pos);
                    return this;
                }
                return this.calling(function (d) {
                    return this.node().value;
                }, pos);
            };
            Boxes.prototype.html = function (html, pos) {
                if (html != null) {
                    this.call(function (d) {
                        this.innerHTML = html;
                    }, pos);
                    return this;
                }
                return this.calling(function (d) {
                    return this.node(0).innerHTML;
                }, this.selected.length > 1 ? pos : 0);
            };
            Boxes.prototype.prop = function (name, value) {
                if (!value) {
                    var node = this.node();
                    return node[name];
                }
                node[name] = value;
                return this;
            };
            Boxes.prototype.render = function (dom) {
                if (dom instanceof Element) {
                    new Boxes(dom).html(this.html());
                }
                else if (!dom) {
                    document.writeln(this.html());
                }
                else {
                    Box.select(dom).append(this);
                }
                return this;
            };
            Boxes.prototype.remove = function () {
                this.each(function () {
                    this.parentNode ? this.parentNode.removeChild(this) : null;
                });
                return this;
            };
            Boxes.prototype["class"] = function (cls) {
                return this.attr("class", cls);
            };
            Boxes.prototype.bind = function (evn, fn, args) { return this.on(evn, fn, args); };
            Boxes.prototype.unbind = function (evn, fn) { return this.off(evn, fn); };
            Boxes.prototype.calling = function (fn, pos, args) {
                if (this.selected.length == 1) {
                    pos = 0;
                }
                if (pos != null && pos > -1) {
                    return fn.call(this.at(pos), args);
                }
                else {
                    return this.eaching(fn, args);
                }
            };
            Boxes.prototype.emit = function (evn) {
                var args = [];
                for (var _i = 1; _i < arguments.length; _i++) {
                    args[_i - 1] = arguments[_i];
                }
                if (!evn) {
                    return this;
                }
                return this.each(function () {
                    if (this.dispatchEvent && document.createEvent) {
                        var e = document.createEvent("MouseEvents");
                        e.initEvent(evn, true, true);
                        this.dispatchEvent(e);
                    }
                    else if (document.all) {
                        this[evn]();
                    }
                });
            };
            Boxes.prototype.removeClass = function (cls) {
                var r = false, j = 0, value, clses = cls.split(/\s+/), i = 0, n = clses.length;
                this.each(function () {
                    if (value = this.classList) {
                        while (i < n) {
                            if (value.contains(clses[i])) {
                                value.remove(clses[i]);
                            }
                            i++;
                        }
                    }
                    else {
                        value = this.getAttribute("class");
                        while (i < n) {
                            value = value.replace(new RegExp("(?:^|\\s+)" + Zonic_base_1.zonic.requote(clses[i]) + "(?:\\s+|$)", "g"), "");
                            i++;
                        }
                        this.setAttribute("class", value);
                    }
                    j++;
                });
                return this;
            };
            Boxes.prototype.hasClass = function (cls) {
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
                    }
                    else {
                        value = this.getAttribute("class");
                        while (i < n) {
                            //value = value.replace(, "");
                            if (!new RegExp("(?:^|\\s+)" + Zonic_base_1.zonic.requote(clses[i]) + "(?:\\s+|$)", "g").test(value)) {
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
            Boxes.prototype.trigger = function (evn) { return this.emit(evn); };
            Boxes.prototype.parseNodeList = function (col) {
                if (!this.selected) {
                    this.selected = [];
                }
                for (var i = 0; i < col.length; i++) {
                    this.selected.push(col[i]);
                }
            };
            Boxes.prototype.queriesSelection = function (exp, context) {
                if (!context || !context.querySelectorAll) {
                    if (context instanceof Boxes) {
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
            Boxes.prototype.select = function (exp, context) {
                this.selected = [];
                if (!exp) {
                    return this;
                }
                if (typeof exp === "string") {
                    if (/^</.test(exp)) {
                        return this.select(Box.parseHtml(exp));
                    }
                    else if (/create:/i.test(exp)) {
                        return this.select(Box.parseHtml(exp.substr(7)));
                    }
                    else {
                        return this.queriesSelection(exp, context);
                    }
                }
                else if (exp instanceof Boxes) {
                    this.selected = exp.selected;
                    return this;
                }
                else if (exp.selected && exp.selected.length) {
                    this.selected = exp.selected;
                    return this;
                }
                else if (exp.nodeType) {
                    this.selected.push(exp);
                    return this;
                }
                else if (Zonic_base_1.zonic.isArray(exp)) {
                    var self = this;
                    exp.forEach(function (x) { x.nodeType ? self.selected.push(x) : ""; });
                }
                return this;
            };
            Boxes.prototype.selectionMatchs = function (n, s) {
                var obj = document.createElement("div"), selectMatcher = obj["matches"] /*|| obj[temp_vendorSymbolFunc(obj, "matchesSelector")]*/, selectMatches = function (n, s) {
                    return selectMatcher.call(n, s);
                };
                return selectMatches(n, s);
            };
            Boxes.prototype.children = function (exp, pos) {
                var rs = [];
                var rns = new Boxes();
                this.call(function (d) {
                    Boxes.children(d).forEach(function (d) {
                        if (d)
                            rs.push(d);
                    });
                }, pos);
                rns.selected = rs;
                return exp ? rns.filter(exp) : rns;
            };
            ;
            Boxes.prototype.parent = function (exp, pos) {
                var rs = [];
                this.call(function (d) {
                    rs.push(d.parentNode);
                }, pos);
                this.selected = rs;
                return exp ? this.filter(exp) : this;
            };
            ;
            Boxes.prototype.next = function (exp, pos) {
                var rs = [];
                this.call(function (d) {
                    Boxes.next(d).forEach(function (d) {
                        if (d)
                            rs.push(d);
                    });
                }, pos);
                this.selected = rs;
                return exp ? this.filter(exp) : this;
            };
            ;
            Boxes.prototype.prev = function (exp, pos) {
                var rs = [];
                this.call(function (d) {
                    Boxes.prev(d).forEach(function (d) {
                        if (d)
                            rs.push(d);
                    });
                }, pos);
                this.selected = rs;
                return exp ? this.filter(exp) : this;
            };
            ;
            return Boxes;
        }());
        Boxes.children = function (d) {
            var rs = [];
            var cs = d.children; //window.document.children
            for (var i = 0; i < cs.length; i++) {
                rs.push(cs[i]);
            }
            return rs;
        };
        Boxes.select = function (exp, context) {
            return new Boxes().select(exp, context);
        };
        Boxes.next = function (d) {
            var r = d, rs = [];
            while ((r = r.nextElementSibling) != null) {
                rs.push(r);
            }
            return rs;
        };
        Boxes.prev = function (d) {
            var r = d, rs = [];
            while ((r = r.previousElementSibling) != null) {
                rs.push(r);
            }
            return rs;
        };
        Box_1.Boxes = Boxes;
        var ValidateRule = (function () {
            function ValidateRule(rule) {
                if (rule) {
                    this.RegExp = new RegExp((rule.RegExp || rule.reg || rule.rule).trim('/'));
                    this.warning = rule.warning || rule.message || "";
                    if (rule.require) {
                        this.RegExp = /\s+/;
                    }
                    if (rule.number) {
                        this.RegExp = /\d+/;
                    }
                }
            }
            ValidateRule.prototype.validate = function (val, fn) {
                if (!this.RegExp.test(val)) {
                    fn ? fn(true, this.warning, val) : "";
                    return true;
                }
                fn ? fn(false, this.warning, val) : "";
                return false;
            };
            return ValidateRule;
        }());
        var RedBorderBehavior = (function () {
            function RedBorderBehavior() {
                this.borderColor = "";
            }
            RedBorderBehavior.prototype.apply = function (message, val) {
                this.borderColor = this.binded.css("border-color");
                this.binded.css({ "border-color": RedBorderBehavior.Color || "red" }).attr("title", message);
            };
            RedBorderBehavior.prototype.unapply = function () {
                this.binded.css({ "border-color": this.borderColor }).removeAttr("title");
            };
            return RedBorderBehavior;
        }());
        RedBorderBehavior.Color = "";
        Box_1.RedBorderBehavior = RedBorderBehavior;
        var Behavior = (function () {
            function Behavior() {
            }
            return Behavior;
        }());
        Behavior.behaviors = {
            redborder: RedBorderBehavior.prototype.constructor
        };
        Box_1.Behavior = Behavior;
        var PopupBehavior = (function () {
            function PopupBehavior() {
            }
            PopupBehavior.prototype.apply = function (message, val) {
                var area = this.binded.area();
            };
            PopupBehavior.prototype.unapply = function () {
            };
            return PopupBehavior;
        }());
        Box_1.PopupBehavior = PopupBehavior;
        var Validate = (function () {
            function Validate(dom) {
                this.mode = "auto";
                this.message = "";
                this.rules = [];
                if (dom) {
                    this.binded = new Box(dom);
                }
            }
            Validate.prototype.addRules = function (rule) {
                if (typeof rule === "string") {
                    rule = JSON.parse(rule);
                }
                this.rules.push(new ValidateRule(rule));
            };
            Validate.prototype.__validate = function (val) {
                for (var r in this.rules) {
                    if (this.rules[r].validate(val)) {
                        this.message = this.rules[r].warning;
                        return true;
                    }
                }
                this.message = "";
                return false;
            };
            Validate.prototype.bind = function (dom) {
                if (dom) {
                    this.binded = new Box(dom);
                }
                var me = this;
                var rule = this.binded.attr("validate-rule");
                rule = JSON.parse(rule);
                this.behavior = new Behavior.behaviors[rule.behavior]();
                this.behavior.binded = this.binded;
                for (var i in rule.rules) {
                    this.addRules(rule.rules[i]);
                }
                //{behavior:"",mode:"auto",rules:[]}
                this.binded.on("blur", function (e) {
                    var value = me.binded.value();
                    if (me.__validate(value)) {
                        me.binded.attr("validate-result", "true");
                        me.behavior.apply(me.message, value);
                    }
                }).on("focus", function () {
                    me.binded.attr("validate-result", null);
                    me.behavior.unapply();
                });
            };
            Validate.prototype.register = function (behavior, unbehavior) {
                this.behavior = behavior || this.behavior;
            };
            Validate.prototype.validate = function () {
                var value = this.binded.value(), me = this;
                me.binded = this.binded;
                this.binded.once("focus", function () {
                    me.binded.attr("validate-result", null);
                    me.behavior.unapply();
                });
                if (this.__validate(value)) {
                    me.behavior.apply(me.message, value);
                    return false;
                }
                return true;
            };
            Validate.validate = function () {
            };
            Validate.prototype.result = function () {
                return /true/ig.test(this.binded.attr("validate-result"));
            };
            return Validate;
        }());
        Box_1.Validate = Validate;
        var Slideshow = (function (_super) {
            __extends(Slideshow, _super);
            function Slideshow(box) {
                var _this = _super.call(this, box) || this;
                _this.count = 0;
                _this.currentIndex = -1;
                _this.loop = false;
                _this._width = 0;
                _this._height = 0;
                _this.interval = 5000;
                Emitter_1.zonic.Emitter.mixin(_this);
                if (_this.hasChild()) {
                    _this._children = _this.children();
                    if (_this._children.selected.length == 1 && _this._children[0].hasClass("slide-options")) {
                    }
                    _this.html("");
                    _this.count = _this._children.selected.length;
                }
                _this.wrapper = new Box("create:div");
                //transition: all 1000ms ease; transform: translate3d(-5760px, 0px, 0px);
                _this.wrapper.rule.css({ width: _this.width() + "px", height: "100%", "margin": "0px", padding: "0px", "transition": "all 1000ms ease", transform: "translate3d(0px, 0px, 0px)" });
                _this.wrapper.appendTo(_this);
                for (var i = 0; i < _this.count; i++) {
                    _this.wrapper.append(_this._children);
                }
                _this.goto(0);
                return _this;
            }
            Slideshow.prototype.get = function (url, data) {
                new Promise(function () {
                });
                return this;
            };
            Slideshow.prototype.indexof = function (witch) {
                for (var i = 0; i < this.count; i++) {
                    if (this.equalsNode(this._children[i])) {
                        return i;
                    }
                }
                return -1;
            };
            Slideshow.prototype.goto = function (index) {
                if (index >= this.count) {
                    console.warn("slideshow 跳转索引大于子节点集长度");
                    return this;
                }
                if (index < 0) {
                    console.warn("slideshow 跳转索引小于子节点集长度");
                    return this;
                }
                this.wrapper.rule.css({ transform: "translate3d(" + (this._width * (index + 1)) + "px, 0px, 0px)" });
                this.current = this._children[index];
                this.currentIndex = index;
                //this.emit("slidechanged")
                return this;
            };
            Slideshow.prototype.next = function () {
                this.currentIndex++;
                if (this.currentIndex >= this.count && this.loop) {
                    this.currentIndex = 0;
                }
                this.goto(this.currentIndex);
                return this;
            };
            Slideshow.prototype.prev = function () {
                this.currentIndex--;
                if (this.currentIndex < 0 && this.loop) {
                    this.currentIndex = this.count + 1;
                }
                this.goto(this.currentIndex);
                return this;
            };
            Slideshow.prototype.rerender = function () {
            };
            Slideshow.prototype.start = function () {
                this.timerid = window.setInterval(function () {
                    this.next();
                }, this.interval);
            };
            Slideshow.prototype.clear = function () {
                this.currentIndex = 0;
                this.current = null;
                window.clearInterval(this.timerid);
            };
            Slideshow.prototype.stop = function () {
                window.clearInterval(this.timerid);
            };
            return Slideshow;
        }(Box));
        Box_1.Slideshow = Slideshow;
        var Layer = (function (_super) {
            __extends(Layer, _super);
            function Layer(dom) {
                var _this = _super.call(this, null) || this;
                _this.zindex = 10;
                _this.DOM = Box.parseHtml("div");
                return _this;
            }
            Layer.prototype.initialize = function (conf) {
                conf = conf || {};
                var me = this;
                this.zindex = conf.zindex || 10;
                this.rule.apply({
                    top: "0px", left: "0px", opacity: "0", position: conf.position || "absolute;", "z-index": this.zindex, width: conf.width || "40%",
                    height: conf.height || "300px"
                });
                this["class"](conf.mode == "hover" ? "zonic-section-layer-hover" : (conf.mode == "mask" ? "zonic-section-mask" : "zonic-section-layer"));
                if (conf.mode == "mask") {
                    this.rule.apply({ left: "0px", right: "0px", top: "0px", bottom: "0px", width: window.screen.availWidth + "px", height: window.screen.height + "px", opacity: 0.4, position: "fixed" });
                }
                if ("D3" in conf) {
                    this.attr("layer-box-3D", "D3");
                }
                if (conf.center) {
                    window.setTimeout(function () {
                        var r = me.area(), width = r.width, height = r.height;
                        me.rule.apply({
                            opacity: conf.mode == "mask" ? 0.45 : 1,
                            top: ((conf.top || window.screen.availHeight / 2) - height / 1.8) + "px", left: ((conf.left || window.screen.availWidth / 2) - width / 2) + "px"
                        });
                    }, 0);
                }
                else {
                    window.setTimeout(function () { me.rule.apply({ opacity: conf.mode == "mask" ? 0.45 : 1 }); }, 0);
                }
                if (!Zonic_base_1.zonic.hasProp(conf, "dragable") || conf.dragable)
                    Box.drag(this, this, 0.3);
                return this;
            };
            Layer.prototype.layout = function (css) {
                return this.rule.apply(css);
            };
            Layer.prototype.render = function (dom) {
                this.appendTo(dom || new Box("body"));
                return this;
            };
            return Layer;
        }(Box));
        Box_1.Layer = Layer;
        var Popup = (function (_super) {
            __extends(Popup, _super);
            function Popup() {
                var _this = _super.call(this) || this;
                _this.zindex = 10;
                _this.show2Top = true;
                _this.speed = 300;
                return _this;
            }
            Popup.prototype.initialize = function (conf) {
                conf = conf || {};
                conf.mode = conf.mode ? conf.mode : "hover";
                conf.center = true;
                conf.width = conf.width || "210px";
                conf.height = conf.height || "60px";
                conf.position = "absolute";
                conf.background = conf.bgcolor || "white";
                conf.center = true;
                this.zindex = conf.zindex;
                _super.prototype.initialize.call(this, conf);
                return this;
            };
            Popup.prototype.close = function () {
                this.emit("closed", []);
                return this.remove();
            };
            Popup.prototype.render = function (html, dom) {
                if (html) {
                    if (Zonic_base_1.zonic.isFunction(html)) {
                        html.call(this, dom);
                    }
                    else {
                        this.append(html);
                    }
                }
                var me = this;
                _super.prototype.render.call(this, new Box("body", Document(this.show2Top)));
                return this;
            };
            Popup.prototype.show = function (speed) {
                return this;
            };
            return Popup;
        }(Layer));
        Popup.popups = [];
        Box_1.Popup = Popup;
        var Select = (function (_super) {
            __extends(Select, _super);
            function Select() {
                var _this = _super.call(this, null) || this;
                _this.type = "text";
                _this.selected = [];
                _this.selectedValue = "";
                _this.selectedText = "";
                _this.items = [];
                _this.DOM = Box.parseHtml("div");
                _this.txt = "";
                return _this;
            }
            Select.prototype.initialize = function (conf) {
                conf = conf || {};
                conf.labelColor = conf.labelColor || "rgba(0, 0, 0, 0.298039)";
                this.height = conf.height || 72;
                this.width = conf.width || 256;
                this.label = conf.label || "";
                this.hintColor = conf.hintColor;
                this.hint = conf.hint;
                this.type = conf.type;
                this.placeholder = conf.placeholder;
                if (!conf.label && conf.placeholder) {
                    this.label = conf.placeholder;
                }
                this.list = new List("ul");
                return this;
            };
            Select.prototype.render = function (dom) {
                var u = this;
                var height = this._height;
                u.rule.apply("transition:height 200ms cubic-bezier(0.23, 1, 0.32, 1); width: " + (u.width || 256) + "px; height: " + (u._height || 256) + "px; line-height: " + (u._height * 0.33) + "px; font-family: Roboto,sans-serif; font-size: 16px; display: inline-block; position: relative; cursor: auto; background-color: transparent;");
                u.append((function () {
                    var returnBox = new Box((Box.parseHtml("label")).rule.apply("transition:450ms cubic-bezier(0.23, 1, 0.32, 1); top: " + (u._height * 0.605) + "px; color: rgba(0, 0, 0, 0.3); line-height: " + (u._height * 0.405) + "px; position: absolute; z-index: 1; pointer-events: none; transform: scale(1) translate(0px, 0px); transform-origin: left top 0px;").DOM)["class"]("label").html(u.label || "&nbsp;");
                    returnBox.append(new Box(Box.parseHtml("div").rule.apply('font:inherit; padding: 0px; outline: invert; border: medium; transition:450ms cubic-bezier(0.23, 1, 0.32, 1); border-image: none; width: 100%; height: 100%; color: rgba(0, 0, 0, 0.87); margin-top: ' + (u._height * 0.2) + 'px; display: block; position: relative; cursor: inherit; box-sizing: border-box; font-size-adjust: none; font-stretch: inherit; -webkit-tap-highlight-color: rgba(0, 0, 0, 0); background-color: rgba(0, 0, 0, 0)').DOM)
                        .call(function () {
                        new Box(Box.parseHtml("div")).css({ "padding-left": "5px", cursor: "pointer", height: "100%", position: "relative", width: "100%" })
                            .append(Box.parseHtml("div").rule.apply("box-sizing:border-box;content: \" \";display:table;").DOM)
                            .append(new Box(Box.parseHtml("div").rule.apply("top: " + (height * 0.21) + "px; color: rgba(0, 0, 0, 0.87); line-height: " + (height * 0.8) + "px; overflow: hidden; padding-right: 56px; padding-left: 0px; white-space: nowrap; position: relative;text-overflow: ellipsis; opacity: 1;").DOM)["class"]("zonic-select-area").html(u.txt || "&nbsp;"))
                            .append(Box.parseHtml("button").attr({ type: "button" }).css({ "border": (height * 0.117) + "px", " box-sizing": "border-box", "display": "inline-block", "font-family": " Roboto, sans-serif", " -webkit-tap-highlight-color": " rgba(0, 0, 0, 0)", " cursor": " pointer", " text-decoration": " none", " margin": " 0px", "padding": (height * 0.1067) + "px", "outline": " none", "font-size": " 0px", "font-weight": " inherit", "position": "absolute", "z-index": " 1", "overflow": " visible", "transition": " all 450ms cubic-bezier(0.23, 1, 0.32, 1) 0ms", " width": (height * 0.76067) + "px", " height": (height * 0.76067) + "px", " fill": " rgb(224, 224, 224)", " right": " 0px", " top": (height * 0.30) + "px", " background": " none" }).html('<div><svg viewBox="0 0 ' + (height * 0.33) + ' ' + (height * 0.33) + '" style="display: inline-block; color: rgba(0, 0, 0, 0.870588); fill: inherit; height: ' + (height * 0.33) + 'px; width: ' + (height * 0.33) + 'px; user-select: none; transition: all 450ms cubic-bezier(0.23, 1, 0.32, 1) 0ms;"><path d="M7 10l5 5 5-5z"></path></svg></div>'))
                            .append(Box.parseHtml("div").css({ "border-top": "none", "bottom": "1px", left: "0px", margin: "-1px 24px", right: "0px", position: "absolute" }))
                            .append(Box.parseHtml("div").rule.apply("box-sizing:border-box;content: \" \";display:table;clear: both;").DOM)
                            .appendTo(this);
                    }).bind("click", function () {
                        if (u.layer) {
                            return;
                        }
                        var rect = u.area();
                        var top = u.selected[0].offsetTop;
                        u.layer = new Layer();
                        u.layer.initialize({ D3: true, position: "fixed", dragable: false, width: "0px", height: "0px" })["class"]("zonic-select").
                            rule.apply("transition:height 1s cubic-bezier(0.23, 1, 0.32, 1) 0ms;").apply("width: " + (u.width || rect.width) + "px;height:" + (u.layerHeight || 160) + "px;border-radius:2px; left: " + (rect.left + (u.horizontal == "right" ? rect.width : (u.horizontal == "middle" ? rect.width / 2 : 0))) + "px; top: " + (rect.top + u._height * 0.2 + (u.vertical == "top" ? 0 : (u.vertical == "center" ? rect.height / 2 : rect.height))) + "px;");
                        u.find("label.label").css({ transform: "scale(0.75) translate(0px, " + (-u.height * 0.10) + "px)", color: u.labelColor || "rgba(0, 0, 0, 0.298039)" });
                        //u.find("label.")
                        u.list = new List("ul").on("selectchanged", function (e, data, item) {
                            if (!Zonic_base_1.zonic.isObject(data)) {
                                u.selectedValue = u.selectedText = data;
                                u.find(".zonic-select-area").html(data).attr({ "zonic-selected-value": data });
                            }
                            else {
                                u.selectedValue = data.value;
                                u.selectedText = data.text;
                                u.find(".zonic-select-area").html(data.text).attr({ "zonic-selected-value": data.value });
                            }
                            if (!u.selectedValue) {
                                u.find("label.label").css({ transform: "scale(1) translate(0px, 0px)", color: u.labelColor });
                            }
                            u.emit("selectchanged", Zonic_base_1.zonic.slice(arguments));
                        });
                        u.list.additem(u.items);
                        u.list.appendTo(u.layer);
                        u.layer.render("body");
                    })
                        .append((function () {
                        return new Box(Box.parseHtml("div")).html('<hr style="border-width: medium medium 1px; border-style: none none solid; border-color: rgb(224, 224, 224); margin: 0px; width: 100%; bottom:' + (u._height * 0.06) + 'px; position: absolute; box-sizing: content-box;">');
                    })()));
                    return returnBox;
                })());
                Box.select("body").on("click", function (e) {
                    var e = e || window.event;
                    if (u.DOM.isEqualNode(e.target) ||
                        u.find("*").has(e.target)) {
                        return;
                    }
                    u.layer.remove();
                    u.layer = null;
                });
                if (dom) {
                    this.render(dom);
                }
                return this;
            };
            Select.prototype.additem = function () {
                //this.list.additem.apply(this.list, helper.slice(arguments))
                this.items = this.items.concat(Zonic_base_1.zonic.slice(arguments));
                return this;
            };
            return Select;
        }(Box));
        Box_1.Select = Select;
        var List = (function (_super) {
            __extends(List, _super);
            function List(dom) {
                var _this = _super.call(this, null) || this;
                _this.multi = false;
                _this.itemCss = null;
                _this.dragable = false;
                _this.DOM = Box.parseHtml(dom || "ul");
                _this["class"]("zonic-list");
                Emitter_1.zonic.Emitter.mixin(_this);
                return _this;
            }
            List.prototype.additem = function (data) {
                var me = this;
                if (Zonic_base_1.zonic.isArray(data)) {
                    data.forEach(function (i) { me.additem(i); });
                    return me;
                }
                return (function () {
                    var d = data;
                    var item = new Box(Box.parseHtml("li"));
                    item["uiParent"] = this;
                    item.on("click", function (e) {
                        if (item.attr("disabled")) {
                            return;
                        }
                        if (!me.multi) {
                            me.find(".zonic-list-item[selected='true']").attr({ "selected": "false" });
                        }
                        if (item.attr("selected") == "true") {
                            item.attr({ "selected": "false" });
                        }
                        else {
                            item.attr({ "selected": "true" });
                        }
                        me.emit("selectchanged", [e, data, item]);
                    }).bind("mouseenter", function (e) {
                        me.emit("hoveritem", [e, data, item]);
                    })["class"]("zonic-list-item").attr({ "selected": "false" }).appendTo(me)["if"](typeof data !== "object", function () {
                        return item.html(data);
                    })["if"](data.text || data.html, function () {
                        return item.html(data.text || data.html);
                    })["if"](data.value, function () {
                        return item.attr("value", data.value);
                    }).css(me.itemCss || {});
                    return item;
                })();
            };
            List.prototype.binddata = function (conf, data) {
                if (arguments.length = 1) {
                    data = conf.data;
                    conf = conf.conf;
                }
                return this;
            };
            List.prototype.initialize = function (conf) {
                if (conf) {
                    this.itemCss = conf.itemCss || {};
                    this.dragable = Zonic_base_1.zonic.hasProp(conf, "dragable") ? conf.dragable : false;
                }
                return this;
            };
            List.prototype.render = function (dom) {
                if (dom) {
                    this.appendTo(new Box(dom));
                }
            };
            return List;
        }(Box));
        Box_1.List = List;
        var Text = (function (_super) {
            __extends(Text, _super);
            function Text() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            return Text;
        }(Box));
        Box_1.Text = Text;
        var Item = (function (_super) {
            __extends(Item, _super);
            function Item() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            return Item;
        }(Box));
        Box_1.Item = Item;
        var TreeItem = (function (_super) {
            __extends(TreeItem, _super);
            function TreeItem() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            return TreeItem;
        }(Item));
        Box_1.TreeItem = TreeItem;
        var Tree = (function (_super) {
            __extends(Tree, _super);
            function Tree() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            return Tree;
        }(Box));
        Box_1.Tree = Tree;
        var Checkbox = (function (_super) {
            __extends(Checkbox, _super);
            function Checkbox() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            return Checkbox;
        }(Box));
        Box_1.Checkbox = Checkbox;
        var CheckboxGroup = (function (_super) {
            __extends(CheckboxGroup, _super);
            function CheckboxGroup() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            return CheckboxGroup;
        }(Box));
        Box_1.CheckboxGroup = CheckboxGroup;
    })(Box = exports.Box || (exports.Box = {}));
    exports["default"] = Box;
});
