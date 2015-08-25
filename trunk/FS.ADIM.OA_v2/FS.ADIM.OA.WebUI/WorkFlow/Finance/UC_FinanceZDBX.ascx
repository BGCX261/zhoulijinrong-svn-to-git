<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_FinanceZDBX.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.Finance.UC_FinanceZDBX" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../../PageWF/UC_Print.ascx" TagName="UCPrint" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_FileControl.ascx" TagName="FileControl" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_OASelect.ascx" TagName="OASelectUC" TagPrefix="uc" %>
<script language="javascript" type="text/javascript">
    $("containTitle").innerHTML = "<%=TemplateName%> - " + "<%=StepName%>";
</script>
<div class="divCenter">
    <table>
        <tr>
            <td style="text-align: left">
                <div style="width: 720px; margin: 0px auto; border: #000000 1px solid;">
                    <table style="text-align: center; height: 50px; width: 100%">
                        <tr>
                            <td style="background-color: #DaDaDa">
                                <span id="FileTitle" class="label_title_darkblue">海南核电有限公司招待费报销申请单 </span>
                            </td>
                        </tr>
                    </table>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">报销单编号:</span>
                            </td>
                            <td style="width: 150px">
                                <cc1:FSTextBox ID="txtDocumentNo" CssClass="txtbox_bgreen" runat="server" Width="140px"
                                    ReadOnly="true">
                                </cc1:FSTextBox>
                            </td>
                            <td style="width: 60px">
                                <span class="label_title_bold">日期:</span>
                            </td>
                            <td style="width: 150px">
                                <cc1:FSTextBox ID="txtNiGaoRiQi" CssClass="txtbox_bgreen" runat="server" Width="140px"
                                    ReadOnly="true">
                                </cc1:FSTextBox>
                            </td>
                            <td style="width: 70px;">
                                <span style="color: Red;">*</span> <span class="label_title_bold">单据张数:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtDanJuZhangShu" RequiredType="Money" CssClass="txtbox_yellow"
                                    runat="server" Width="145px">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">报销人姓名:</span>
                            </td>
                            <td style="width: 150px">
                                <cc1:FSTextBox ID="txtNiGaoRen" CssClass="txtbox_bgreen" runat="server" Width="140px"
                                    ReadOnly="true">
                                </cc1:FSTextBox>
                                <asp:Label ID="lblNiGaoRen" runat="server" Text=""></asp:Label>
                                <cc1:FSTextBox ID="txtNiGaoRenID" runat="server" Text="" Visible="false"></cc1:FSTextBox>
                            </td>
                            <td style="width: 60px">
                                <span class="label_title_bold">部门:</span>
                            </td>
                            <td style="width: 150px">
                                <cc1:FSDropDownList ID="ddlDepartment" runat="server" Width="134px" DataTextField="Name"
                                    DataValueField="ID" AutoPostBack="true" CssClass="dropdownlist_yellow">
                                </cc1:FSDropDownList>
                            </td>
                            <td colspan="2">
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span style="color: Red;">*</span><span class="label_title_bold">用途:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtYongTu" CssClass="txtbox_yellow" runat="server" TextMode="MultiLine"
                                    MaxLength="2000" Height="38px" Width="587px"></cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span style="color: Red;">*</span><span class="label_title_bold">收款单位名称:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtShouKuanDanWei" CssClass="txtbox_yellow" runat="server" Width="580px"></cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span style="color: Red;">*</span><span class="label_title_bold">收款开户银行:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtShouKuanYinHang" runat="server" Width="580px" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="LeftSpan">
                                <span style="color: Red;">*</span><span class="label_title_bold">收款单位账号:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtShouKuanZhangHao" CssClass="txtbox_yellow" runat="server" MaxLength="600"
                                    Width="580px"></cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span style="color: Red;">*</span><span class="label_title_bold">报销金额大写:</span>
                            </td>
                            <td style="width: 150px;">
                                <cc1:FSTextBox ID="txtBaoXiaoJinEDaXie" CssClass="txtbox_yellow" runat="server" Width="150px"></cc1:FSTextBox>
                            </td>
                            <td style="width: 50px;">
                                <span style="color: Red;">*</span><span class="label_title_bold">￥：</span>
                            </td>
                            <td>
                                <cc1:FSTextBox RequiredType="Money" ID="txtBaoXiaoJinE" CssClass="txtbox_yellow"
                                    runat="server" Width="150px"></cc1:FSTextBox>
                            </td>
                            <td style="width: 100px;">
                                <span style="color: Red;">*</span><span class="label_title_bold">累计报销金额:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtLeiJiBaoXiaoJinE" RequiredType="Money" CssClass="txtbox_yellow"
                                    runat="server" Style="width: 120px;" MaxLength="50">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span style="color: Red;">*</span><span class="label_title_bold">宴请人数:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtYanQingRenShu" CssClass="txtbox_yellow" runat="server" Style="width: 120px;"
                                    MaxLength="50">
                                </cc1:FSTextBox>
                            </td>
                            <td class="LeftSpan">
                                <span style="color: Red;">*</span><span class="label_title_bold">人均消费数额:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtRenJunXiaoFeiE" RequiredType="Money" CssClass="txtbox_yellow"
                                    runat="server" Style="width: 120px;" MaxLength="50">
                                </cc1:FSTextBox>
                            </td>
                            <td style="width: 100px;">
                                <span style="color: Red;">*</span><span class="label_title_bold">预算金额:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtYuSuanJinE" RequiredType="Money" CssClass="txtbox_yellow" runat="server"
                                    Style="width: 120px;" MaxLength="50">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">总经理:</span>
                            </td>
                            <td style="width: 150px;">
                                <cc1:FSDropDownList ID="ddlZongJingLi" runat="server" Width="140px" CssClass="dropdownlist_yellow">
                                </cc1:FSDropDownList>
                            </td>
                            <td style="width: 70px;">
                                <span class="label_title_bold">主管领导:</span>
                            </td>
                            <td style="width: 150px;">
                                <cc1:FSDropDownList ID="ddlZhuGuanLingDao" runat="server" Width="140px" CssClass="dropdownlist_yellow">
                                </cc1:FSDropDownList>
                            </td>
                            <td style="width: 70px;">
                                <span style="color: Red;">*</span><span class="label_title_bold">处领导:</span>
                            </td>
                            <td>
                                <cc1:FSDropDownList ID="ddlChuLingDao" runat="server" Width="140px" CssClass="dropdownlist_yellow">
                                </cc1:FSDropDownList>
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
                                <span style="color: Red;">*</span><span class="label_title_bold">财务:</span>
                            </td>
                            <td>
                                <cc1:FSDropDownList ID="ddlJingShouRen" runat="server" Width="140px" CssClass="dropdownlist_yellow">
                                </cc1:FSDropDownList>
                            </td>
                            <td style="width: 70px;">
                            </td>
                            <td style="width: 150px;">
                            </td>
                        </tr>
                    </table>
                    <table class="SheetContent" style="display: none">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">抄 送:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtChaoSong" CssClass="txtbox_bgreen" TextMode="MultiLine" runat="server"
                                    Style="width: 584px; height: 30px">
                                </cc1:FSTextBox>
                                <uc:OASelectUC ID="OASelectUC1" runat="server" Visible="false" />
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table id="TaTishi" visible="true" runat="server" class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">提示信息:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtTiShiXinXi" CssClass="txtbox_bgreen" runat="server" Style="width: 298px;
                                    height: 60px;" TextMode="MultiLine" ReadOnly="true" TabIndex="-1">
                                </cc1:FSTextBox>
                                <cc1:FSTextBox ID="txtTianJia" CssClass="txtbox_yellow" runat="server" Style="width: 276px;
                                    height: 60px;" TextMode="MultiLine">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">附件：</span>
                            </td>
                            <td>
                                <uc:FileControl ID="ucAttachment" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <div class="divSubmit" id="divSubmit">
                        <cc1:FSButton ID="btnSave" runat="server" Text="保存" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" />
                        <cc1:FSButton ID="btnTiJiaoShenHe" runat="server" Text="提交审核" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" Visible="false" />
                        <cc1:FSButton ID="btnShenHeWanCheng" runat="server" CssClass="btn" OnClientClick="DisableButtons()"
                            Text="审核完成" UseSubmitBehavior="false" Visible="false" />
                        <cc1:FSButton ID="btnApplyComplete" runat="server" CssClass="btn" Text="完成" OnClientClick="DisableButtons()"
                            UseSubmitBehavior="false" Visible="false" />
                        <cc1:FSButton ID="btnTuiHui" runat="server" CssClass="btn" OnClientClick="DisableButtons()"
                            Text="退回" UseSubmitBehavior="false" Visible="false" />
                        <cc1:FSButton ID="btnCancel" runat="server" Text="撤销" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" ToolTip="撤销" Visible="false" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
<div style="display: none">
    <cc1:FSTextBox ID="FSTextBox1" runat="server"></cc1:FSTextBox>
    <cc1:FSTextBox ID="txtIsBack" runat="server"></cc1:FSTextBox>
    <cc1:FSTextBox ID="txtIsManager" runat="server"></cc1:FSTextBox>
     <cc1:FSTextBox ID="txtIsChaoYuSuan" runat="server"></cc1:FSTextBox>
    <cc1:FSTextBox ID="txtType" runat="server"></cc1:FSTextBox>
    <cc1:FSTextBox ID="txtChuShiID" runat="server"></cc1:FSTextBox>
</div>
