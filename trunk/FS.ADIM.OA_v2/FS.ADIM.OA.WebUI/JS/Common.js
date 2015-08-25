//判断ie
var isIE = (document.all) ? true : false;
function $(id) { return "string" == typeof id ? document.getElementById(id) : id; };

var Class = {
    create: function() {
        return function() {
            this.initialize.apply(this, arguments);
        }
    }
}
var CurrentStyle = function(element) {
    return element.currentStyle || document.defaultView.getComputedStyle(element, null);
}
function addEventHandler(oTarget, sEventType, fnHandler) {
    if (oTarget.addEventListener) {
        oTarget.addEventListener(sEventType, fnHandler, false);
    } else if (oTarget.attachEvent) {
        oTarget.attachEvent("on" + sEventType, fnHandler);
    } else {
        oTarget["on" + sEventType] = fnHandler;
    }
}
function removeEventHandler(oTarget, sEventType, fnHandler) {
    if (oTarget.removeEventListener) {
        oTarget.removeEventListener(sEventType, fnHandler, false);
    } else if (oTarget.detachEvent) {
        oTarget.detachEvent("on" + sEventType, fnHandler);
    } else {
        oTarget["on" + sEventType] = null;
    }
}
var Extend = function(destination, source) {
    for (var property in source) {
        destination[property] = source[property];
    }
};
var Bind = function(object, fun) {
    return function() {
        return fun.apply(object, arguments);
    }
};
var BindAsEventListener = function(object, fun) {
    return function(event) {
        return fun.call(object, (event || window.event));
    }
};

function DisableButtons(e) {
    var submit;
    if (e != null) {
        submit = e.parentElement;
    } else {
        submit = $("divSubmit");
    }
    var conns = submit.getElementsByTagName("INPUT");
    for (var i = 0; i < conns.length; i++) {
        conns[i].disabled = true;
    }
}
function DisableCtrls() {
    var conns = document.getElementsByTagName("INPUT");
    for (var i = 0; i < conns.length; i++) {
        conns[i].disabled = true;
    }
}
function AutoFitHeight(e) {
    try {
        var bHeight = e.contentWindow.document.body.scrollHeight;
        var dHeight = e.contentWindow.document.documentElement.scrollHeight;
        var height = Math.max(bHeight, dHeight);
        e.height = height;
    } catch (ex) { }
}
