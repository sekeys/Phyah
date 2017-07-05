define(["require", "exports"], function (require, exports) {
    "use strict";
    exports.__esModule = true;
    var zonic;
    (function (zonic) {
        var Emitter = (function () {
            function Emitter(obj) {
                this.hasListeners = function (event) {
                    return !!this.listeners(event).length;
                };
                this.emittermixin(obj);
            }
            Emitter.prototype.emittermixin = function (obj) {
                for (var key in Emitter.prototype) {
                    obj[key] = Emitter.prototype[key];
                }
                return obj;
            };
            Emitter.mixin = function (obj) {
                for (var key in Emitter.prototype) {
                    obj[key] = Emitter.prototype[key];
                }
                return obj;
            };
            Emitter.prototype.on = function (eventName, func) {
                this.callbacks = this.callbacks || {};
                (this.callbacks['$' + event] = this.callbacks['$' + event] || [])
                    .push(func);
                return this;
            };
            Emitter.prototype.addEventListener = function (eventName, func) {
                return this.on(eventName, func);
            };
            Emitter.prototype.once = function (eventName, func) {
                function on() {
                    this.off(event, on);
                    func.apply(this, arguments);
                }
                on["fn"] = func;
                this.on(event, on);
                return this;
            };
            Emitter.prototype.off = function (event, fn) {
                this.callbacks = this.callbacks || {};
                // all
                if (0 == arguments.length) {
                    this.callbacks = {};
                    return this;
                }
                // specific event
                var callbacks = this.callbacks['$' + event];
                if (!callbacks)
                    return this;
                // remove all handlers
                if (1 == arguments.length) {
                    delete this.callbacks['$' + event];
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
            Emitter.prototype.removeEventListener = function (event, fn) { return this.off(event, fn); };
            Emitter.prototype.removeAllListeners = function (event, fn) { return this.off(event, fn); };
            Emitter.prototype.removeListener = function (event, fn) { return this.off(event, fn); };
            Emitter.prototype.emit = function (event) {
                this.callbacks = this.callbacks || {};
                var args = [].slice.call(arguments, 1), callbacks = this.callbacks['$' + event];
                if (callbacks) {
                    callbacks = callbacks.slice(0);
                    for (var i = 0, len = callbacks.length; i < len; ++i) {
                        callbacks[i].apply(this, args);
                    }
                }
                return this;
            };
            Emitter.prototype.listeners = function (event) {
                this.callbacks = this.callbacks || {};
                return this.callbacks['$' + event] || [];
            };
            ;
            return Emitter;
        }());
        zonic.Emitter = Emitter;
    })(zonic = exports.zonic || (exports.zonic = {}));
});
