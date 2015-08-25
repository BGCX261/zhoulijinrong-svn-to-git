//Modify By IvanYao
document.write("<iframe id=OuterIframe scrolling=no frameborder=0 style='display:none;' class='iframeContainer'></iframe>");
function writeIframe() {
    var strIframe = "<html><head></head><body style='background-Color:gray'></body></html>";
    with (window.frames["OuterIframe"]) {
        document.write(strIframe);
    }
    var iframe = $("OuterIframe");
    iframe.style.height = getPosition().scrollHeight;
    iframe.style.width = getPosition().scrollWidth;
    iframe.style.display = '';
}
function hideIframe() {
    var iframe = $("OuterIframe");
    if (iframe == null) {
        return;
    }
    iframe.style.display = 'none';
}
function getPosition() {
    return {
        top: document.documentElement.scrollTop,
        left: document.documentElement.scrollLeft,
        height: document.documentElement.clientHeight,
        width: document.documentElement.clientWidth,
        scrollHeight: document.documentElement.scrollHeight,
        scrollWidth: document.documentElement.scrollWidth
    };
}
//弹出层
function ShowPopDiv(divContent, option) {
    writeIframe();
    var content = $(divContent), width;
    if (!!(option && option.width)) {
        width = parseInt(option.width);
    } else {
        width = parseInt(content.style.width);
    }
    var height = parseInt(content.style.height);
    var loadingImg = $('imgLoading');

    var Position = getPosition();

    content.style.top = (Position.top + (Position.height - height) / 2) + "px";
    content.style.left = (Position.left + (Position.width - width) / 2) + "px";
    content.style.width = width + "px";
    content.style.display = "";

    loadingImg.style.top = (Position.top + (Position.height - 84) / 2) + "px";
    loadingImg.style.left = (Position.left + (Position.width - 84) / 2) + "px";

    window.onscroll = Bind(content, MediatePosition);

//    event.returnValue = false;
}

//隐藏弹出层
function hiddenPopDiv(divmain1, divcontents1) {

    $(divmain1).style.display = "none";
    $(divcontents1).style.display = "none";
    var checks = document.getElementsByTagName("INPUT");
    for (i = 0; i < checks.length; i++) {
        if (checks[i].type == 'checkbox' && checks[i].id.indexOf(divmain1.substr(9)) > -1) {
            checks[i].checked = false;
        }
    }
    window.onresize = function() { return false; }
    window.onscroll = function() { return false; }
}

//隐藏层
function ClosePopDiv(divContent) {
    $(divContent).style.display = "none";
    hideIframe();
    window.onscroll = null;
}
function HideLoadingPic() {
    var loadingImg = $("imgLoading");
    if (loadingImg == null) {
        return;
    }
    loadingImg.style.display = 'none';
}
function ShowLoadingPic() {
    var loadingImg = $("imgLoading");
    if (loadingImg == null) {
        return;
    }
    loadingImg.style.display = '';
}
function MediatePosition() {
    var Position = getPosition();
    this.style.top = (Position.top + (Position.height - parseInt(this.style.height)) / 2) + "px";
    this.style.left = (Position.left + (Position.width - parseInt(this.style.width)) / 2) + "px";
}