<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_ProgramType.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.ProgramFileMaintain.UC_ProgramType" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>

<script language="javascript" type="text/javascript">
    $("containTitle").innerHTML = "程序类型维护";
    function DelConfirm() {
        var selected = $("<%=hfSelectedIndex.ClientID%>").value;
        if (selected == "") {
            return false;
        } else {
            return confirm('是否真的要删除？');
        }
    }
    function EditConfirm() {
        var selected = $("<%=hfSelectedIndex.ClientID%>").value;
        if (selected == "") {
            return false;
        } else {
            return FSTextBox_SubmitCheck()
        }
    }
    function AddConfirm() {
        var objDllSort = $("<%=ddlProgramSort.ClientID %>");
        var selectedValue = objDllSort.options[objDllSort.selectedIndex].text;
        if (selectedValue == "部门级管理程序" || selectedValue == "管理程序") {
            if (confirm('请确认文档中心是否添加该分类,并且在DevolveConfig.xml中进行配置？')) {
                return FSTextBox_SubmitCheck();
            } else {
                return false;
            }
        } else {
            return FSTextBox_SubmitCheck();
        }
    }
    
</script>

<asp:ScriptManager ID="ScriptManager1" EnableScriptGlobalization="false" EnableScriptLocalization="false"
    runat="server">
</asp:ScriptManager>
<div class="divProgress">
    <asp:UpdateProgress ID="UProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <img alt="" src="../../css/images/loading/loading11.gif" />
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div>
            <div class="div_gv_tool">
                <cc1:FSLinkButton ID="lnkbtnAdd" runat="server" CssClass="a_add" OnClientClick="return AddConfirm()"
                    OnClick="lnkbtnAdd_Click">添加</cc1:FSLinkButton>
                <cc1:FSLinkButton ID="lnkbtnModify" runat="server" CssClass="a_edit" OnClick="lnkbtnModify_Click"
                    Enabled="False" OnClientClick="return EditConfirm()">修改</cc1:FSLinkButton>
                <cc1:FSLinkButton ID="lnkbtnDelete" runat="server" CssClass="a_delete" OnClientClick="return DelConfirm()"
                    OnClick="lnkbtnDelete_Click" Enabled="False">删除</cc1:FSLinkButton>
            </div>
            <table>
                <tr>
                    <td style="width: 60px">
                        <span id="Label1" class="label">程序类型：</span>
                    </td>
                    <td style="width: 170px">
                        <cc1:FSTextBox ID="txtProgramType" runat="server" Width="150" RequiredType="NotNull"
                            MaxLength="20" CssClass="txtbox_yellow"></cc1:FSTextBox>
                        <span style="color: Red">*</span>
                    </td>
                    <td style="width: 85px">
                        <span id="Span1" class="label">程序类型描述：</span>
                    </td>
                    <td style="width: 225px">
                        <cc1:FSTextBox ID="txtProgramTypeDesc" runat="server" Width="200" MaxLength="50"
                            CssClass="txtbox_yellow"></cc1:FSTextBox>
                    </td>
                    <td style="width: 60px">
                        <span id="Span2" class="label">程序分类：</span>
                    </td>
                    <td style="width: 140px">
                        <cc1:FSDropDownList ID="ddlProgramSort" runat="server" Width="120px" CssClass="dropdownlist_yellow">
                        </cc1:FSDropDownList>
                        <span style="color: Red">*</span>
                    </td>
                </tr>
            </table>
        </div>
        <div width="750px">
            <cc1:FSGridView ID="gvProgramTypeList" runat="server" AutoGenerateColumns="False"
                AutoGenerateSelectButton="true" CssClass="table_gv" DataKeyNames="ID" AllowPaging="True"
                AllowSorting="false" PageSize="10" OnPageIndexChanged="gvProgramTypeList_PageIndexChanged"
                OnSelectedIndexChanged="gvProgramTypeList_SelectedIndexChanged" OnRowDataBound="gvProgramTypeList_RowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="程序类型" DataField="Name">
                        <HeaderStyle CssClass="th_j04" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="程序类型描述" DataField="Description">
                        <HeaderStyle CssClass="th_j06" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="程序分类" DataField="Sort">
                        <HeaderStyle CssClass="th_j04" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="ID" Visible="false" DataField="ID" />
                </Columns>
                <SelectedRowStyle BackColor="#B3D0F5" ForeColor="Black" />
                <RowStyle HorizontalAlign="Center" />
            </cc1:FSGridView>
            <cc1:FSHiddenField ID="hfSelectedIndex" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
