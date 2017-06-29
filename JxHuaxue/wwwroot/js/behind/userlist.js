
(function () {
    zonic.ajax("users", "get", null, function (d) {
        var z = zonic("#userlist tbody");
        var index = 0;
        window.JSON.parse(d).forEach(function (t) {
            zonic("<tr><td>" + (++index) + "</td><td>" + t.UserName + "</td><td>" + t.Nick + "</td><td><button class=\"btn btn-turquoise\" onclick='modify('" + t.Id6 + "')'>重置密码</button></td></tr>").appendTo(z);
        });

    });
    zonic.ui.layer({ dragable: false, center:true});
})();