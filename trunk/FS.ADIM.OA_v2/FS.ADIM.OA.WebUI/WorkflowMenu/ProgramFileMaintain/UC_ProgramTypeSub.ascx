<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_ProgramTypeSub.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.ProgramFileMaintain.UC_ProgramTypeSub" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>

<script language="javascript" type="text/javascript">
    $("containTitle").innerHTML = "程序子类维护";
    function DelConfirm() {
        var selectedIndex = $("<%=hfSelectedIndex.ClientID%>").value;
        if (selectedIndex == "") {
            return false;
        } else {
            return confirm('是否真的删除？');
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
</script>

<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="false"
    EnableScriptLocalization="false">
</asp:ScriptManager>
<div class="divProgress">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <img alt="" src="../../CSS/Images/loading11.gif" />
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div>
            <div class="div_gv_tool">
                <cc1:FSLinkButton ID="lnkbtnAdd" runat="server" CssClass="a_add" OnClientClick="return FSTextBox_SubmitCheck ()"
                    OnClick="lnkbtnAdd_Click">添加</cc1:FSLinkButton>
                <cc1:FSLinkButton ID="lnkbtnModify" runat="server" CssClass="a_edit" OnClick="lnkbtnModify_Click"
                    Enabled="false" OnClientClick="return EditConfirm()">修改</cc1:FSLinkButton>
                <cc1:FSLinkButton ID="lnkbtnDelete" runat="server" CssClass="a_delete" OnClientClick="return DelConfirm()"
                    OnClick="lnkbtnDelete_Click" Enabled="false">删除</cc1:FSLinkButton>
                <cc1:FSLinkButton ID="lnkbtnSearch" runat="server" CssClass="a_search" OnClick="lnkbtnSearch_Click">查询</cc1:FSLinkButton>
            </div>
            <table>
                <tr>
                    <td style="width: 60px; padding-left: 4px">
                        程序类型：
                    </td>
                    <td style="width: 140px">
                        <cc1:FSDropDownList ID="ddlProgramType" runat="server" Width="120px" CssClass="dropdownlist_yellow"
                            AutoPostBack="True">
                        </cc1:FSDropDownList>
                        <span style="color: Red">*</span>
                    </td>
                    <td style="width: 60px">
                        <span id="Label1" class="label">程序子类：</span>
                    </td>
                    <td style="width: 170px">
                        <cc1:FSTextBox ID="txtProgramSubType" runat="server" RequiredType="NotNull" MaxLength="20"
                            CssClass="txtbox_yellow" Style="width: 120px;"></cc1:FSTextBox>
                        <span style="color: Red">*</span>
                    </td>
                    <td style="width: 60px">
                        <span id="Span1" class="label">编码结构：</span>
                    </td>
                    <td style="width: 150px">
                        <cc1:FSTextBox ID="txtCodeFrame" runat="server" MaxLength="20" CssClass="txtbox_yellow"></cc1:FSTextBox>
                        <span style="color: White">*</span>
                    </td>
                    <td style="width: 85px">
                        &nbsp;
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table>
                            <tr>
                                <td>
                                    <span id="Span2" class="label">程序子类描述：</span>
                                </td>
                                <td>
                                    <cc1:FSTextBox ID="txtProgramSubTypeDesc" runat="server" Width="200" MaxLength="50"
                                        CssClass="txtbox_yellow"></cc1:FSTextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 60px">
                    </td>
                    <td style="width: 150px">
                    </td>
                    <td style="width: 85px">
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
        <div width="750px">
            <cc1:FSGridView ID="gvProgramSubTypeList" runat="server" AutoGenerateColumns="False"
                CheckTemplateHeaderText="选择" AutoGenerateSelectButton="True" DataKeyNames="ID"
                AllowPaging="True" AllowSorting="false" PageSize="10" OnSelectedIndexChanged="gvProgramSubTypeList_SelectedIndexChanged"
                CssClass="table_gv" OnRowDataBound="gvProgramSubTypeList_RowDataBound" OnExteriorPaging="gvProgramSubTypeList_ExteriorPaging">
                <Columns>
                    <asp:BoundField HeaderText="程序类型" DataField="TypeName">
                        <HeaderStyle CssClass="th_j03" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="程序子类" DataField="SubTypeName">
                        <HeaderStyle CssClass="th_j03" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Description" HeaderText="程序子类描述">
                        <HeaderStyle CssClass="th_j07" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="编码结构" DataField="CodeFrame">
                        <HeaderStyle CssClass="th_j03" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Sort" HeaderText="Sort">
                        <HeaderStyle CssClass="hidden" />
                        <ItemStyle CssClass="hidden" />
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
