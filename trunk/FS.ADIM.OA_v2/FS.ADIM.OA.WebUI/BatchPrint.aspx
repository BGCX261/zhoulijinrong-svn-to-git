<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchPrint.aspx.cs" Inherits="FS.ADIM.OA.WebUI.BatchPrint" %>

<%@ Register Src="PageWF/UC_Print.ascx" TagName="UC_Print" TagPrefix="uc1" %>
<%@ Register assembly="FounderSoftware.Framework.UI.WebCtrls" namespace="FounderSoftware.Framework.UI.WebCtrls" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>批量打印</title>
    <link href="Css/Control.css" rel="stylesheet" type="text/css" />
    <link href="Css/FormPage.css" rel="stylesheet" type="text/css" />
    <script src="Js/Common.js" type="text/javascript"></script>
    <script src="Js/Popup.js" type="text/javascript"></script>
    <script language="javascript"> 
    <!-- Hide 
    function killErrors() { 
    return true; 
    } 
    window.onerror = killErrors; 
    // --> 
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <p>
    <asp:Label ID="Label3" runat="server" Text="起始时间:"></asp:Label>
    <cc1:FSCalendar ID="CaleStart" runat="server" Width="150px" Value="2010-03-22" AutoPostBack=true>2010-01-01</cc1:FSCalendar>
    <br />
    <asp:Label ID="Label4" runat="server" Text="结束时间:"></asp:Label>
    <cc1:FSCalendar ID="CaleEnd" runat="server" Width="150px" Value="2010-03-24">2010-03-24</cc1:FSCalendar>
    <br />
    <asp:Label ID="Label1" runat="server" Text="流程类型:"></asp:Label>
    <asp:DropDownList ID="ddlProc" runat="server" Height="26px" Width="123px" AutoPostBack=true
        onselectedindexchanged="ddlProc_SelectedIndexChanged">
        <asp:ListItem>公司发文</asp:ListItem>
        <asp:ListItem>公司收文</asp:ListItem>
        <asp:ListItem>函件发文</asp:ListItem>
        <asp:ListItem>函件收文</asp:ListItem>
        <asp:ListItem>请示报告</asp:ListItem>
        <asp:ListItem>工作联系单</asp:ListItem>
        <asp:ListItem>程序文件</asp:ListItem>
        <asp:ListItem>党政工团发文</asp:ListItem>
        <asp:ListItem>党政工团收文</asp:ListItem>
    </asp:DropDownList>
    <br />
    <asp:Label ID="Label2" runat="server" Text="步骤名称:"></asp:Label>
    <asp:DropDownList ID="ddlStep" runat="server" Height="26px" Width="123px"
        AutoPostBack=true>
        <asp:ListItem>拟稿</asp:ListItem>
        <asp:ListItem>审稿</asp:ListItem>
        <asp:ListItem>部门会签</asp:ListItem>
        <asp:ListItem>核稿</asp:ListItem>
        <asp:ListItem>主任核稿</asp:ListItem>
        <asp:ListItem>领导会签</asp:ListItem>
        <asp:ListItem>签发</asp:ListItem>
        <asp:ListItem>校对</asp:ListItem>
        <asp:ListItem Selected="True">分发</asp:ListItem>
        <asp:ListItem>传阅</asp:ListItem>
    </asp:DropDownList>
    <br />
    <uc1:UC_Print ID="UC_Print1" runat="server" />
    文件将被输出到服务器FS.ADIM.OA.WebUI\Batch\目录下<p>
    <asp:TextBox ID="TextBox1" runat="server" ReadOnly="True" Width="597px"></asp:TextBox>
    </form>
</body>
</html>
