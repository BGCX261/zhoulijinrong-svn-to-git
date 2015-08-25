<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PG_Role.aspx.cs" Inherits="FS.ADIM.OA.WebUI.PageOU.PG_Role" %>

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
            if (parent.document.getElementById("<%=UCUserIDControl %>") != null) {
                document.getElementById("hUserID").value = parent.document.getElementById("<%=UCUserIDControl %>").value;
            }
            __doPostBack('btnSX', '');
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
            <div style="height: 330px; overflow-y: auto">
                <cc1:FSGridView ID="gvRole" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    ShowEmptyHeader="true" Width="95%" OnRowDataBound="gvRole_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="选择" HeaderStyle-Width="30px" ItemStyle-CssClass="td_Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkStatus" ToolTip='<%# GetDomainID(DataBinder.Eval(Container.DataItem, "Domain").ToString(),DataBinder.Eval(Container.DataItem, "UserID").ToString()) +"|"+ DataBinder.Eval(Container.DataItem,"Name") %>'
                                    runat="server" AutoPostBack="true" OnCheckedChanged="chkStatus_CheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="选择" HeaderStyle-Width="30px" ItemStyle-CssClass="td_Center">
                            <ItemTemplate>
                                <asp:RadioButton ID="rbtnStatus" ToolTip='<%# GetDomainID(DataBinder.Eval(Container.DataItem, "Domain").ToString(),DataBinder.Eval(Container.DataItem, "UserID").ToString()) +"|"+ DataBinder.Eval(Container.DataItem, "Name") %>'
                                    runat="server" OnCheckedChanged="rbtnStatus_CheckedChanged" AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-Width="30px"></asp:BoundField>
                        <asp:BoundField DataField="Name" HeaderText="员工姓名" HeaderStyle-Width="150px"></asp:BoundField>
                        <asp:TemplateField HeaderText="账号">
                            <ItemTemplate>
                                <asp:Label ID="lblUserID" runat="server" Text='<%#GetDomainID(DataBinder.Eval(Container.DataItem, "Domain").ToString(),DataBinder.Eval(Container.DataItem, "UserID").ToString())%>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </cc1:FSGridView>
            </div>
            <table width="100%" style="display: none">
                <tr>
                    <td style="color: #8080FF">
                        已选择的人员：
                    </td>
                </tr>
                <tr>
                    <td>
                        <cc1:FSListBox ID="fsltbRoleUser" runat="server" Width="370px" SelectionMode="Multiple"
                            Rows="4">
                        </cc1:FSListBox>
                    </td>
                </tr>
            </table>
            <div style="width: 100%; text-align: center; margin-top: 5px;">
                <asp:Button ID="btnOK" runat="server" CssClass="btn" Text="确定" OnClick="btnOK_Click" />
                <asp:Button ID="btnClear" runat="server" CssClass="btn" OnClick="btnClear_Click"
                    Text="清除" /></div>
            <div style="display: none">
                <asp:HiddenField ID="hUserID" runat="server" />
                <asp:Button ID="btnSX" runat="server" Text="刷新" OnClick="btnSX_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
