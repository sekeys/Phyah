import {zonic as helper} from './Zonic.base'
import {zonic as emitter} from './Emitter'
import box from './Box'

 export default class xhr {
    url: string
    mimeType: string
    contentType = "text/html"
    type = ""
    data = null
    headers = {}
    method = "get"
    promise: pmodule.Promise<Object>;
    status = null
    constructor(url, mimeType, response, callback) {
        this.url = "";
        this.contentType = "application/json";
        this.type = "";
        this.data = "";
        this.headers = {};
        this.method = "get";

        emitter.Emitter.mixin(this);
        this.url = url;
        var me = this;
        var d = new pmodule.Deferred<Object>();
        this.promise = new pmodule.Promise(d);
        function xhrHasResponse(request) {
            var type = request.responseType;
            return type && type !== "text" ? request.response : request.responseText;
        }
        function respond() {
            var status = me.request.status, result;
            me.promise.deferred.done(v=>{
                console.log("request the "+me.url+" with status["+status+"]");
            });
            me["emit"]("load", status,me.request);
            if (!status && xhrHasResponse(me.request) || status >= 200 && status < 300 || status === 304) {
                try {
                    result = /xml/.test(me.mimeType) ? me.request : /json/.test(me.mimeType) ? me.request : me.request.responseText;
                    me["emit"]("success", result, me.request);
                    //d.resolve(param);
                    me.promise.deferred.resolve(result);
                } catch (e) {
                    me["emit"]("error", e);
                    me.promise.deferred.reject(new Error('the processor has some error exist here:'+e));
                    console.error(e);
                    return;
                }
            } else {
                me["emit"]("error",null, me.request);
                me.promise.deferred.reject(new Error('getJSON: `' + url + '` failed with status: [' + me.status + ']'));
            }
        }
        this.request = new XMLHttpRequest();
        if (window["XDomainRequest"] && !("withCredentials" in this.request) && /^(http(s)?:)?\/\//.test(this.url))
            this.request = new window["XDomainRequest"]();
        this.request.onreadystatechange = function () {
            //me._dispatch.completed.call(me, me.request);
            //me.emit("completed", me.request);
            me.request.readyState > 3 && respond();
        };
        if (mimeType) {
            this.mimeType = mimeType;
        }
        if (response instanceof Function) {
            //this._dispatch.on("success", response);
            me["emit"]("success", response);
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
    send =function(method, data, callback) {
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
    abort  () {
        this.request.abort();
        this.promise.deferred.reject(new Error("custom abort request"));
        return this;
    };
    get (d, callback) {
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
    post (d, callback) {
        return this.send.apply(this, ["post", d, callback]);
    };
}