import { zonic as helper } from './Zonic.base'
import { zonic as emitter } from './Emitter'
import { zonic as color } from './Color'
import { zonic as style } from './Style'

export module Box {
    export function width() {
        return window.screen.width;
    }
    export function height() {
        return window.screen.height;
    }
    export function isFunction(obj) {
        return 'function' === typeof obj;
    }
    export function isArray(obj) {
        return Object.prototype.toString.call(obj) === "[object Array]";
    }
    export function isThenable(obj) {
        return obj && typeof obj['then'] == 'function';
    }
    function transition(status, value) {
        var promise = this;
        if (promise._status !== Promise.PENDING) return;
        // 所以的执行都是异步调用，保证then是先执行的
        setTimeout(function () {
            promise._status = status;
            publish.call(promise, value);
        });
    }
    function publish(val) {
        var promise = this,
            fn,
            st = promise._status === Promise.FULFILLED,
            queue = promise[st ? '_resolves' : '_rejects'];

        while (fn = queue.shift()) {
            val = fn.call(promise, val) || val;
        }
        promise[st ? '_value' : '_reason'] = val;
        promise['_resolves'] = promise['_rejects'] = undefined;
    }
    export function windowTop(isTop) {
        if (isTop) {
            return (window.top || window);
        }
        return window;
    }
    export function Document(isTop) {
        if (isTop) {
            return (window.top || window).document;
        }
        return window.document;
    }
    export function qualify(name) {
        var i = name.indexOf(":"), prefix = name;
        if (i >= 0 && (prefix = name.slice(0, i)) !== "xmlns")
            name = name.slice(i + 1);
        return this.nsPrefix.hasOwnProperty(prefix) ? {
            space: this.nsPrefix[prefix],
            local: name
        } : name;
    }
    export function attr(name, value) {
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
    export class XHR {
        url: string
        mimeType: string
        contentType = "text/html"
        type = ""
        data = null
        headers = {}
        method = "get"
        promise: Promise
        status = null
        constructor(url, ajaxsetting?) {
            this.url = "";
            this.contentType = "application/json";
            this.type = "";
            this.data = "";

            this.headers = {};
            this.method = "get";
            emitter.Emitter.mixin(this);
            this.url = url;
            var me = this;
            this.promise = new Promise(function (resolve, reject) {
            });
            if (helper.isFunction(ajaxsetting)) {
                ajaxsetting = { response: ajaxsetting };
            }
            helper.merge(this, ajaxsetting);

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
                    } catch (e) {
                        me["emit"]("error", e);
                        var er = new Error('the processor has some error exist here:' + e);
                        me.promise.reject(er);
                        console.error(er);
                        return;
                    }
                } else {
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
        request = null;
        header = function (name, value) {
            name = (name + "").toLowerCase();
            if (arguments.length < 2)
                return this.headers[name];
            if (value == null)
                delete this.headers[name];
            else
                this.headers[name] = value + "";
            return this;
        };
        onprogress = function (event) {
            try {
                this.emit("progress", event, this.request);
            } finally {
            }
        };
        MimeType = function (value) {
            if (!arguments.length)
                return this._mimeType;
            this._mimeType = value == null ? null : value + "";
            return this;
        };
        send = function (method?, data?, callback?) {
            if (arguments.length === 2 && typeof data === "function")
                callback = data, data = null;
            this.method = method || this.method;
            if (data) {
                data = helper.merge(data, this.data);
            } else {
                data = this.data;
            }
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
                if (!/head|get/i.test(this.method)) {
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
        abort() {
            this.request.abort();
            this.promise.reject(new Error("custom abort request"));
            return this;
        };
        get(d, callback) {
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
        post(d, callback) {
            return this.send.apply(this, ["post", d, callback]);
        };
    }
    export function fetch(url, ajaxsetting?) {
        var xhr = new XHR(url, ajaxsetting || {});
        xhr.send();
        //xhr.promise["send"] = function () { xhr.send(); };
        return xhr.promise;
    }
    export class Promise {
        static PENDING = undefined
        static FULFILLED = 1
        static REJECTED = 2;
        private _value: any;
        private _reason: any;
        private _status: any
        private _resolves: any[]
        private _rejects: any[]
        constructor(resolver) {
            if (!isFunction(resolver))
                throw new TypeError('You must pass a resolver function as the first argument to the promise constructor');
            if (!(this instanceof Promise)) return new Promise(resolver);

            var promise = this;
            promise._value;
            promise._reason;
            promise._status = Promise.PENDING;
            promise._resolves = [];
            promise._rejects = [];

            var resolve = function (value) {
                transition.apply(promise, [Promise.FULFILLED].concat([value]));
            }
            var reject = function (reason) {
                transition.apply(promise, [Promise.REJECTED].concat([reason]));
            }

            resolver(resolve, reject);
        }
        resolve(value) {
            transition.apply(this, [Promise.FULFILLED].concat([value]));
            return this;
        }
        reject(reason) {
            transition.apply(this, [Promise.REJECTED].concat([reason]));
            return this;
        }
        then = function (onFulfilled, onRejected?) {
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
                    } else {
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
                } else if (promise._status === Promise.FULFILLED) { // 状态改变后的then操作，立刻执行
                    callback(promise._value);
                } else if (promise._status === Promise.REJECTED) {
                    errback(promise._reason);
                }
            });
        }
        catch = function (onRejected) {
            return this.then(undefined, onRejected)
        }
        delay(ms, val) {
            return this.then(function (ori) {
                return Promise.delay(ms, val || ori);
            })
        }
        static delay(ms, val?) {
            return new Promise(function (resolve, reject) {
                setTimeout(function () {
                    resolve(val);
                }, ms);
            })
        }
        static resolve(arg) {
            return new Promise(function (resolve, reject) {
                resolve(arg)
            })
        }
        static reject(arg) {
            return new Promise(function (resolve, reject) {
                reject(arg)
            })
        }
        static all(promises) {
            if (!isArray(promises)) {
                throw new TypeError('You must pass an array to all.');
            }
            return new Promise(function (resolve, reject) {
                var i = 0,
                    result = [],
                    len = promises.length,
                    count = len

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
                        resolve(result)
                    }
                }

                for (; i < len; i++) {
                    promises[i].then(resolver(i), rejecter);
                }
            });
        }
        static race(promises) {
            if (!isArray(promises)) {
                throw new TypeError('You must pass an array to race.');
            }
            return new Promise(function (resolve, reject) {
                var i = 0,
                    len = promises.length;

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
        }
    }
    export class Box {
        rule: style.StyleRule
        DOM: any
        constructor(dom: any, context?) {

            if (typeof dom === "string") {
                if (context) {
                    this.DOM = context;
                } else {
                    this.DOM = window.document;
                }
                this.find(dom);
            } else if (dom instanceof Box) {
                this.DOM = dom.DOM;
            }
            else {
                this.DOM = dom;
            }
            this.rule = new style.StyleRule(this.DOM);
        }
        extend(option) {
            return this;
        }
        show() {
            this.rule.apply({ opacity: 1, display: "block" });
        }
        hide() {
            this.rule.apply({ opacity: 0, display: "none" });
        }
        click(fn) {
            this.on("click", fn);
            return this;
        }
        dbclick(fn) {
            this.on("dbclick", fn);
            return this;
        }
        hasChild() {
            return !!this.DOM.children.length;
        }
        equalsNode(box) {
            return box instanceof Box ? box.DOM.isEqualNode(this.DOM) : box.isEqualNode(this.DOM);
        }
        prev() {
            return new Box((this.DOM as HTMLElement).previousElementSibling);
        }
        next() {
            return new Box((this.DOM as HTMLElement).nextElementSibling);
        }
        sibling() {

        }
        formJson(ignore?) {
            var json = {};
            this.selects("input,select,.camouflage-form").each(function (item) {
                var b = new Box(item), val = null, name = b.attr("name");
                if (!name) { return; }
                if (item.tagName.toLowerCase() === "input" || item.tagName.toLowerCase() === "select") {
                    val = b.value();
                    if (val == null) {
                        ignore ? "" : json[name] = null;
                    } else {
                        json[name] = val;
                    }
                } else {
                    val = b.attr("value");
                    if (!val) {
                        ignore ? "" : json[name] = null;
                    } else {
                        json[name] = val;
                    }
                }
            });
            return json;
        }
        deformJson(json) {
            this.selects("input,select,.camouflage-form").each(function (item) {
                var b = new Box(item), val = null, name = b.attr("name");
                if (!name || json[name] == null || json[name] == undefined) { return; }
                if (item.tagName.toLowerCase() === "input" || item.tagName.toLowerCase() === "select") {
                    b.value(json[name]);
                } else {
                    b.attr("value", json[name]);
                    if (b.hasClass("camouflage-text")) {
                        b.html(json[name]);
                    }
                }
            });
            return json;
        }
        children(exp?) {
            return new Boxes(this).children(exp);
        }
        appendTo(dom) {
            new Box(dom).append(this);
            return this;
        }
        appendBefore(inserted) {
            (this.parent().DOM as HTMLElement).insertBefore(inserted, this.DOM);
            return this;
        }
        appendAfter(inserted) {
            var dom = this.next().DOM;
            if (dom) {
                (this.parent().DOM as HTMLElement).insertBefore(inserted, dom);
            } else {
                this.parent().append(dom);
            }
            return this;
        }
        append(dom) {
            if (dom instanceof Box) {
                dom = dom.DOM;
            } else if ((typeof dom).toLowerCase() === "string") {
                dom = new Box(dom.trim()).DOM;
            }
            this.DOM.appendChild(dom);
            return this;
        }
        replace(old, newd) {
            return this;
        }
        call(fn, args?) {
            fn.call(this, args);
            return this;
        }
        on(evn, fn, args?) {
            if (!evn) {
                return this;
            }
            try {
                this.DOM.addEventListener(evn, fn, false);
            } catch (e) {
                try {
                    this.DOM["attachEvent"]('on' + evn, fn);
                } catch (e) {
                    this['on' + evn] = fn;
                }
            }
            return this;
        }
        off(evn, fn) {
            if (!evn) {
                return this;
            }
            try {
                this.DOM.removeEventListener(evn, fn, false);
            } catch (e) {
                try {
                    this.DOM["detachEvent"]('on' + evn, fn);
                } catch (e) {
                    delete this['on' + evn];
                }
            }
            return this;
        }
        bind(evn, fn, args?) { return this.on(evn, fn, args); }
        unbind(evn, fn) { return this.off(evn, fn); }
        once(evn, fn: Function) {
            var me = this;
            function temp() {
                fn.apply(this, [].slice.call(arguments));
                me.off(evn, temp);
            }
            this.on(evn, temp);
            return this;
        }
        emit(evn, args?) {
            if (!evn) {
                return this;
            }
            if (this.DOM.dispatchEvent && document.createEvent) {
                var e = document.createEvent("MouseEvents");
                e.initEvent(evn, true, true);
                this.DOM.dispatchEvent(e);
            } else if (document.all) {
                this.DOM[evn]();
            }
            return this;
        }
        removeClass(cls) {
            var r = false, j = 0, value, clses = cls.split(/\s+/), i = 0, n = clses.length;
            if (value = this.DOM.classList) {
                while (i < n) {
                    if (value.contains(clses[i])) {
                        value.remove(clses[i]);
                    }
                    i++;
                }
            } else {
                value = this.DOM.getAttribute("class");
                while (i < n) {
                    value = value.replace(new RegExp("(?:^|\\s+)" + helper.requote(clses[i]) + "(?:\\s+|$)", "g"), "");
                    i++;
                }
                this.DOM.setAttribute("class", value);
            }
            return this;
        }
        hasClass(cls) {
            var r = true, j = 0, value, clses = cls.split(/\s+/), i = 0, n = clses.length;
            if (value = this.DOM.classList) {
                while (i < n) {
                    if (!value.contains(clses[i])) {
                        r = false;
                    }
                }
            } else {
                value = this.DOM.getAttribute("class");
                while (i < n) {
                    if (!new RegExp("(?:^|\\s+)" + helper.requote(clses[i]) + "(?:\\s+|$)", "g").test(value)) {
                        r = false;
                        break;
                    }
                    i++;
                }
                this.DOM.setAttribute("class", value);
            }
            return r;
        }
        trigger(evn) { return this.emit(evn); }
        static drag = function (moveDom, bindDom, opacity?) {
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
                    var d = document; if (!a) a = window.event;
                    var area = o.area();
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
                        var area = o.area();

                        var tx = a.pageX - x + area.left, ty = a.pageY - y + area.top;
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
                        new Box(d).off("mousemove", move).off("mouseup", up);
                    }
                    new Box(d).on("mousemove", move).on("mouseup", up);
                });
            }
            if (bindDom) {
                listen();
            }
        }
        static parseHtml = function (tag): any {
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
                        } else {
                            var html = box.html();
                            window["eval"](html || "");
                        }
                    });
                }
                return boxes;
            }
            return new Box(obj.children[0]);
        };
        select(exp) {
            var b = new Box(this.DOM);
            return b.find(exp);
        }
        selects(exp): Boxes {
            var b = new Boxes(this.DOM);
            return b.find(exp);
        }
        static select(exp) {
            var b = new Box(window.document);
            return b.find(exp);
        }
        find(exp) {
            //this.selected = [];
            if (!exp) { return this; }
            if (typeof exp === "string") {
                exp = exp.trim();
                if (/^</.test(exp)) {
                    this.DOM = Box.parseHtml(exp).DOM;
                    return this;
                } else if (/create:/i.test(exp)) {
                    this.DOM = Box.parseHtml(exp.substr(7)).DOM;
                    return this;
                } else {
                    this.DOM = this.DOM.querySelector(exp.toString()) as HTMLElement;
                    return this;
                }
            } else if (exp instanceof Box) {
                this.DOM = exp.DOM;
                return this;
            } else if (exp.selected && exp.selected.length) {
                this.DOM = exp.selected[0];
                return this;
            } else if (exp.nodeType) {
                this.DOM = exp;
                return this;
            }
            return this;
        }
        css(css: any) {
            if ((typeof css).toLowerCase() === "string" && !/:/g.test(css.toString())) {
                var cs = this.rule.get(css);
                return !cs ? "" : cs.value;//.get(css).value;
            }
            else {
                var cssrule = style.StyleRule.parse(css);
                cssrule.apply(this.DOM);
                //this.rule.apply(cssrule);
                return this;
            }
        }
        area() {
            return this.DOM.getBoundingClientRect();
        }
        width() {
            return this.area().width || 0;
        }
        height() {
            return this.area().height || 0;
        }
        top() {
            return this.area().top || 0;
        }
        left() {
            return this.area().left || 0;
        }
        right() {
            return this.area().right || 0;
        }
        bottom() {
            return this.area().bottom || 0;
        }
        parent() {
            return new Box(this.DOM.parentElement);
        }
        attr(attr, value?: string): any {
            if (value || value === "") {
                this.DOM.setAttribute(attr, value);
                return this;
            }

            return this.DOM.getAttribute(attr);
        }
        removeAttr(attr) {
            this.DOM.removeAttribute(attr);
            return this;
        }
        checked() {
            var val = "";
            return (val = this.attr("checked")) === "checked" ? true : (val === "true" ? true : false);
        }
        remove() {
            this.DOM.parentNode ? this.DOM.parentNode.removeChild(this.DOM) : null;
            return this;
        }
        value(value?) {
            return this.prop("value", value);
        }
        html(value?) {
            return this.prop("innerHTML", value);
        }
        outerHtml() {
            return this.prop("outerHTML");
        }
        prop(name, value?) {
            if (value==null || value==undefined) {
                return this.DOM[name];
            }
            this.DOM[name] = value;
            return this;
        }
        class(cls) {
            if (!cls) {
                return this;
            }
            var r = false, j = 0, value;
            if ((value = this.DOM.classList) && value.add) {
                value.add(cls);
            } else {
                value = this.DOM.getAttribute("class");
                this.DOM.setAttribute("class", value + " " + cls);
            }
            return this;
        }
    }

    export class Boxes {
        selected: any[]
        context: HTMLElement
        constructor(exp?) {
            if (exp instanceof Box) {
                this.selected = [exp.DOM]
            }
            else if (exp instanceof HTMLElement) {
                this.selected = [exp]
            }
            else if (exp instanceof String) {
                this.select(exp);
            }
        }
        static children = function (d) {
            var rs = [];
            var cs = d.children;//window.document.children
            for (var i = 0; i < cs.length; i++) {
                rs.push(cs[i]);
            }
            return rs;
        };
        first() {
            return this.at(0);
        };
        lt(pos) {
            this.selected = this.selected.slice(0, pos);
            return this;
        };
        gt(pos) {
            this.selected = this.selected.slice(pos);
            return this;
        };
        odd() {
            var r = [];
            this.selected.forEach(function (d, i) {
                if (i % 2 !== 0) {
                    r.push(d);
                }
            });
            this.selected = r;
            return this;
        };
        eve() {
            var r = [];
            this.selected.forEach(function (d, i) {
                if (i % 2 === 0) {
                    r.push(d);
                }
            });
            this.selected = r;
            return this;
        };
        last() {
            return this.at(this.selected.length - 1);
        };
        at(pos) {
            return new Box(this.selected[pos]);
        };
        eq(eq) {
            if (typeof eq === "number") {
                return this.at(eq);
            } else if (typeof eq === "string") {
                return this.filter(eq);
            } else {
                return eq.call(this, null);
            }
        };
        node(pos?) {
            if (pos != null) {
                return this.selected[pos];
            } else {
                if (this.selected.length == 1) {
                    return this.selected[0];
                }
                return this.selected;
            }
        }
        attr(name, value?) {
            if (arguments.length < 2) {
                if (typeof name === "string") {
                    var node = this.node();
                    name = qualify(name);
                    if (!("getAttribute" in node)) { return ""; }
                    return name.local ? node["getAttributeNS"](name.space, name.local) : node["getAttribute"](name);
                }
                for (value in name)
                    this.each(attr(value, name[value]));
                return this;
            }
            return this.each(attr(name, value));
        }
        filter(exp) {
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
        appendTo(name) {
            var me = this;
            if (name instanceof Boxes) {
                name.each(function () {
                    var self = this;
                    me.each(function () {
                        self.appendChild(this);
                    });
                });
                return this;
            } else {
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
        static select = function (exp, context?) {
            return new Boxes().select(exp, context);
        }
        empty() {
            this.html("");
            return this;
        };
        has(node) {
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
        each(fn, args?) {
            this.selected.forEach(function (val, index, arry) {
                fn.apply(val, [val, index, arry].concat(args));
            });
            return this;
        }
        eaching(fn, args?) {
            var r = [];
            this.selected.forEach(function (val, index, arry) {
                var rs = fn.apply(this, [val, index, arry].concat(args));
                if (rs) {
                    r.push(rs);
                }
            });
            return r;
        }
        call(fn, pos?, args?) {
            if (pos != null && pos > 0) {
                return fn.call(this.at(pos), args);
            } else {
                return this.each(fn, args);
            }
        }
        find(exp): any {
            if (typeof exp === "string") {
                exp = exp.trim();
                if (/^</.test(exp)) {
                    return Boxes.select(Box.parseHtml(exp));
                } else {
                    return new Boxes().queriesSelection(exp, this);
                }
            } else if (exp instanceof Boxes) {
                return new Boxes().select(exp);
            } else if (exp.nodeType) {
                return new Boxes().select(exp);
            }
        }
        on(evn, fn, args?) {
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
        }
        off(evn, fn) {
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
        }
        text(txt?, pos?): any {
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
        }
        value(val?, pos?) {
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
        }
        html(html?, pos?) {
            if (html != null) {
                this.call(function (d) {
                    this.innerHTML = html;
                }, pos);
                return this;
            }
            return this.calling(function (d) {
                return this.node(0).innerHTML;
            }, this.selected.length > 1 ? pos : 0);
        }
        prop(name, value?) {
            if (value==null || value==undefined) {
                var node = this.node();
                return node[name];
            }
            node[name] = value;
            return this;
        }
        render(dom) {
            if (dom instanceof Element) {
                new Boxes(dom).html(this.html());
            } else if (!dom) {
                document.writeln(this.html());
            } else {
                Box.select(dom).append(this);
            }
            return this;
        }
        remove() {
            this.each(function () {
                this.parentNode ? this.parentNode.removeChild(this) : null;
            });
            return this;
        }
        class(cls) {
            return this.attr("class", cls);
        }
        bind(evn, fn, args?) { return this.on(evn, fn, args); }
        unbind(evn, fn) { return this.off(evn, fn); }
        calling(fn, pos?, args?) {
            if (this.selected.length == 1) {
                pos = 0;
            }
            if (pos != null && pos > -1) {
                return fn.call(this.at(pos), args);
            } else {
                return this.eaching(fn, args);
            }
        }
        emit(evn, ...args) {
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
        }
        removeClass(cls) {
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
                        value = value.replace(new RegExp("(?:^|\\s+)" + helper.requote(clses[i]) + "(?:\\s+|$)", "g"), "");
                        i++;
                    }
                    this.setAttribute("class", value);
                }
                j++;
            });
            return this;
        }
        hasClass(cls) {
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
                        if (!new RegExp("(?:^|\\s+)" + helper.requote(clses[i]) + "(?:\\s+|$)", "g").test(value)) {
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
        }
        trigger(evn) { return this.emit(evn); }
        private parseNodeList(col?) {
            if (!this.selected) { this.selected = []; }
            for (var i = 0; i < col.length; i++) {
                this.selected.push(col[i]);
            }
        }
        private queriesSelection(exp, context?): any {
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
        }
        select(exp: any, context?): any {
            this.selected = [];
            if (!exp) { return this; }
            if (typeof exp === "string") {
                if (/^</.test(exp)) {
                    return this.select(Box.parseHtml(exp));
                } else if (/create:/i.test(exp)) {
                    return this.select(Box.parseHtml(exp.substr(7)));
                } else {
                    return this.queriesSelection(exp, context);
                }
            } else if (exp instanceof Boxes) {
                this.selected = exp.selected;
                return this;
            } else if (exp.selected && exp.selected.length) {
                this.selected = exp.selected;
                return this;
            } else if (exp.nodeType) {
                this.selected.push(exp);
                return this;
            } else if (helper.isArray(exp)) {
                var self = this;
                exp.forEach(function (x) { x.nodeType ? self.selected.push(x) : ""; });
            }
            return this;
        }
        private selectionMatchs(n?, s?): any {
            var obj = document.createElement("div"), selectMatcher = obj["matches"] /*|| obj[temp_vendorSymbolFunc(obj, "matchesSelector")]*/, selectMatches = function (n, s) {
                return selectMatcher.call(n, s);
            };
            return selectMatches(n, s);
        }
        children(exp, pos?) {
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
        parent(exp, pos?) {
            var rs = [];
            this.call(function (d) {
                rs.push(d.parentNode);
            }, pos);
            this.selected = rs;
            return exp ? this.filter(exp) : this;
        };
        static next = function (d) {
            var r = d, rs = [];
            while ((r = r.nextElementSibling) != null) {
                rs.push(r);
            }
            return rs;
        };
        static prev = function (d) {
            var r = d, rs = [];
            while ((r = r.previousElementSibling) != null) {
                rs.push(r);
            }
            return rs;
        };
        next(exp, pos) {
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
        prev(exp, pos) {
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
    }
    class ValidateRule {
        RegExp: RegExp
        warning: string
        validate(val, fn?: Function) {
            if (!this.RegExp.test(val)) {
                fn ? fn(true, this.warning, val) : "";
                return true;
            }
            fn ? fn(false, this.warning, val) : "";
            return false;
        }
        constructor(rule: any) {
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
    }
    interface IBehavior {
        binded: Box
        apply(message?, val?);
        unapply();
    }
    export class RedBorderBehavior implements IBehavior {
        binded: Box
        static Color = "";
        borderColor = "";
        apply(message?: string, val?) {
            this.borderColor = this.binded.css("border-color") as string;
            (this.binded.css({ "border-color": RedBorderBehavior.Color || "red" }) as Box).attr("title", message);
        }
        unapply() {
            (this.binded.css({ "border-color": this.borderColor }) as Box).removeAttr("title");
        }
    }
    export class Behavior {
        static behaviors = {
            redborder: RedBorderBehavior.prototype.constructor
        }
    }
    export class PopupBehavior implements IBehavior {
        binded: Box
        apply(message?, val?) {
            var area = this.binded.area();

        }
        unapply() {

        }
    }
    export class Validate {
        rules: ValidateRule[]
        binded: Box
        behavior: IBehavior
        mode = "auto"
        message = ""
        constructor(dom?) {
            this.rules = [];
            if (dom) {
                this.binded = new Box(dom);
            }
        }
        addRules(rule) {
            if (typeof rule === "string") {
                rule = JSON.parse(rule);
            }
            this.rules.push(new ValidateRule(rule));
        }
        private __validate(val) {
            for (var r in this.rules) {
                if (this.rules[r].validate(val)) {
                    this.message = this.rules[r].warning;
                    return true;
                }
            }
            this.message = "";
            return false;
        }
        bind(dom?) {
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
        }
        register(behavior, unbehavior) {
            this.behavior = behavior || this.behavior;
        }
        validate() {
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
        }
        static validate() {

        }
        result() {
            return /true/ig.test(this.binded.attr("validate-result"));
        }
    }
    export class Slideshow extends Box {
        timerid: number
        count = 0;
        private _children: any
        current: Box
        currentIndex = -1;
        loop = false;
        wrapper: Box
        _width = 0
        _height = 0
        interval = 5000;
        constructor(box) {
            super(box);
            emitter.Emitter.mixin(this);
            if (this.hasChild()) {
                this._children = this.children();
                if (this._children.selected.length == 1 && this._children[0].hasClass("slide-options")) {
                }
                this.html("");
                this.count = this._children.selected.length;
            }
            this.wrapper = new Box("create:div");
            //transition: all 1000ms ease; transform: translate3d(-5760px, 0px, 0px);
            this.wrapper.rule.css({ width: this.width() + "px", height: "100%", "margin": "0px", padding: "0px", "transition": "all 1000ms ease", transform: "translate3d(0px, 0px, 0px)" });
            this.wrapper.appendTo(this);
            for (var i = 0; i < this.count; i++) {
                this.wrapper.append(this._children);
            }
            this.goto(0);
        }
        get(url, data) {
            new Promise(function () {

            });
            return this;
        }
        indexof(witch) {

            for (var i = 0; i < this.count; i++) {
                if (this.equalsNode(this._children[i])) {
                    return i;
                }
            }
            return -1;
        }
        goto(index) {
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
        }
        next() {
            this.currentIndex++;
            if (this.currentIndex >= this.count && this.loop) {
                this.currentIndex = 0;
            }
            this.goto(this.currentIndex);
            return this;
        }
        prev() {
            this.currentIndex--;
            if (this.currentIndex < 0 && this.loop) {
                this.currentIndex = this.count + 1;
            }
            this.goto(this.currentIndex);
            return this;
        }
        rerender() {

        }
        start() {
            this.timerid = window.setInterval(function () {
                this.next();
            }, this.interval);
        }

        clear() {
            this.currentIndex = 0;
            this.current = null;
            window.clearInterval(this.timerid);
        }
        stop() {
            window.clearInterval(this.timerid);
        }
    }
    export interface ITemplate {
        _html: string,
        data: any
        bind(data?);
        html(html?);
        apply();
    }
    export class Layer extends Box {
        constructor(dom?) {
            super(Box.parseHtml("div"));
        }
        zindex = 10;
        mask: Layer
        initialize(conf?) {
            conf = conf || {};
            var me = this;
            this.zindex = conf.zindex || 10;
            this.rule.apply({
                top: "0px", left: "0px", opacity: "0", position: conf.position || "absolute;", "z-index": this.zindex, width: conf.width || "40%"
                , height: conf.height || "300px"
            });
            this.class(conf.mode == "hover" ? "zonic-section-layer-hover" : (conf.mode == "mask" ? "zonic-section-mask" : "zonic-section-layer"));
            if (conf.mode == "mask") {
                this.rule.apply({ left: "0px", right: "0px", top: "0px", bottom: "0px", width: window.screen.availWidth + "px", height: window.screen.height + "px", opacity: 0.4, position: "fixed" });
            }
            else if (conf.mode == "masking") {
                //this.rule.apply({ left: "0px", right: "0px", top: "0px", bottom: "0px", width: window.screen.availWidth + "px", height: window.screen.height + "px", opacity: 0.4, position: "fixed" });
                this.mask = new Layer();
                this.mask.initialize({ mode: "mask", zindex: this.zindex - 1 });
                //this.append(this.mask);
            }
            if ("D3" in conf) {
                this.attr("layer-box-3D", "D3");
            }
            if (conf.center) {
                window.setTimeout(function () {
                    var r = me.area(), width = r.width, height = r.height;
                    me.rule.apply({
                        opacity: conf.mode == "mask" ? 0.45 : 1,
                        top: conf.mode == "mask" ? "0px" : (((conf.top || window.screen.availHeight / 2) - height / 1.8) + "px"), left: conf.mode == "mask" ? "0px" : (((conf.left || window.screen.availWidth / 2) - width / 2) + "px")
                    });
                }, 0);
            }
            else { window.setTimeout(function () { me.rule.apply({ opacity: conf.mode == "mask" ? 0.45 : 1 }); }, 0); }
            if (!helper.hasProp(conf, "dragable") || conf.dragable)
                Box.drag(this, this, 0.3);
            return this;
        }
        layout(css) {
            this.rule.apply(css);
            return this;
        }

        remove() {
            super.remove();
            if (this.mask && this.mask.remove) { this.mask.remove(); }
            return this;
        }
        render(dom) {
            dom = dom || new Box("body");
            this.appendTo(dom);
            this.mask.appendTo(dom);
            return this;
        }
    }
    export class Popup extends Layer {
        constructor() {
            super();
        }
        zindex = 10
        _remove: any
        initialize(conf?) {
            conf = conf || {};
            conf.mode = conf.mode ? conf.mode : "hover";
            conf.center = true;
            conf.width = conf.width || "210px";
            conf.height = conf.height || "60px";
            conf.position = "absolute";
            conf.background = conf.bgcolor || "white";
            conf.center = true;
            this.zindex = conf.zindex;
            super.initialize(conf);
            return this;
        }
        close() {
            this.emit("closed", []);
            return this.remove();
        }
        static popups = [];
        show2Top = true;
        speed = 300;
        render(html, dom?) {
            if (html) {
                if (helper.isFunction(html)) {
                    html.call(this, dom);
                } else {
                    this.append(html);
                }
            }
            var me = this;
            super.render(new Box("body", Document(this.show2Top)));
            return this;
        }
        show(speed?) {

            return this;
        }
    }
    export class Select extends Box {
        constructor() {
            super(null);
            this.DOM = Box.parseHtml("div");
            this.txt = "";
        }
        label: any;
        _height: number;
        _width: number;
        placeholder: any;
        type = "text";
        hint: any;
        hintColor: any;
        labelColor: any;
        val: any;
        txt: any;
        horizontal: string;
        selected = [];
        vertical: string;
        initialize(conf?) {
            conf = conf || {};
            conf.labelColor = conf.labelColor || "rgba(0, 0, 0, 0.298039)";
            this.height = conf.height || 72;
            this.width = conf.width || 256;
            this.label = conf.label || "";
            this.hintColor = conf.hintColor;
            this.hint = conf.hint;
            this.type = conf.type;
            this.placeholder = conf.placeholder;
            if (!conf.label && conf.placeholder) { this.label = conf.placeholder; }
            this.list = new List("ul");
            return this;
        }
        list: List;
        layerHeight: number;
        layer: Layer;
        render(dom) {
            var u = this;
            var height = this._height;
            u.rule.apply("transition:height 200ms cubic-bezier(0.23, 1, 0.32, 1); width: " + (u.width || 256) + "px; height: " + (u._height || 256) + "px; line-height: " + (u._height * 0.33) + "px; font-family: Roboto,sans-serif; font-size: 16px; display: inline-block; position: relative; cursor: auto; background-color: transparent;")
            u.append(
                (function () {
                    var returnBox = new Box((Box.parseHtml("label")).rule.apply("transition:450ms cubic-bezier(0.23, 1, 0.32, 1); top: " + (u._height * 0.605) + "px; color: rgba(0, 0, 0, 0.3); line-height: " + (u._height * 0.405) + "px; position: absolute; z-index: 1; pointer-events: none; transform: scale(1) translate(0px, 0px); transform-origin: left top 0px;").DOM).class("label").html(u.label || "&nbsp;");
                    returnBox.append(
                        new Box(Box.parseHtml("div").rule.apply('font:inherit; padding: 0px; outline: invert; border: medium; transition:450ms cubic-bezier(0.23, 1, 0.32, 1); border-image: none; width: 100%; height: 100%; color: rgba(0, 0, 0, 0.87); margin-top: ' + (u._height * 0.2) + 'px; display: block; position: relative; cursor: inherit; box-sizing: border-box; font-size-adjust: none; font-stretch: inherit; -webkit-tap-highlight-color: rgba(0, 0, 0, 0); background-color: rgba(0, 0, 0, 0)').DOM)
                            .call(function () {
                                new Box(Box.parseHtml("div")).css({ "padding-left": "5px", cursor: "pointer", height: "100%", position: "relative", width: "100%" })
                                ["append"](Box.parseHtml("div").rule.apply("box-sizing:border-box;content: \" \";display:table;").DOM)
                                    .append(new Box(Box.parseHtml("div").rule.apply("top: " + (height * 0.21) + "px; color: rgba(0, 0, 0, 0.87); line-height: " + (height * 0.8) + "px; overflow: hidden; padding-right: 56px; padding-left: 0px; white-space: nowrap; position: relative;text-overflow: ellipsis; opacity: 1;").DOM).class("zonic-select-area").html(u.txt || "&nbsp;"))
                                    .append(Box.parseHtml("button").attr({ type: "button" }).css({ "border": (height * 0.117) + "px", " box-sizing": "border-box", "display": "inline-block", "font-family": " Roboto, sans-serif", " -webkit-tap-highlight-color": " rgba(0, 0, 0, 0)", " cursor": " pointer", " text-decoration": " none", " margin": " 0px", "padding": (height * 0.1067) + "px", "outline": " none", "font-size": " 0px", "font-weight": " inherit", "position": "absolute", "z-index": " 1", "overflow": " visible", "transition": " all 450ms cubic-bezier(0.23, 1, 0.32, 1) 0ms", " width": (height * 0.76067) + "px", " height": (height * 0.76067) + "px", " fill": " rgb(224, 224, 224)", " right": " 0px", " top": (height * 0.30) + "px", " background": " none" }).html('<div><svg viewBox="0 0 ' + (height * 0.33) + ' ' + (height * 0.33) + '" style="display: inline-block; color: rgba(0, 0, 0, 0.870588); fill: inherit; height: ' + (height * 0.33) + 'px; width: ' + (height * 0.33) + 'px; user-select: none; transition: all 450ms cubic-bezier(0.23, 1, 0.32, 1) 0ms;"><path d="M7 10l5 5 5-5z"></path></svg></div>'))
                                    .append(Box.parseHtml("div").css({ "border-top": "none", "bottom": "1px", left: "0px", margin: "-1px 24px", right: "0px", position: "absolute" }))
                                    .append(Box.parseHtml("div").rule.apply("box-sizing:border-box;content: \" \";display:table;clear: both;").DOM)
                                    .appendTo(this);
                            }).bind("click", function () {
                                if (u.layer) { return; }
                                var rect = u.area();
                                var top = u.selected[0].offsetTop;
                                u.layer = new Layer();
                                u.layer.initialize({ D3: true, position: "fixed", dragable: false, width: "0px", height: "0px" }).class("zonic-select").
                                    rule.apply("transition:height 1s cubic-bezier(0.23, 1, 0.32, 1) 0ms;").apply("width: " + (u.width || rect.width) + "px;height:" + (u.layerHeight || 160) + "px;border-radius:2px; left: " + (rect.left + (u.horizontal == "right" ? rect.width : (u.horizontal == "middle" ? rect.width / 2 : 0))) + "px; top: " + (rect.top + u._height * 0.2 + (u.vertical == "top" ? 0 : (u.vertical == "center" ? rect.height / 2 : rect.height))) + "px;");
                                u.find("label.label").css({ transform: "scale(0.75) translate(0px, " + (-u.height * 0.10) + "px)", color: u.labelColor || "rgba(0, 0, 0, 0.298039)" });
                                //u.find("label.")
                                u.list = new List("ul").on("selectchanged", function (e, data, item) {
                                    if (!helper.isObject(data)) {
                                        u.selectedValue = u.selectedText = data; u.find(".zonic-select-area").html(data).attr({ "zonic-selected-value": data });
                                    } else {
                                        u.selectedValue = data.value; u.selectedText = data.text; u.find(".zonic-select-area").html(data.text).attr({ "zonic-selected-value": data.value });
                                    }
                                    if (!u.selectedValue) { //如果没有选择值则 复原
                                        u.find("label.label").css({ transform: "scale(1) translate(0px, 0px)", color: u.labelColor });
                                    }
                                    u.emit("selectchanged", helper.slice(arguments));
                                });
                                u.list.additem(u.items);
                                u.list.appendTo(u.layer);
                                u.layer.render("body");
                            })
                            .append((function () {
                                return new Box(Box.parseHtml("div")).html('<hr style="border-width: medium medium 1px; border-style: none none solid; border-color: rgb(224, 224, 224); margin: 0px; width: 100%; bottom:' + (u._height * 0.06) + 'px; position: absolute; box-sizing: content-box;">')
                                    //.append(new element(element.parseHtml("hr")).css({ "border-width":"medium medium 1px","border-style":"none none solid","border-color":"rgb(224, 224, 224)","margin":"0px","width":"100%","bottom":(height*0.06)+"px","position":"absolute","box-sizing":"content-box" }))
                                    ;
                            })())
                    )
                    return returnBox;
                })()
            );
            Box.select("body").on("click", function (e) {
                var e = e || window.event;
                if (u.DOM.isEqualNode(e.target)) {
                    return;
                }
                u.layer.remove();
                u.layer = null;
            });
            if (dom) {
                this.render(dom);
            }
            return this;
        }
        selectedValue = "";
        selectedText = "";
        items = []
        additem() {
            //this.list.additem.apply(this.list, helper.slice(arguments))
            this.items = this.items.concat(helper.slice(arguments));
            return this;
        }
    }
    export class List extends Box {
        constructor(dom) {
            super(null);
            this.DOM = Box.parseHtml(dom || "ul");
            this.class("zonic-list");
            emitter.Emitter.mixin(this);
        }
        public multi = false;
        additem(data) {
            var me = this;
            if (helper.isArray(data)) { data.forEach(function (i) { me.additem(i); }); return me; }
            return (function () {
                var d = data;
                var item = new Box(Box.parseHtml("li"));
                item["uiParent"] = this;
                item.on("click", function (e) {
                    if (item.attr("disabled")) { return; }
                    if (!me.multi) {
                        me.find(".zonic-list-item[selected='true']").attr({ "selected": "false" });
                    }
                    if (item.attr("selected") == "true") {
                        item.attr({ "selected": "false" });
                    } else {
                        item.attr({ "selected": "true" });
                    }
                    me.emit("selectchanged", [e, data, item]);
                }).bind("mouseenter", function (e) {
                    me.emit("hoveritem", [e, data, item]);
                }).class("zonic-list-item").attr({ "selected": "false", }).appendTo(me).if(typeof data !== "object", function () {
                    return item.html(data);
                }).if(data.text || data.html, function () {
                    return item.html(data.text || data.html);
                }).if(data.value, function () {
                    return item.attr("value", data.value);
                }).css(me.itemCss || {});
                return item;
            })();
        }
        binddata(conf, data) {
            if (arguments.length = 1) {
                data = conf.data;
                conf = conf.conf;
            }
            return this;
        }
        initialize(conf?) {
            if (conf) {
                this.itemCss = conf.itemCss || {};
                this.dragable = helper.hasProp(conf, "dragable") ? conf.dragable : false;
            }
            return this;
        }
        render(dom) {
            if (dom) {
                this.appendTo(new Box(dom));
            }
        }
        itemCss = null;
        dragable = false;
    }

    export class Text extends Box {

    }
    export class Item extends Box {

    }
    export class TreeItem extends Item {

    }
    export class Tree extends Box {

    }

    export class Checkbox extends Box {

    }
    export class CheckboxGroup extends Box {

    }

}
export default Box;