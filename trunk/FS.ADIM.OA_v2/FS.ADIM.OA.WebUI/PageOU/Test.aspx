<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="FS.ADIM.OA.WebUI.PageOU.Test" %>

<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="UC_Role.ascx" TagName="RoleUC" TagPrefix="uc" %>
<%@ Register Src="UC_OASelect.ascx" TagName="OASelectUC" TagPrefix="uc" %>
<%@ Register Src="UC_Company.ascx" TagName="UC_Company" TagPrefix="uc1" %>
<%@ Register Src="UC_CompanyMore.ascx" TagName="UC_CompanyMore" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../css/List.css" rel="stylesheet" type="text/css" />
    <link href="../css/FormPage.css" rel="stylesheet" type="text/css" />
    <link href="../css/Control.css" rel="stylesheet" type="text/css" />

    <script src="../Js/Common.js" type="text/javascript"></script>

    <script src="../Js/Drag.js" type="text/javascript"></script>

    <script src="../Js/Popup.js" type="text/javascript"></script>

    <style type="../text/css" media="screen">
        html
        {
            overflow-y: auto !important; *overflow-y:scroll;}</style>
</head>
<body>
    <form id="form1" runat="server">
    &nbsp;公司ID：<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <br />
    公司NO：<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
    <br />
    公司Name：<asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
    &nbsp;&nbsp;
    <uc1:UC_Company ID="UC_Company1" runat="server" />
    <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
    <uc2:UC_CompanyMore ID="UC_Company2" runat="server" />
    <br />
    角色：<asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
    <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
    &nbsp;
    <uc:RoleUC ID="RoleUC1" runat="server" />
    <br />
    <div>
        选择方式：
        <asp:DropDownList ID="drpSelectType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSelectType_SelectedIndexChanged">
            <asp:ListItem Value="0">部门</asp:ListItem>
            <asp:ListItem Value="1">人员</asp:ListItem>
            <asp:ListItem Value="2">2者都选</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:CheckBox ID="CheckBox1" runat="server" Text="是否单选" AutoPostBack="True" />
        &nbsp;
        <asp:CheckBox ID="CheckBox2" runat="server" Text="是否全选" AutoPostBack="true" />
        <br />
        可点击的部门ID,逗号分隔
        <asp:TextBox ID="txtShowDeptID" runat="server" AutoPostBack="True"></asp:TextBox>
        <br />
        显示几层部门
        <asp:TextBox ID="txtLevel" runat="server" AutoPostBack="True" Text="3"></asp:TextBox>
        <br />
        指定特殊角色
        <asp:TextBox ID="txtRole" runat="server" AutoPostBack="True" Text="公司领导"></asp:TextBox>
        <br />
        <asp:CheckBoxList ID="CheckBoxList1" runat="server" AutoPostBack="True">
            <asp:ListItem Value="1">负责人</asp:ListItem>
            <asp:ListItem Value="2">&gt;副处</asp:ListItem>
            <asp:ListItem Value="3">部门领导</asp:ListItem>
        </asp:CheckBoxList>
        <br />
        &nbsp;&nbsp;
        <uc:OASelectUC ID="UCDeptSelect1" runat="server" />
        <p>
            <hr />
            回传:="server" />
            <hr />
            回传:</p>
        <p>
            &nbsp;部门ID
            <asp:TextBox ID="TextBox11" runat="server"></asp:TextBox>
            <br />
            部门名
            <asp:TextBox ID="TextBox12" runat="server"></asp:TextBox>
            <br />
            用户ID
            <asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>
            <br />
            用户名
            <asp:TextBox ID="TextBox14" runat="server"></asp:TextBox>
            <br />
            部门和用户名称：
            <asp:TextBox ID="TextBox19" runat="server"></asp:TextBox>
        </p>
        <p>
            部门树节点上的用户ID:<asp:TextBox ID="TextBox17" runat="server"></asp:TextBox>
        </p>
        <p>
            部门树节点上的用户姓名：<asp:TextBox ID="TextBox18" runat="server"></asp:TextBox>
        </p>
        <p>
            bu特殊角色用户ID
            <asp:TextBox ID="TextBox15" runat="server"></asp:TextBox>
        </p>
        <p>
            特殊角色用户名
            <asp:TextBox ID="TextBox16" runat="server"></asp:TextBox>
        </p>
    </div>
    </form>
</body>
</html>
