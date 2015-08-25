<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Left.aspx.cs" Inherits="FS.ADIM.OA.WebUI.Left" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <title>核电海南办公文档一体化系统</title>

    <script language="javascript" type="text/javascript" src="Js/LeftMenu.js"></script>

    <link href="Css/Common.css" rel="stylesheet" type="text/css" />
    <link href="Css/Left.css" rel="stylesheet" type="text/css" />
    <style type="text/css" media="screen">
        html, BODY, FORM { height: 100%; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="div_hide" onclick="HideMenu();">
        <img src="Img/fimg.gif" name="fimg" alt="" /></div>
    <div class="left_main">
        <div class="left_bt">
            <div>
                <img src="../Img/sys_oa.gif" alt="" />OA
            </div>
        </div>
        <div align="center">
            <hr width="120" style="color: #007BB7" />
        </div>
        <div>
            <asp:Label ID="lblMenu" runat="server"></asp:Label>
        </div>
    </div>
    </form>
</body>
</html>
