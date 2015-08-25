<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PG_OASelect.aspx.cs" Inherits="FS.ADIM.OA.WebUI.PageOU.PG_OASelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/Common.css" rel="stylesheet" type="text/css" />
    <link href="../css/PopPage.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function onPost() {
            __doPostBack("", "");
        }
        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                var d = o.id;
                var e = d.replace("CheckBox", "Nodes");
                var div = window.document.getElementById(e);
                if (div != null) {
                    var check = div.getElementsByTagName("INPUT");
                    for (i = 0; i < check.length; i++) {
                        if (check[i].type == "checkbox") {
                            check[i].checked = o.checked;
                        }
                    }
                }
            }
        }
        function GetParent() {
                   
                if ("<%=UCDeptIDControl %>" != "") {
                    document.getElementById("hUCDeptID").value = parent.document.getElementById("<%=UCDeptIDControl%>").value;
                }
                if ("<%=UCDeptUserIDControl %>" != "") {
                   document.getElementById("hUCDeptUserID").value = parent.document.getElementById("<%=UCDeptUserIDControl %>").value;
                }
                if ("<%=UCRoleUserIDControl %>" != "") {
                    document.getElementById("hUCRoleUserID").value = parent.document.getElementById("<%=UCRoleUserIDControl %>").value;
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
            <table style="width: 100%" cellspacing="0">
                <tr>
                    <td valign="top">
                        <div style="overflow-y: scroll; width: 250px; height: 381px;" runat="server" id="divDeptList">
                            <asp:TreeView ID="tvDeptList" runat="server" ShowLines="True" ShowCheckBoxes="All">
                            </asp:TreeView>
                            <asp:TreeView ID="tvRoleList" runat="server" ShowLines="True" Visible="false">
                            </asp:TreeView>
                        </div>
                    </td>
                    <td valign="top">
                        <asp:Panel ID="PanelUser" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:ListBox ID="lbxLeft" runat="server" Width="150px" Height="381px" BackColor="#CCFFFF"
                                            SelectionMode="Multiple"></asp:ListBox>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddAll" class="btn" runat="server" Text=">>" OnClick="btnAddAll_Click"
                                                        ToolTip="左侧全部添加" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddOne" class="btn" runat="server" Text=">" OnClick="btnAddOne_Click"
                                                        ToolTip="左侧选中添加" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnDeleteOne" class="btn" runat="server" Text="<" OnClick="btnDeleteOne_Click"
                                                        ToolTip="右侧选中移除" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnDeleteAll" class="btn" runat="server" Text="<<" OnClick="btnDeleteAll_Click"
                                                        ToolTip="右侧全部移除" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lbxRight" runat="server" Width="150px" Height="381px" SelectionMode="Multiple">
                                        </asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="PanelRole" runat="server">
                            <asp:DropDownList ID="drpRole" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpRole_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:CheckBoxList ID="chkRole" runat="server" RepeatDirection="Horizontal">
                            </asp:CheckBoxList>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <div class="divSubmit" id="divSubmit">
                <asp:Button ID="btnOK" runat="server" Text="确定" CssClass="btn" OnClick="btnOK_Click" /></div>
            <div style="display: none">
                <asp:HiddenField ID="hUCDeptID" runat="server" />
                <asp:HiddenField ID="hUCDeptUserID" runat="server" />
                <asp:HiddenField ID="hUCRoleUserID" runat="server" />
                <asp:Button ID="btnSX" runat="server" Text="刷新" OnClick="btnSX_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
