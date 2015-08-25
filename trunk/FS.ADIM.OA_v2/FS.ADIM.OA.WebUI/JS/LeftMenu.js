var selectedItem = null;

document.onclick = handleClick;

function handleClick() {
    el = getReal(window.event.srcElement, "tagName", "DIV");

    if ((el.className == "topFolder") || (el.className == "subFolder")) {
        el.sub = eval(el.id + "Sub");

        if (el.sub.style.display == null) {
            el.sub.style.display = "none";
        }
        if (el.sub.style.display != "block") {
            if (el.parentElement.openedSub != null) {
                var opener = eval(el.parentElement.openedSub + ".opener");
                hide(el.parentElement.openedSub);
                if (opener.className == "topFolder") {
                    outTopItem(opener);
                }
            }
            el.sub.style.display = "block";
            el.sub.parentElement.openedSub = el.sub.id;
            el.sub.opener = el;
            setCookies("menu", el.id);
        } else {
            if (el.sub.openedSub != null) {
                hide(el.sub.openedSub);
            } else {
                hide(el.sub.id);
            }
            setCookies("menu", "");
        }
    }
    if ((el.className == "subItem") || (el.className == "subFolder")) {
        if (selectedItem != null) {
            restoreSubItem(selectedItem);
        }
        highlightSubItem(el);
    }
    if ((el.className == "topItem") || (el.className == "topFolder")) {
        if (selectedItem != null) {
            restoreSubItem(selectedItem);
        }
    }
    if ((el.className == "topItem") || (el.className == "subItem")) {
        if ((el.href != null) && (el.href != "")) {
            if ((el.target == null) || (el.target == "")) {
                if (window.opener == null) {
                    if (document.all.tags("BASE").item(0) != null)
                        window.open(el.href, document.all.tags("BASE").item(0).target);
                    else window.location = el.href;
                } else {
                    window.opener.location = el.href;
                }
            } else {
                window.open(el.href, el.target);
            }
        }
    }
    var tmp = getReal(el, "className", "favMenu");
    if (tmp.className == "favMenu") {
        fixScroll(tmp);
    }
}
function restoreSubItem(e) {
    e.style.color = "black";
    e.style.fontWeight = "normal";
}
function highlightSubItem(e) {
    e.style.color = "white";
    e.style.fontWeight = "bold";
    selectedItem = e;
}
function hide(elID) {
    var el = eval(elID);
    el.style.display = "none";
    el.parentElement.openedSub = null;
    if (el.openedSub != null) {
        hide(el.openedSub);
    }
}
function outTopItem(el) {
    if ((el.sub != null) && (el.parentElement.openedSub == el.sub.id)) {
        with (el.style) {
            lineheight = "150%";
            marginleft = "20px";
        }
    } else {
        with (el.style) {
            lineheight = "150%";
            marginleft = "20px";
        }
    }
}
function getReal(el, type, value) {
    temp = el;
    while ((temp != null) && (temp.tagName != "BODY")) {
        if (eval("temp." + type) == value) {
            el = temp;
            return el;
        }
        temp = temp.parentElement;
    }
    return el;
}
function ItemMouseOver(e) {
    e.style.backgroundColor = "#FF8040";
}
function ItemMouseOut(e) {
    e.style.backgroundColor = "";
}
function HideMenu() {
    if (parent.leftframe.cols == '10,*') {
        parent.leftframe.cols = '150,*';
        window.scrollTo(0, 0);
        document.all.fimg.src = document.all.fimg.src.replace("Img/fimg1.gif", "Img/fimg.gif");
    } else {
        parent.leftframe.cols = '10,*';
        window.scrollTo(140, 0);
        document.all.fimg.src = document.all.fimg.src.replace("Img/fimg.gif", "Img/fimg1.gif");
    }
}
function setCookies(name, value) {
    var Days = 30;
    var exp = new Date();
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + ";expire*=" + exp.toGMTString();
}
function getCookies(name) {
    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (arr != null) return unescape(arr[2]);
    return null;
}
function LoadLast() {
    if (getCookies("menu") != null && getCookies("menu") != "") {
        eval(getCookies("menu")).click();
    }
}
function delCookies(name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookies(name);
    if (cval != null) {
        document.cookie = name + "=" + cval + ";expire*=" + exp.toGMTString();
    }
}