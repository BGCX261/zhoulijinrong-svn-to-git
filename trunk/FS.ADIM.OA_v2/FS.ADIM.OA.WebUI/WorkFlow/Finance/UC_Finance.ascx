<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Finance.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.Finance.UC_Finance" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../../PageWF/UC_Print.ascx" TagName="UCPrint" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_FileControl.ascx" TagName="FileControl" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_OASelect.ascx" TagName="OASelectUC" TagPrefix="uc" %>
<style type="text/css">
    .txtbox_yellow
    {
    }
</style>
<script language="javascript" type="text/javascript">
    function checkChaoSong() {
        if ($('<%= txtChaoSong.ClientID %>').value == "") {
            return confirm("没有选择抄送,是否继续？");
        } else {
            return true;
        }
    }


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
                                <span id="FileTitle" class="label_title_darkblue">
                                    <%=FileTitle %>
                                </span>
                            </td>
                        </tr>
                    </table>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span style="color: Red;">*</span><span class="label_title_bold">主 题:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtZhuTi" CssClass="txtbox_yellow" runat="server" Style="width: 584px;"
                                    MaxLength="50">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">申请人:</span>
                            </td>
                            <td style="width: 150px">
                                <cc1:FSTextBox ID="txtNiGaoRen" CssClass="txtbox_bgreen" runat="server" Width="140px"
                                    ReadOnly="true">
                                </cc1:FSTextBox>
                                <asp:Label ID="lbNiGaoRen" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                            <td style="width: 60px">
                                <span class="label_title_bold">日期:</span>
                            </td>
                            <td style="width: 150px">
                                <cc1:FSTextBox ID="lblNiGaoRiQi" CssClass="txtbox_bgreen" runat="server" Width="140px"
                                    ReadOnly="true">
                                </cc1:FSTextBox>
                            </td>
                            <td style="width: 70px;">
                                <span class="label_title_bold">编号:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtBianHao" CssClass="txtbox_bgreen" runat="server" Width="145px"
                                    ReadOnly="true">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">职务:</span>
                            </td>
                            <td style="width: 150px">
                                <cc1:FSDropDownList ID="drpZhiWu" runat="server" CssClass="dropdownlist_yellow" MaxLength="40"
                                    ReadOnly="True" Width="140px">
                                </cc1:FSDropDownList>
                                <asp:Label ID="lblZhiWu" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                            <td style="width: 60px">
                                <span class="label_title_bold">职称:</span>
                            </td>
                            <td style="width: 150px">
                                <cc1:FSDropDownList ID="drpZhiCheng" runat="server" CssClass="dropdownlist_yellow"
                                    MaxLength="40" ReadOnly="True" Width="140px">
                                </cc1:FSDropDownList>
                                <asp:Label ID="lblZhiCheng" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                            <td style="width: 70px;">
                                <span class="label_title_bold">申请部门:</span>
                            </td>
                            <td>
                                <cc1:FSDropDownList ID="ddlBianZhiBuMen" runat="server" CssClass="dropdownlist_yellow"
                                    Width="150px" DataTextField="Name" DataValueField="ID" AutoPostBack="true" OnSelectedIndexChanged="ddlBianZhiBuMen_SelectedIndexChanged">
                                </cc1:FSDropDownList>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">同行人员:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtTongXing" CssClass="txtbox_yellow" runat="server" Style="width: 584px;
                                    height: 30px" TextMode="MultiLine" MaxLength="2000">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span style="color: Red;">*</span><span class="label_title_bold">出发日期:</span>
                            </td>
                            <td style="width: 150px;">
                                <cc1:FSCalendar ID="timeChuFa" runat="server" Width="120px" />
                                <asp:Label ID="lblChuFaTime" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                            <td style="width: 60px;">
                                <span style="color: Red;">*</span><span class="label_title_bold">回程日期:</span>
                            </td>
                            <td style="width: 150px;">
                                <cc1:FSCalendar ID="timeHuiCheng" runat="server" Width="120px" />
                                <asp:Label ID="lblHuiCheng" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                            <td style="width: 60px;">
                                <span style="color: Red;">*</span><span class="label_title_bold">目的地:</span>
                            </td>
                            <td style="width: 160px;">
                                <cc1:FSTextBox ID="txtDestination" CssClass="txtbox_yellow" runat="server" Style="width: 135px;"
                                    MaxLength="50">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span style="color: Red;">*</span><span class="label_title_bold">出差任务:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtChuChaiRenWu" CssClass="txtbox_yellow" runat="server" Style="width: 584px;
                                    height: 185px" TextMode="MultiLine" MaxLength="2000">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span style="color: Red;">*</span><span class="label_title_bold">差旅费预算:</span>
                            </td>
                            <td style="width: 150px;">
                                <cc1:FSTextBox RequiredType="Money" ID="txtFeeYuSuan" CssClass="txtbox_yellow" runat="server"
                                    Width="150px"></cc1:FSTextBox>
                            </td>
                            <td style="width: 85px;">
                                <span style="color: Red;">*</span><span class="label_title_bold">已发生值：</span>
                            </td>
                            <td>
                                <cc1:FSTextBox RequiredType="Numeric" ID="txtFeeFaSheng" CssClass="txtbox_yellow"
                                    runat="server" Width="150px"></cc1:FSTextBox>
                            <span class="">(单位:元)</span>
                            </td>
                            <td style="text-align:left;font-size:12px;width: 85px;">
                                
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
                                <span class="label_title_bold">处领导:</span>
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
                                <span class="label_title_bold">审批意见:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtShenPiYiJian" CssClass="txtbox_bgreen" runat="server" Style="width: 584px;
                                    height: 60px" TextMode="MultiLine" ReadOnly="true">
                                </cc1:FSTextBox>
                                <asp:Label ID="lblShenPiRen" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">商务信息:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtShangWu" CssClass="txtbox_bgreen" runat="server" Style="width: 584px;
                                    height: 60px" TextMode="MultiLine" ReadOnly="true">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">抄 送:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtChaoSong" CssClass="txtbox_yellow" TextMode="MultiLine" runat="server"
                                    Style="width: 584px; height: 30px">
                                </cc1:FSTextBox>
                                <uc:OASelectUC ID="OASelectUC1" runat="server" Visible="false" />
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr id="TrChuanYue" runat="server" visible="false">
                            <td class="LeftSpan">
                                <span class="label_title_bold">分发范围:</span>
                            </td>
                            <td colspan="7">
                                <cc1:FSTextBox ID="txtChuanYueRenYuan" CssClass="txtbox_yellow" TextMode="MultiLine"
                                    runat="server" Style="width: 584px; height: 30px">
                                </cc1:FSTextBox><uc:OASelectUC ID="OASelectUC2" runat="server" Visible="false" />
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
                    <div style="display: none; border-bottom: 1px #ccc dashed">
                        <table>
                            <tr>
                                <td class="LeftSpan">
                                    <span class="label_title_bold">部门负责人:</span>
                                </td>
                                <td style="width: 80px;">
                                    <cc1:FSDropDownList ID="ddlFuZeRen" runat="server" CssClass="dropdownlist_yellow"
                                        Width="75px">
                                    </cc1:FSDropDownList>
                                    <asp:Label ID="lbLeader" runat="server" Text="" Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
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
                        <cc1:FSButton ID="btnTiJiaoShenHe" runat="server" Text="提交审核" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" Visible="false" />
                        <cc1:FSButton ID="btnShenHeWanCheng" runat="server" CssClass="btn" OnClientClick="DisableButtons()"
                            Text="审核完成" UseSubmitBehavior="false" Visible="false" />
                        <cc1:FSButton ID="btnDingPiao" runat="server" CssClass="btn" OnClientClick="DisableButtons()"
                            Text="订票完成" UseSubmitBehavior="false" Visible="false" />
                        <cc1:FSButton ID="btnApplyComplete" runat="server" CssClass="btn" Text="完成" OnClientClick="DisableButtons()" UseSubmitBehavior="false"
                            Visible="false" />
                        <cc1:FSButton ID="btnSave" runat="server" Text="保存" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" Visible="false" />
                        <cc1:FSButton ID="btnTuiHui" runat="server" CssClass="btn" OnClientClick="DisableButtons()"
                            Text="退回" UseSubmitBehavior="false" Visible="false" />
                        <cc1:FSButton ID="btnCancel" runat="server" Text="撤销" CssClass="btn" UseSubmitBehavior="false"
                            Visible="false" />
                        <cc1:FSButton ID="btnZhuiJiaFenFa" runat="server" CssClass="btn" OnClientClick="DisableButtons()"
                            Text="追加分发" UseSubmitBehavior="false" Visible="false" />
                        <cc1:FSButton ID="btnWanChengGuiDang" runat="server" CssClass="btn" Text="完成归档" UseSubmitBehavior="false"
                            Visible="false" />
                        <cc1:FSButton ID="btn_GuiDang" runat="server" CssClass="btn" Text="归档" UseSubmitBehavior="false"
                            Visible="false" OnClick="btn_GuiDang_Click" />
                        <uc:UCPrint ID="ucPrint" runat="server" />
                        <cc1:FSButton ID="btnTongGuoShenHe" runat="server" CssClass="btn" OnClientClick="DisableButtons()"
                            Text="处领导审核" UseSubmitBehavior="false" Visible="false" />
                        <cc1:FSButton ID="btnZhuGuan" runat="server" CssClass="btn" Text="提交主管领导" UseSubmitBehavior="false"
                            Visible="false" />
                        <cc1:FSButton ID="btnZongJingLi" runat="server" Text="提交总经理" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" Visible="false" />
                    </div>
                </div>
            </td>
            <td id="TdYiJian" valign="top" runat="server" visible="false" style="width: 185px;
                text-align: left">
                <div style="height: 20px; padding: 10px 0px; background: #deeef4; border: #64b7d7 1px solid;
                    text-align: center; font-weight: bold; width: 100%">
                    领导批示</div>
                <div style="border: 1px solid #64b7d7; text-align: center; width: 100%">
                    <table style="width: 100%; font-size: 12px; text-align: center;">
                        <tr>
                            <td style="text-align: left">
                                提示信息：
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <cc1:FSTextBox ID="txtBanShuiXinXi" runat="server" CssClass="txtbox_bgreen" Height="150px"
                                    TextMode="MultiLine" Width="165px" ReadOnly="true">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                批示意见：
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <cc1:FSTextBox ID="txtPiShiYiJian" runat="server" CssClass="txtbox_yellow" Height="150px"
                                    TextMode="MultiLine" Width="165px" MaxLength="2000">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div class="divSubmit">
                        <cc1:FSButton ID="btnComplete" runat="server" class="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons(this)" Text="批示完成" />
                        <cc1:FSButton ID="btnSavePishi" runat="server" class="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons(this)" Text="保存" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
<div style="display: none">
    <cc1:FSTextBox ID="txtIsBack" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtZhuSongID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtChaoSongID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtChaoSongDeptID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtChaoSongUserID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtNiGaoRenID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtHeGaoRenID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtKeShiLingDaoID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtKeShiLingDao" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtChengBanRen" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtChengBanRenID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtChengBanBuMenID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtChuanYueRenIDs" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtChuanYueDeptIDs" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtBuMenLingDaoID" runat="server">
    </cc1:FSTextBox>
    <asp:TextBox ID="txtRequestCategory" runat="server"></asp:TextBox>
    <cc1:FSTextBox ID="txtGeneralManagerID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtChargeLeaderID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtDepartmentLeaderID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtBookingOfficeID" runat="server">
    </cc1:FSTextBox>
</div>
<span id="idShow"></span>
<input type="hidden" id="IsMoTai" value="true" />
<input type="hidden" id="IsDrag" value="true" />
<input type="hidden" id="IsMiddle" value="true" />
<input type="hidden" id="IsResize" value="true" />
<input type="hidden" id="imgLoading" value="true" />
<asp:Label ID="lbJs" runat="server"></asp:Label>