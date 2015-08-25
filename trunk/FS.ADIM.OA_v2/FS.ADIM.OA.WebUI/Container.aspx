<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Container.aspx.cs" Inherits="FS.ADIM.OA.WebUI.Container" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="Css/Common.css" rel="stylesheet" type="text/css" />
    <link href="Css/Control.css" rel="stylesheet" type="text/css" />
    <link href="Css/List.css" rel="stylesheet" type="text/css" />
    <link href="Css/FormPage.css" rel="stylesheet" type="text/css" />

    <script src="Js/Common.js" type="text/javascript"></script>

    <script src="Js/Drag.js" type="text/javascript"></script>

    <script src="Js/Popup.js" type="text/javascript"></script>

    <style type="text/css" media="screen">
        html { overflow-y: auto !important; *overflow-y:scroll;}</style>
</head>
<body>
    <form runat="server" id="form1">
    <img src='../Img/loading12.gif' id='imgLoading' style='display: none; position: absolute;
        z-index: 14'></img>
    <table class="table_gv_top">
        <tr class="table_gv_title">
            <td>
                <table>
                    <tr>
                        <td style="width: 30px;">
                            <img src="Img/i_info_view.gif" alt="" />
                        </td>
                        <td>
                            <span id="containTitle" class="HeadTitle" runat="server"></span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="table_add_partition">
            </td>
        </tr>
        <tr>
            <td>
                <div id="containBody" runat="server">
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
