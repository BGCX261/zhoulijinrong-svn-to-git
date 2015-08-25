<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PG_CompanyMore.aspx.cs"
    Inherits="FS.ADIM.OA.WebUI.PageOU.PG_CompanyMore" %>

<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/Common.css" rel="stylesheet" type="text/css" />
    <link href="../css/PopPage.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
    function GetParent() {
    }
    </script>

    <style type="text/css" media="screen">
        html
        {
            overflow-y: auto !important; *overflow-y:scroll;}</style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" runat="server" EnableScriptGlobalization="false" EnableScriptLocalization="false">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <center>
                <table cellspacing="0" valign="top" style="margin-top:5px" width="300px">
                    <tr>
                        <td valign="top" align="left">
                            <div style="color: #0099cc; font-weight: bold; text-align: left">
                                选取外单位</div>
                            <cc1:FSMatchSelector ID="msExoticCompanyList" runat="server" ShowValueField="false"
                                Height="330" DataValueField="Name" DataTextField="Name" ListBoxHeight="300" ListBoxWidth="300" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="Button1" runat="server" Text="确定" OnClick="btnOK_Click" CssClass="btn" />
                        </td>
                    </tr>
                </table>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
