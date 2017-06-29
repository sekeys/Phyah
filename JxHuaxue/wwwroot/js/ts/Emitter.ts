export module zonic {
    export class Emitter {
        private callbacks: any;
        private emittermixin(obj) {
            for (var key in Emitter.prototype) {
                obj[key] = Emitter.prototype[key];
            }
            return obj;
        }
        constructor(obj: any) {
            this.emittermixin(obj);
        }
        static mixin(obj:any) {
            for (var key in Emitter.prototype) {
                obj[key] = Emitter.prototype[key];
            }
            return obj;
        }
        on(eventName, func) {
            this.callbacks = this.callbacks || {};
            (this.callbacks['$' + event] = this.callbacks['$' + event] || [])
                .push(func);
            return this;
        }
        addEventListener(eventName, func) {
            return this.on(eventName, func);
        }
        once(eventName, func) {
            function on() {
                this.off(event, on);
                func.apply(this, arguments);
            }
            on["fn"] = func;
            this.on(event, on);
            return this;
        }
        off(event, fn) {
            this.callbacks = this.callbacks || {};

            // all
            if (0 == arguments.length) {
                this.callbacks = {};
                return this;
            }

            // specific event
            var callbacks = this.callbacks['$' + event];
            if (!callbacks) return this;

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
        }
        removeEventListener(event, fn) { return this.off(event, fn); }
        removeAllListeners(event, fn) { return this.off(event, fn); }
        removeListener(event, fn) { return this.off(event, fn); }
        emit(event) {
            this.callbacks = this.callbacks || {};
            var args = [].slice.call(arguments, 1)
                , callbacks = this.callbacks['$' + event];

            if (callbacks) {
                callbacks = callbacks.slice(0);
                for (var i = 0, len = callbacks.length; i < len; ++i) {
                    callbacks[i].apply(this, args);
                }
            }
            return this;
        }
        listeners(event) {
            this.callbacks = this.callbacks || {};
            return this.callbacks['$' + event] || [];
        };
        hasListeners = function (event) {
            return !!this.listeners(event).length;
        };
    }
}