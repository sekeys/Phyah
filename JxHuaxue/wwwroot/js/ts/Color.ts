
export module zonic {
    export interface IColor {
        R: number,
        B: number,
        G: number,
        O: number,
        hex(value?);
        rgb(value?);
        value(value?);
    }
    export class Color implements IColor {
        R = 0;
        B = 0;
        G = 0;
        O = 1;
        private NumberToHex(number) {
            if (!number) return;
            return number.toString(16);
        }
        private toHex(num) {
            var hex =this.NumberToHex(num);
            return hex.length <= 1 ? "0" + hex : hex;
        }
        private hexToNumber(number) {
            if (!number) return;
            return parseInt(number, 16);
        }
        hex(value?: string):any {
            if (arguments.length > 0)
                return "#" + this.toHex(this.R) + this.toHex(this.G) + this.toHex(this.B);
            else if (!value) {
                this.R = this.G = this.B = 0; this.O = 1;
                return this;
            } else {
                if (/^#/.test(value)) {
                    value = value.substr(1);
                }
                this.R = this.hexToNumber(value.substr(0, 2));
                this.G = this.hexToNumber(value.substr(3, 2));
                this.B = this.hexToNumber(value.substr(5, 2));
            }
        }
        rgb(value?) {
            if (value) {
                value = value.substr(4);
                value = value.substr(0, value.length - 1);
                var vals = value.split(',');
                this.R = parseInt(vals[0]);
                this.G = parseInt(vals[1]);
                this.B = parseInt(vals[2]);
                if (vals.length > 3)
                    this.O = parseInt(vals[3]);
            } else {
                return "rgb(" + this.R + "," + this.G + "," + this.B + "," + this.O + ")"
            }
        }
        value(value?) {
            if (!value) return this.rgb();
            return /^#/.test(value) ? this.hex(value) : this.rgb(value);
        }
        constructor(val:string){
            this.value(val);
        }
    }
}
