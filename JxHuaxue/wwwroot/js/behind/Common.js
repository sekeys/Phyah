;
(function () {
    zonic.loadModule = function (href, data) {
        //zonic.loadHtml(href, data, function (d) {
        //    zonic("#module_area").html(d);
        //});
        zonic("#module_area").attr("src",href);
    }
    zonic("#main-menu li").on("click", function (e) {
        e = window.event || e;
        var zl = zonic(this);
        var ah = zl.find("a");
        var href = ah.attr("href");
        if (zl.hasClass("has-sub")) {
            zl.parent().find(".has-sub.expanded ul").hide();
            if (zl.hasClass("expanded")) {
                zl.removeClass("expanded").removeClass("active");
                zl.find("ul").hide();
            } else {
                zl.parent().find(".active").removeClass("expanded").removeClass("active");
                zl.addClass("expanded").addClass("active");
                zl.find("ul").show();
            }
        } else {
            zl.parent().find(".active").removeClass("expanded").removeClass("active");
            zl.addClass("active").addClass("expanded");
        }
        if (href) {
            zonic.loadModule(href, null);
        }

        if (e && e.preventDefault) {
            e.preventDefault();

        } else {
            e.returnValue = false;
        }
        if (e.stopPropagation) e.stopPropagation();
        if ("cancelBubble" in e) e.cancelBubble = true;
        return false;
    });
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
    zonic.deserialize = function (exp,data) {
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
})();
