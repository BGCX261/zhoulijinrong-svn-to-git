<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Agent.ascx.cs" Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.PersonalManage.UC_Agent" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<!--<div>
    <div style="float: left">
        代理人：<cc1:FSTextBox ID="txtAgentUserName" runat="server" Width="60px"></cc1:FSTextBox>
    </div>
    <div style="float: left">
        
    </div>
</div>-->

<script language="javascript" type="text/javascript">
    function onPost() {
        __doPostBack("", "");
    }
</script>

<script type="text/javascript">

    function postBackByObject() {
        var o = window.event.srcElement;
        if (o.tagName == "INPUT" && o.type == "checkbox") //点击treeview的checkbox是触发
        {
            var d = o.id; //获得当前checkbox的id;
            var e = d.replace("CheckBox", "Nodes"); //通过查看脚本信息,获得包含所有子节点div的id
            var div = window.document.getElementById(e); //获得div对象
            if (div != null)  //如果不为空则表示,存在子节点
            {
                var check = div.getElementsByTagName("INPUT"); //获得div中所有的已input开始的标记
                for (i = 0; i < check.length; i++) {
                    if (check[i].type == "checkbox") //如果是checkbox
                    {
                        check[i].checked = o.checked; //子节点的状态和父节点的状态相同,即达到全选
                    }
                }
            }
        }
    }
</script>

<script type="text/javascript">
    function GetParent() {
        //__doPostBack('btnSX', '');
        //$('<%=btnSX.ClientID %>').click();
    }
</script>

<asp:ScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table>
            <tr>
                <td valign="top">
                    <table style="width: 370px">
                        <tr>
                            <td>
                                部门
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblInfo" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <div style="height: 400px; overflow: scroll;">
                                    <asp:TreeView ID="tvDB" runat="server" ExpandDepth="1" ShowCheckBoxes="All" ShowLines="True"
                                        Width="200px">
                                        <SelectedNodeStyle ForeColor="Blue" />
                                    </asp:TreeView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top" style="width: 299px">
                    <asp:Panel ID="PanelUser" runat="server">
                        <table>
                            <tr>
                                <td>
                                    部门人员
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    代理人
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="lboxLeft" runat="server" BackColor="#CCFFFF" Height="390px" SelectionMode="Multiple"
                                        Width="200px"></asp:ListBox>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td style="height: 40px">
                                                <asp:Button ID="btnAddAll" runat="server" class="btn" OnClick="btnAddAll_Click" Text="&gt;&gt;"
                                                    ToolTip="左侧全部添加" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 40px">
                                                <asp:Button ID="btnAddOne" runat="server" class="btn" OnClick="btnAddOne_Click" Text="&gt;"
                                                    ToolTip="左侧选中添加" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 40px">
                                                <asp:Button ID="btnDeleteOne" runat="server" class="btn" OnClick="btnDeleteOne_Click"
                                                    Text="&lt;" ToolTip="右侧选中移除" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 40px">
                                                <asp:Button ID="btnDeleteAll" runat="server" class="btn" OnClick="btnDeleteAll_Click"
                                                    Text="&lt;&lt;" ToolTip="右侧全部移除" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:ListBox ID="lboxRight" runat="server" Height="390px" SelectionMode="Multiple"
                                        Width="120px" BackColor="#CCFFFF"></asp:ListBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:Button ID="btnOK" runat="server" Text="确定并关闭" CssClass="btn" OnClick="btnOK_Click"
                        Visible="False" />
                </td>
            </tr>
        </table>
        <div style="display: none">
            <asp:HiddenField ID="hUCDeptID" runat="server" />
            <asp:HiddenField ID="hUCDeptUserID" runat="server" />
            <asp:HiddenField ID="hUCRoleUserID" runat="server" />
            <asp:Button ID="btnSX" runat="server" Text="刷新" OnClick="btnSX_Click" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
