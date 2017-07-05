define(["require", "exports"], function (require, exports) {
    "use strict";
    exports.__esModule = true;
    var zonic;
    (function (zonic) {
        var Color = (function () {
            function Color(val) {
                this.R = 0;
                this.B = 0;
                this.G = 0;
                this.O = 1;
                this.value(val);
            }
            Color.prototype.NumberToHex = function (number) {
                if (!number)
                    return;
                return number.toString(16);
            };
            Color.prototype.toHex = function (num) {
                var hex = this.NumberToHex(num);
                return hex.length <= 1 ? "0" + hex : hex;
            };
            Color.prototype.hexToNumber = function (number) {
                if (!number)
                    return;
                return parseInt(number, 16);
            };
            Color.prototype.hex = function (value) {
                if (arguments.length > 0)
                    return "#" + this.toHex(this.R) + this.toHex(this.G) + this.toHex(this.B);
                else if (!value) {
                    this.R = this.G = this.B = 0;
                    this.O = 1;
                    return this;
                }
                else {
                    if (/^#/.test(value)) {
                        value = value.substr(1);
                    }
                    this.R = this.hexToNumber(value.substr(0, 2));
                    this.G = this.hexToNumber(value.substr(3, 2));
                    this.B = this.hexToNumber(value.substr(5, 2));
                }
            };
            Color.prototype.rgb = function (value) {
                if (value) {
                    value = value.substr(4);
                    value = value.substr(0, value.length - 1);
                    var vals = value.split(',');
                    this.R = parseInt(vals[0]);
                    this.G = parseInt(vals[1]);
                    this.B = parseInt(vals[2]);
                    if (vals.length > 3)
                        this.O = parseInt(vals[3]);
                }
                else {
                    return "rgb(" + this.R + "," + this.G + "," + this.B + "," + this.O + ")";
                }
            };
            Color.prototype.value = function (value) {
                if (!value)
                    return this.rgb();
                return /^#/.test(value) ? this.hex(value) : this.rgb(value);
            };
            return Color;
        }());
        zonic.Color = Color;
    })(zonic = exports.zonic || (exports.zonic = {}));
});
