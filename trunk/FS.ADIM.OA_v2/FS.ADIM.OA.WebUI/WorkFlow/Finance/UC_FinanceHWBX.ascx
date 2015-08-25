<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_FinanceHWBX.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.Finance.UC_FinanceHWBX" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../../PageWF/UC_Print.ascx" TagName="UCPrint" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_FileControl.ascx" TagName="FileControl" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_OASelect.ascx" TagName="OASelectUC" TagPrefix="uc" %>
<%@ Register Src="UC_CCBXDetail.ascx" TagName="UC_CCBXDetail" TagPrefix="uc" %>
<div class="divCenter">
    <table>
        <tr>
            <td style="text-align: left">
                <div style="width: 720px; margin: 0px auto; border: #000000 1px solid;">
                    <table style="text-align: center; height: 50px; width: 100%">
                        <tr>
                            <td style="background-color: #DaDaDa">
                                <span id="FileTitle" class="label_title_darkblue">
                                    <cc1:FSLabel ID="lblFileTitle" runat="server" Text="海南核电有限公司会务费用报销单"></cc1:FSLabel>
                                </span>
                            </td>
                        </tr>
                    </table>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">报销单编号:</span>
                            </td>
                            <td style="width: 160px">
                                <cc1:FSTextBox ID="txtDocumentNo" runat="server" Width="120px" CssClass="txtbox_yellow"
                                    AutoPostBack="True"></cc1:FSTextBox>
                            </td>
                            <td style="width: 70px;">
                                <span class="label_title_bold">日期:</span>
                            </td>
                            <td style="width: 150px">
                                <cc1:FSTextBox ID="txtNiGaoRiQi" runat="server" Width="130px" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                            <td style="width: 70px">
                                <span class="label_title_bold">单据张数:</span>
                            </td>
                            <td style="width: 140px">
                                <cc1:FSTextBox ID="txtDanJuZhangShu" runat="server" Width="130px" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">申请报销人:</span>
                            </td>
                            <td style="width: 160px">
                                <cc1:FSTextBox ID="txtNiGaoRen" runat="server" Width="130px" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                            <td style="width: 70px;">
                                <span class="label_title_bold">部门:</span>
                            </td>
                            <td style="width: 150px">
                                <cc1:FSDropDownList ID="ddlBianZhiBuMen" runat="server" Width="134px" DataTextField="Name"
                                    DataValueField="ID" AutoPostBack="true" CssClass="dropdownlist_yellow">
                                </cc1:FSDropDownList>
                            </td>
                            <td style="width: 70px;">
                                <span class="label_title_bold"></span>
                            </td>
                            <td style="width: 140px">
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">用途:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtYongTu" runat="server" Style="width: 600px; height: 30px" TextMode="MultiLine"
                                    MaxLength="2000" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">收款单位名称:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtShouKuanDanWei" runat="server" Width="400px" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">收款单位开户银行:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtShouKuanYinHang" runat="server" Width="400px" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">收款单位帐号:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtShouKuanZhangHao" runat="server" CssClass="txtbox_yellow" Width="200px">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">报销金额(大写):</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtBaoXiaoJinEDaXie" runat="server" Width="200px" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                            <td>
                                <span class="label_title_bold">￥:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtBaoXiaoJinE" runat="server" CssClass="txtbox_yellow" Width="200px">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <table class="SheetContent" id="tablePeiXun1" runat="server">
                        <tr>
                            <td class="LeftSpan" >
                                <span class="label_title_bold">立项号:</span>
                            </td>
                            <td style="width: 150px;">
                                <cc1:FSTextBox ID="txtLiXiangHao" runat="server" Width="140px" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                            <td style="width: 90px;">
                                <span class="label_title_bold">立项/预算金额:</span>
                            </td>
                            <td style="width: 150px;">
                                <cc1:FSTextBox ID="txtLiXiangJinE" runat="server" Width="100px" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                            <td style="width: 90px;">
                                <span class="label_title_bold">累计报销金额:</span>
                            </td>
                            <td style="width: 120px;">
                                <cc1:FSTextBox ID="txtLiXiangLeiJiJinE" runat="server" Width="100px" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">验收人:</span>
                            </td>
                            <td style="width: 150px;">
                                <cc1:FSDropDownList ID="ddlYanShouRen" runat="server" Width="140px" CssClass="dropdownlist_yellow">
                                </cc1:FSDropDownList>
                            </td>
                            <td style="width: 70px;">
                                <span class="label_title_bold">立项审签:</span>
                            </td>
                            <td style="width: 180px;">
                                <cc1:FSDropDownList ID="ddlShenQianRen" runat="server" Width="140px" CssClass="dropdownlist_yellow">
                                </cc1:FSDropDownList>
                            </td>
                            <td style="width: 60px;">
                                <span class="label_title_bold">审核：</span>
                            </td>
                            <td>
                                <cc1:FSDropDownList ID="ddlShenHe" runat="server" Width="140px" CssClass="dropdownlist_yellow">
                                </cc1:FSDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">审批:</span>
                            </td>
                            <td style="width: 150px;">
                                <cc1:FSDropDownList ID="ddlShenPi" runat="server" Width="140px" CssClass="dropdownlist_yellow">
                                </cc1:FSDropDownList>
                            </td>
                            <td style="width: 70px;">
                                <span class="label_title_bold">财务:</span>
                            </td>
                            <td style="width: 180px;">
                                <cc1:FSDropDownList ID="ddlCaiWu" runat="server" Width="140px" CssClass="dropdownlist_yellow">
                                </cc1:FSDropDownList>
                            </td>
                            <td style="width: 60px;">
                                <span class="label_title_bold"></span>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">提示信息:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtTiShiXinXi" CssClass="txtbox_bgreen" Style="width: 298px; height: 60px;"
                                    runat="server" TextMode="MultiLine" ReadOnly="true" TabIndex="-1">
                                </cc1:FSTextBox>
                                <cc1:FSTextBox ID="txtTianJia" CssClass="txtbox_yellow" Style="width: 276px; height: 60px;"
                                    runat="server" TextMode="MultiLine">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <div class="divSubmit" id="divSubmit">
                        <cc1:FSButton ID="btnSave" runat="server" CssClass="btn" Text="保存" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" ToolTip="保存" />
                        <cc1:FSButton ID="btnYanShou" runat="server" CssClass="btn_long" Text="提交验收" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" ToolTip="提交验收" />
                        <cc1:FSButton ID="btnShenQian" runat="server" CssClass="btn_long" Text="提交立项审签" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" ToolTip="提交审签" />
                        <cc1:FSButton ID="btnTongYi" runat="server" CssClass="btn_long" Text="审核通过" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" ToolTip="审核通过" />
                        <cc1:FSButton ID="btnShenPi" runat="server" CssClass="btn_long" Text="提交审批" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" ToolTip="提交审批" />
                        <cc1:FSButton ID="btnShenHe" runat="server" CssClass="btn_long" Text="提交审核" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" ToolTip="提交审核" />
                        <cc1:FSButton ID="btnCaiWu" runat="server" CssClass="btn" Text="提交财务" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" ToolTip="提交财务审核" />
                            <cc1:FSButton ID="btnWanCheng" runat="server" CssClass="btn" Text="完成" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" ToolTip="完成" />
                        <cc1:FSButton ID="btnBack" runat="server" CssClass="btn_long" Text="退回" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" ToolTip="退回" />

                        <cc1:FSButton ID="btnCancel" runat="server" Text="撤销" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" ToolTip="撤销" />
                        <uc:UCPrint ID="ucPrint" runat="server" Visible="false" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
<div style="display: none">
    <cc1:FSTextBox ID="txtNiGaoRenID" runat="server"></cc1:FSTextBox>
    <cc1:FSTextBox ID="txtIsBack" runat="server"></cc1:FSTextBox>
    <cc1:FSTextBox ID="txtType" runat="server"></cc1:FSTextBox>
    <cc1:FSTextBox ID="txtChuShiID" runat="server"></cc1:FSTextBox>
    <asp:Label ID="lbJs" runat="server"></asp:Label>
    <br />
    <br />
    <br />
</div>
