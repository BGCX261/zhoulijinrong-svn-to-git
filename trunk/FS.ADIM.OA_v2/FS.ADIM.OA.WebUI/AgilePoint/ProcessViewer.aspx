<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ProcessViewer.aspx.cs"
    Inherits="FS.ADIM.OA.WebUI.AgilePoint.ProcessViewer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>流程图</title>

    <script type="text/javascript">

        var MAX_POP_HIGHT = 200;
        var block = false;
        var selectActivityName = null;
        var x_pos = 0;
        var y_pos = 0;
        var highlightCtrl = null;
        var oPopup = window.createPopup();

        function init()
        {
            window.dialogWidth = "770px";
            window.dialogHeight = "500px";

            var piID = ProcessViewer.hiddenProcInstID.value;
            if (piID == null || piID == "") return;

            highlightCtrl = document.all("HighlightActivity");

            service.useService("ProcessViewerService.asmx?WSDL", "Workflow");
            window.setInterval("updateProcessInstance()", 1000);

        }

        function showSubProcess(piID)
        {
            var szFeatures = 'scrollbars=yes,width=770,height=500,resizable=yes';
            var d = new Date();
            var timeStamp = d.getTime();

            var url = "ProcessViewer.aspx?PIID=" + piID;
            url = url + "&time=" + timeStamp;
            var parent = window.parent;
            var win = parent.open(url, "SubProcessViewer", szFeatures);
            if (win) win.focus();
        }

        function showProcessAdaptation(processAdptUrl)
        {
            var url = processAdptUrl;
            var win = window.open(url);
            if (win) win.focus();
        }
        function updateProcessInstance()
        {
            var piID = ProcessViewer.hiddenProcInstID.value;
            if (service.Workflow == null || piID == null || piID == "") return;
            if (ProcessViewer.ProcInstImage.complete && !block)
            {
                block = true;
                service.Workflow.callService(handleGetActivityInstStatus, "GetActivityInstStatus", piID);
            }
        }

        function handleGetActivityInstStatus(result)
        {
            block = false;
            if (result.error)
            {
                //alert( result.errorDetail.string );
                return;
            }

            var rv = result.value;
            if (rv == null || rv.length == 0) return;
            var name;
            for (i = 0; i < rv.length; ++i)
            {
                name = rv[i].Key;
                showImage(name + "_Active", false);
                showImage(name + "_Pending", false);
                showImage(name + "_Passed", false);
                showImage(name + "_Activated", false);
                showImage(name + "_" + rv[i].Value, true);
            }
        }

        function showActivityInfo()
        {
        
            if (service.Workflow == null) return;
            var ctrl = document.all(selectActivityName);
            if (ctrl == null) return;

            x_pos = event.screenX - window.screenLeft; //event.offsetX + 10 + div.offsetLeft;
            y_pos = event.screenY - window.screenTop; ; //event.offsetY + 10 + div.offsetTop;

            var piID = ProcessViewer.hiddenProcInstID.value;
            highlightCtrl.style.CURSOR = "wait";
            
             service.Workflow.callService(handleShowActivityInfo, "GetActivityInstInfo", piID, ctrl.value);
        }

        function handleShowActivityInfo(result)
        {
            if (result.error)
            {
                alert(result.errorDetail.string);
                return;
            }
            var body = oPopup.document.body;
            var div = document.all("popupDiv");
            div.innerHTML = result.value;
            div.style.display = "";
            var c = document.all("popupTable");

            var w = c.offsetWidth + 2;
            var h = c.offsetHeight + 2;
            if (h > MAX_POP_HIGHT)
            {
                w = w + 20;
                h = MAX_POP_HIGHT;
            }
            div.style.width = w;
            div.style.height = h;
            div.style.display = "none";
            div.style.display = "";

            body.innerHTML = div.outerHTML;

            oPopup.show(x_pos, y_pos, w, h, document.body);
            div.style.display = "none";
        }

        function highlightActivity(x, y, w, h, name)
        {
            var image = document.all("HighlightActivity");
            image.style.display = "";
            image.style.left = x + "px";
            image.style.top = y + "px";
            image.width = w;
            image.height = h;
            selectActivityName = name;
            oPopup.hide();
            return true;
        }

        function showImage(name, b)
        {
            var image = document.all(name);
            if (image == null) return;

            if (b)
            {
                image.style.display = "";
            }
            else
            {
                image.style.display = "none";
            }
        }
        
    </script>

    <meta content="True" name="vs_showGrid" />
    <link href="AgilePoint.css" type="text/css" rel="stylesheet" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
</head>
<body onload="init()">
    <form id="ProcessViewer" onload="Page_Load" runat="server">
    <table style="width: 99%; background-color: white" cellspacing="0" cellpadding="0"
        border="0">
        <tr>
            <td style="background-color: DarkBlue; height: 22px;">
                <asp:Label Font-Bold="True" Font-Size="14px" ForeColor="White" ID="lblTitleName"
                    runat="server" BackColor="DarkBlue" Width="100%"></asp:Label>
            </td>
            <td style="text-align: right; background-color: DarkBlue; height: 22px; color: White;
                font-size: 10pt; font-weight: bold;">
                Zoom Rate
                <asp:DropDownList ID="ddlZoomRate" runat="server" AutoPostBack="True" Font-Bold="true"
                    Font-Size="10pt" Font-Names="Verdana">
                    <asp:ListItem Selected="True">100</asp:ListItem>
                    <asp:ListItem>90</asp:ListItem>
                    <asp:ListItem>80</asp:ListItem>
                    <asp:ListItem>70</asp:ListItem>
                    <asp:ListItem>60</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>40</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                </asp:DropDownList>
                %
            </td>
        </tr>
        <tr>
            <td colspan="2" valign="top" align="left" style="background-color: white">
                <div id="diagram" style="left: 0px; position: relative; top: 0px">
                    <asp:Image ID="ProcInstImage" runat="server"></asp:Image>
                    <asp:PlaceHolder ID="PlaceHolder" runat="server"></asp:PlaceHolder>
                    <img src="Resource/en-US/HighlightActivity.gif" style="border-right: red 2px solid;
                        border-top: red 2px solid; display: none; left: 0px; border-left: red 2px solid;
                        cursor: hand; border-bottom: red 2px solid; position: absolute; top: 0px" onclick="showActivityInfo()"
                        id="HighlightActivity" />
                </div>
            </td>
        </tr>
    </table>
    <div>
        <input id="hiddenProcInstID" type="hidden" name="hiddenProcInstID" runat="server" /></div>
    <div id="service" style="behavior: url(webservice.htc)">
    </div>
    <map id="ActivityInfo">
        <asp:Literal ID="ActivityInfoMap" runat="server"></asp:Literal></map>
    <div id="popupDiv" style="display: none; overflow: auto">
    </div>
    </form>
</body>
</html>
