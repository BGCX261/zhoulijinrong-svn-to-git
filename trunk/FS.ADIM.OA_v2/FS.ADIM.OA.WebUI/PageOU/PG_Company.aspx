<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PG_Company.aspx.cs" Inherits="FS.ADIM.OA.WebUI.PageOU.PG_Company" %>

<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/Common.css" rel="stylesheet" type="text/css" />
    <link href="../css/PopPage.css" rel="stylesheet" type="text/css" />
    <link href="../css/List.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
    function GetParent() {
        if (document.getElementById("<%=UCIDControl%>")!= null) {
            document.getElementById("hUCIDControls").value = parent.document.getElementById("<%=UCIDControl%>").value;
        } else {
            __doPostBack('btnSX', '');
        }
    }
    </script>

    <style type="text/css" media="screen">
        html { overflow-y: auto !important; *overflow-y:scroll;}</style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" runat="server" EnableScriptGlobalization="false" EnableScriptLocalization="false">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlType" runat="server">
                            <asp:ListItem Value="[Name]">单位名称</asp:ListItem>
                            <asp:ListItem Value="No">单位编号</asp:ListItem>
                            <asp:ListItem Value="ContactPerson">单位联系人</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSearchText" runat="server" Width="120px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnSearch" runat="server" Text="查找" OnClick="btnSearch_Click" CssClass="btn" />
                    </td>
                </tr>
            </table>
            <div style="width: 100%; margin-top: 5px;">
                <cc1:FSGridView ID="gvCompany" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                    AllowPaging="True" ShowEmptyHeader="true" PageType="InteriorPage" PageSize="10"
                    ShowRadioButton="true" CheckTemplateHeaderText="单选" AllowSpanCheck="true" DataKeyNames="ID">
                    <Columns>
                        <asp:BoundField DataField="ID" Visible="false" HeaderText="ID"></asp:BoundField>
                        <asp:BoundField DataField="No" HeaderText="编号" HeaderStyle-Width="90px"></asp:BoundField>
                        <asp:BoundField DataField="Name" HeaderText="单位名称"></asp:BoundField>
                        <asp:BoundField DataField="contactperson" HeaderText="联系人" HeaderStyle-Width="90px">
                        </asp:BoundField>
                        <asp:BoundField DataField="Faxnumber" HeaderText="传真" HeaderStyle-Width="120px">
                        </asp:BoundField>
                        <asp:BoundField DataField="EmailAddress" Visible="false" HeaderText="Email" HeaderStyle-Width="120px">
                        </asp:BoundField>
                    </Columns>
                </cc1:FSGridView>
            </div>
            <div style="width: 100%; text-align: center; margin-top: 5px;">
                <asp:Button ID="btnOK" runat="server" Text="添加" OnClick="btnOK_Click" CssClass="btn" />
                <asp:Button ID="btnClose" runat="server" Text="关闭" OnClick="btnClose_Click" CssClass="btn" />
                <asp:Button ID="btnClear" runat="server" CssClass="btn" OnClick="btnClear_Click"
                    Text="清空" />
            </div>
            <div style="display: none">
                <asp:HiddenField ID="hUCIDControls" runat="server" />
                <asp:Button ID="btnSX" runat="server" Text="刷新" OnClick="btnSX_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
