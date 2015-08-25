<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_HSReDetail.ascx.cs" Inherits="FS.ADIM.OA.WebUI.WorkFlow.Receive.ReDetail.UC_HSReDetail" %>

<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register src="../../../PageWF/UC_FileControl.ascx" tagname="FileControlUC" tagprefix="uc" %>
<%@ Register Src="UC_Step.ascx" TagName="UC_Step" TagPrefix="uc" %>
<table class="table_add_mid offsetRight" cellpadding="2">
    <tr>
        <td style="width: 73px">
            <span class="label">单位：</span>
        </td>
        <td colspan="5">
            <cc1:FSTextBox ID="txtReceiveUnit" runat="server" CssClass="txtPreview" ReadOnly="true"
                TabIndex="-1" Style="width: 691px"></cc1:FSTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label">文件标题 ：</span>
        </td>
        <td colspan="5">
            <cc1:FSTextBox ID="txtDocumentTitle" runat="server" CssClass="txtPreview" ReadOnly="true"
                TabIndex="-1" Style="width: 691px"></cc1:FSTextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 73px;">
            <span class="label">收文号：</span>
        </td>
        <td style="width: 145px;">
            <cc1:FSTextBox ID="txtDocumentNo" runat="server" CssClass="txtPreview" ReadOnly="true"
                Style="width: 141px" TabIndex="-1"></cc1:FSTextBox>
        </td>
        <td style="width: 73px;">
            <span class="label">文件编码：</span>
        </td>
        <td style="width: 145px;">
            <cc1:FSTextBox ID="txtDocumentEncoding" runat="server" CssClass="txtPreview" ReadOnly="true"
                TabIndex="-1" Style="width: 141px"></cc1:FSTextBox>
        </td>
        <td style="width: 73px;">
            <span class="label Mandatory">收文日期：</span>
        </td>
        <td style="width: 145px;">
            <cc1:FSTextBox ID="txtReceiveDate" runat="server" CssClass="txtPreview" ReadOnly="true"
                TabIndex="-1" Style="width: 141px" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="label">答复文号：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtReplyDocumentNo" runat="server" CssClass="txtPreview" ReadOnly="true"
                TabIndex="-1" Style="width: 141px" MaxLength="50"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label">其他编码：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtOtherEncoding" runat="server" CssClass="txtPreview" ReadOnly="true"
                TabIndex="-1" Style="width: 141px" MaxLength="50"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label">行文日期：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtXingWenDate" runat="server" CssClass="txtPreview" ReadOnly="true"
                TabIndex="-1" Style="width: 141px" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="label">函件类型：</span>
        </td>
        <td colspan="3">
            <cc1:FSTextBox ID="txtDocumentType" runat="server" CssClass="txtPreview" ReadOnly="true"
                TabIndex="-1" Style="width: 416px"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label">形成日期 ：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtFormationDate" runat="server" CssClass="txtPreview" ReadOnly="true"
                TabIndex="-1" Style="width: 141px" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="label">紧急程度：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtUrgentDegree" runat="server" CssClass="txtPreview" ReadOnly="true"
                TabIndex="-1" Style="width: 141px"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label">页数：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtPageCount" runat="server" CssClass="txtPreview" ReadOnly="true"
                TabIndex="-1" Style="width: 141px"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label">保管期限：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtKeepTime" runat="server" CssClass="txtPreview" ReadOnly="true"
                TabIndex="-1" Style="width: 141px"></cc1:FSTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label">对应合同号：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtContractNumber" runat="server" CssClass="txtPreview" ReadOnly="true"
                TabIndex="-1" Style="width: 141px"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label">设备代码：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtEquipmentCode" runat="server" CssClass="txtPreview" ReadOnly="true"
                TabIndex="-1" Style="width: 141px"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label">HN编码 ：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtHNCode" runat="server" CssClass="txtPreview" ReadOnly="true"
                TabIndex="-1" Style="width: 141px"></cc1:FSTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label">备注：</span>
        </td>
        <td colspan="5">
            <cc1:FSTextBox ID="txtRemark" runat="server" CssClass="txtPreview" ReadOnly="true"
                TabIndex="-1" Style="width: 691px" Height="58px" TextMode="MultiLine"></cc1:FSTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label">附件：</span>
        </td>
        <td colspan="5">
            <uc:FileControlUC ID="ucAttachment" runat="server" />
        </td>
    </tr>
</table>
<div class="divSection">
    <div class="divTitle">
        流程信息
    </div>
    <div id="divPrompt" visible="false" runat="server" style="height: 25px; line-height: 25px;
        border: dashed 2px #333; font-size: 12px; font-weight: bold; text-align: center;
        margin: 5px 60px; background-color: ButtonShadow">
        无流程信息</div>
    <asp:Repeater ID="rptProcessDetail" runat="server" OnItemDataBound="rptProcessDetail_ItemDataBound">
        <ItemTemplate>
            <uc:UC_Step ID="ucStep" runat="server" />
        </ItemTemplate>
    </asp:Repeater>
</div>
<div class="divSection">
    <div class="divTitle">
        传阅
    </div>
    <table class="table_add_mid offsetRight" cellpadding="2">
        <tr>
            <td style="width: 75px">
                <span class="label">传阅情况：</span>
            </td>
            <td style="text-align: left;">
                <cc1:FSGridView CssClass="table_gv tblStyle" ID="gdvCirculate" runat="server" AutoGenerateColumns="False"
                    AllowPaging="True" BorderWidth="1px" CellPadding="3">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="150px" HeaderText="传阅人员">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "ReceiveUserID")%>
                                <%# DataBinder.Eval(Container.DataItem, "ReceiveUserName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="传阅日期" DataField="SendDateTime" HeaderStyle-Width="120px" />
                        <asp:BoundField HeaderText="传阅意见" DataField="Comment" HeaderStyle-Width="210px" />
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="状态">
                            <ItemTemplate>
                                <%# ((Boolean)DataBinder.Eval(Container.DataItem, "Is_Read")) == true ? "已阅" : "未阅"%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="td_j02" Height="20px" />
                    <HeaderStyle CssClass="th_j02" Height="20px" />
                </cc1:FSGridView>
            </td>
        </tr>
    </table>
</div>
<div class="divSubmit" id="divSubmit" style="margin-left: 32px;">
    <cc1:FSButton ID="btnReturn" runat="server" CssClass="btn" Text="返回" OnClick="btnReturn_Click" />
</div>
