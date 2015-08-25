<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_ReDetail.ascx.cs" Inherits="FS.ADIM.OA.WebUI.WorkFlow.Receive.ReDetail.UC_ReDetail" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="UC_Step.ascx" TagName="UC_Step" TagPrefix="uc" %>
<%@ Register src="../../../PageWF/UC_FileControl.ascx" tagname="FileControlUC" tagprefix="uc" %>
<div class="divSection">
    <div class="divTitle">
        收文基本信息 -
        <%= ProcessTemplate%>
    </div>
    <div>
        <table class="table_add_mid offsetRight" cellpadding="2">
            <tr>
                <td style="width: 75px">
                    <span class="label">单位：</span>
                </td>
                <td colspan="3">
                    <cc1:FSTextBox ID="txtReceiveUnit" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 350px"></cc1:FSTextBox>
                </td>
                <td style="width: 75px">
                    <span class="label">年份：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtReceiveYear" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 141px"></cc1:FSTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 75px">
                    <span class="label">收文号：</span>
                </td>
                <td colspan="3">
                    <cc1:FSTextBox ID="txtReceiveNo" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 350px"></cc1:FSTextBox>
                </td>
                <td style="width: 75px">
                    <span class="label">收文日期：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtReceiveDate" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 141px"></cc1:FSTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 75px">
                    <span class="label">行文号：</span>
                </td>
                <td colspan="3">
                    <cc1:FSTextBox ID="txtSendNo" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 350px"></cc1:FSTextBox>
                </td>
                <td style="width: 75px">
                    <span class="label">行文日期：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtSendDate" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 141px"></cc1:FSTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 75px">
                    <span class="label">文件标题：</span>
                </td>
                <td colspan="6">
                    <cc1:FSTextBox ID="txtDocumentTitle" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 578px"></cc1:FSTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 75px">
                    <span class="label">主题词：</span>
                </td>
                <td colspan="6">
                    <cc1:FSTextBox ID="txtKeyWord" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 578px"></cc1:FSTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 75px">
                    <span class="label">正文页数：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtPageCount" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 141px"></cc1:FSTextBox>
                </td>
                <td style="width: 75px; text-align: center;">
                    <span class="label">份数：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtShareCount" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 130px"></cc1:FSTextBox>
                </td>
                <td style="width: 75px">
                    <span class="label">附件/页数：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtAttchCount" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 141px"></cc1:FSTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 75px">
                    <span class="label">保管期限：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtKeepTime" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 141px"></cc1:FSTextBox>
                </td>
                <td style="width: 75px; text-align: center;">
                    <span class="label">密级：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtSecretLevel" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 130px"></cc1:FSTextBox>
                </td>
                <td style="width: 75px">
                    <span class="label">紧急程度：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtUrgentDegree" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 141px"></cc1:FSTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 75px">
                    <span class="label">来文单位：</span>
                </td>
                <td colspan="3">
                    <cc1:FSTextBox ID="txtSendUnit" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 350px"></cc1:FSTextBox>
                </td>
                <td style="width: 75px">
                    <span class="label">预立卷号：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtPreVolumnNo" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 141px"></cc1:FSTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 75px">
                    <span class="label">备注：</span>
                </td>
                <td colspan="3">
                    <cc1:FSTextBox ID="txtRemark" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 350px"></cc1:FSTextBox>
                </td>
                <td style="width: 75px">
                    <span class="label">归档状态：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtArchiveStatus" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 141px"></cc1:FSTextBox>
                    <cc1:FSCheckBox ID="chkIsArchive" runat="server" Text="直接归档" Enabled="false" />
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
    </div>
</div>
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
