document.createElement("nav");
var amazonmenu = {
    defaults: {
        animateduration: 100,
        showhidedelay: [200, 300],
        hidemenuonload: !0,
        hidemenuonclick: !0
    },
    setting: {},
    menuzindex: 1e3,
    touchenabled: !!("ontouchstart" in window) || !!("ontouchstart" in document.documentElement) || !!window.ontouchstart || !!window.Touch || !!window.onmsgesturechange || window.DocumentTouch && window.document instanceof window.DocumentTouch,
    showhide: function (e, n, i) {
        clearTimeout(e.data("showhidetimer")), e.data().showhidetimer = "show" == n ? setTimeout(function () {
            e.addClass("selected"), e.data("$submenu").data("fullyvisible", !0).css({
                zIndex: amazonmenu.menuzindex++
            }).fadeIn(i.animateduration, function () {
                $(this).data("fullyvisible", !0)
            })
        }, this.setting.showhidedelay[0]) : setTimeout(function () {
            e.removeClass("selected"), e.data("$submenu").stop(!0, !0).fadeOut(i.animateduration);
            var n = e.data("$submenu").find(".issub").css({
                display: "none"
            });
            n.length > 0 && n.data("$parentli").removeClass("selected")
        }, this.setting.showhidedelay[1])
    },
    setupmenu: function (e, n) {
        var i = e.children("ul:eq(0)");
        i.find("li>div, li>ul").each(function () {
            var e = $(this).parent("li"),
                i = $(this);
            e.addClass("hassub").data({
                $submenu: i,
                showhidetimer: null
            }).on("mouseenter click", function (e) {
                amazonmenu.showhide($(this), "show", n)
            }).on("mouseleave", function (e) {
                amazonmenu.showhide($(this), "hide", n)
            }).on("click", function (e) {
                e.stopPropagation()
            }).children("a").on("click", function (e) {
                e.preventDefault()
            }), i.addClass("issub").data({
                $parentli: e
            }).on("mouseleave" + (n.hidemenuonclick || amazonmenu.touchenabled ? " " + "click" : ""), function (e) {
                1 == $(this).data("fullyvisible") && amazonmenu.showhide($(this).data("$parentli"), "hide", n), "click" == e.type && e.stopPropagation(), $("li.hassub.selected").mouseleave(function () {
                    $("li.hassub.selected").css("background-color", "")
                })
            })
        }), i.on("click", function (e) {
            1 == $(this).data("fullyvisible") && amazonmenu.showhide($(this).children("li.hassub.selected"), "hide", n)
        });
        i.children("li.hassub").on("mouseleave", function () {
            amazonmenu.showhide($(this), "hide", n)
        })
    },
    init: function (e) {
        var n = $("#" + e.menuid);
        this.setting = $.extend({}, e, this.defaults), this.setting.animateduration = Math.max(50, this.setting.animateduration), this.setupmenu(n, this.setting)
    }
};