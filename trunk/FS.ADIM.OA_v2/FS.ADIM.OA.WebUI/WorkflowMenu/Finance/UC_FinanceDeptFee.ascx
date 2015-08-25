<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_FinanceDeptFee.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.Finance.UC_FinanceDeptFee" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<script language="javascript" type="text/javascript">
    $("containTitle").innerHTML = "财务费用维护";
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
                <cc1:FSLinkButton ID="lnkbtnAdd" runat="server" CssClass="a_add" OnClick="lnkbtnAdd_Click">添加</cc1:FSLinkButton>
                <cc1:FSLinkButton ID="lnkbtnModify" runat="server" CssClass="a_edit" OnClick="lnkbtnModify_Click"
                    Enabled="true">修改</cc1:FSLinkButton>
                <cc1:FSLinkButton ID="lnkbtnDelete" runat="server" CssClass="a_delete" OnClientClick="return DelConfirm()"
                    OnClick="lnkbtnDelete_Click" Enabled="False">删除</cc1:FSLinkButton>
            </div>
            <table>
                <tr>
                    <td style="width: 60px">
                        <span style="color: Red">*</span><span id="Label1" class="label">年份：</span>
                    </td>
                    <td style="width: 100px">
                        <cc1:FSDropDownList ID="ddlFinanceYear" runat="server" Width="80px" CssClass="dropdownlist_yellow">
                            <asp:ListItem Selected="True" Value="2013">2013</asp:ListItem>
                            <asp:ListItem Value="2014">2014</asp:ListItem>
                        </cc1:FSDropDownList>
                    </td>
                    <td style="width: 60px">
                        <span id="Span1" class="label">处室：</span>
                    </td>
                    <td style="width: 135px">
                        <cc1:FSDropDownList ID="ddlDept" runat="server" Width="120px" CssClass="dropdownlist_yellow">
                        </cc1:FSDropDownList>
                    </td>
                    <td style="width: 85px">
                        <span style="color: Red">*</span><span id="Span2" class="label">差旅费预算：</span>
                    </td>
                    <td style="width: 140px">
                        <cc1:FSTextBox ID="txtTripBudget" runat="server" MaxLength="50" Width="140px" CssClass="txtbox_yellow"
                            RequiredType="NotNull"></cc1:FSTextBox>
                    </td>
                    <td style="width: 85px">
                        <span style="color: Red">*</span><span id="Span3" class="label">培训费预算：</span>
                    </td>
                    <td style="width: 100px">
                        <cc1:FSTextBox ID="txtTrainingBudget" runat="server" MaxLength="50" Width="140px"
                            CssClass="txtbox_yellow" RequiredType="NotNull"></cc1:FSTextBox>
                    </td>
                    <td style="width: 85px">
                        <span style="color: Red">*</span><span id="Span5" class="label">招待费预算：</span>
                    </td>
                    <td style="width: 100px">
                        <cc1:FSTextBox ID="txtZDBudget" runat="server" MaxLength="50" Width="140px" CssClass="txtbox_yellow"
                            RequiredType="NotNull"></cc1:FSTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                    </td>
                    <td>
                        <span id="Span4" class="label">差旅费已发生值：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtTripUse" runat="server" MaxLength="50" Width="140px" RequiredType="NotNull"></cc1:FSTextBox>
                    </td>
                    <td>
                        <span id="Span7" class="label">培训费已发生值：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtTrainingUse" runat="server" MaxLength="50" Width="140px" RequiredType="NotNull"></cc1:FSTextBox>
                    </td>
                    <td>
                        <span id="Span6" class="label">招待费已发生值：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtZDUse" runat="server" MaxLength="50" Width="140px" RequiredType="NotNull"></cc1:FSTextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div style="width: 100%">
            <cc1:FSGridView ID="gvFinanceFeeList" runat="server" AutoGenerateColumns="False"
                AutoGenerateSelectButton="true" CssClass="table_gv" DataKeyNames="ID" AllowPaging="True"
                AllowSorting="false" PageSize="20" OnSelectedIndexChanged="gvFinanceFeeList_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField HeaderText="年份" DataField="FinanceYear" HeaderStyle-Width="8%">
                        <HeaderStyle CssClass="th_j02" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="处室" DataField="DeptName" HeaderStyle-Width="10%">
                        <HeaderStyle CssClass="th_j02" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="差旅费预算" DataField="TripBudgetCost" HeaderStyle-Width="10%">
                        <HeaderStyle CssClass="th_j02" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="差旅费已发生值" DataField="TripUseCost" HeaderStyle-Width="14%">
                        <HeaderStyle CssClass="th_j02" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="培训费预算" DataField="TrainingBudgetCost" HeaderStyle-Width="10%">
                        <HeaderStyle CssClass="th_j02" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="培训费已发生值" DataField="TrainingUseCost" HeaderStyle-Width="14%" />
                    <asp:BoundField HeaderText="招待费预算" DataField="ZDBudgetCost" HeaderStyle-Width="10%">
                        <HeaderStyle CssClass="th_j02" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="招待费已发生值" DataField="ZDUseCost" HeaderStyle-Width="14%" />
                </Columns>
                <SelectedRowStyle BackColor="#B3D0F5" ForeColor="Black" />
                <RowStyle HorizontalAlign="Center" />
            </cc1:FSGridView>
            <cc1:FSHiddenField ID="hfSelectedIndex" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
