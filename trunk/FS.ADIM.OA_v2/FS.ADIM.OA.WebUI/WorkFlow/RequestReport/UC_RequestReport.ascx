<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_RequestReport.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.RequestReport.UC_RequestReport" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../../PageWF/UC_Print.ascx" TagName="UCPrint" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_FileControl.ascx" TagName="FileControl" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_OASelect.ascx" TagName="OASelectUC" TagPrefix="uc" %>

<script language="javascript" type="text/javascript">
    function checkChaoSong() {
        if ($('<%= txtChaoSong.ClientID %>').value == "") {
            return confirm("没有选择抄送,是否继续？");
        } else {
            return true;
        }
    }
    function checkFenFa() {
        if ($('<%= txtChuanYueRenYuan.ClientID %>').value == "") {
            return confirm("没有分发范围,是否继续？");
        } else {
            return true;
        }
    }
    function checkChuanYue() {
        if ($('<%= txtDeptName.ClientID %>').value == "" && $('<%= txtUserName.ClientID %>').value == "") {
            return confirm("没有选择传阅,是否继续？");
        } else {
            return true;
        }
    }
    function SelectedChanged() {
        var ddl = $("<%=ddlType.ClientID%>");
        var index = ddl.selectedIndex;
        var Value = ddl.options[index].value;
        var Texts = ddl.options[index].text;
        var content;
        if (Texts == "请示") {
            content = "海南核电有限公司请示";
        } else {
            content = "海南核电有限公司报告";
        }
        $("FileTitle").innerHTML = content;
    }
    function SelectRequestCategory() {
        $('<%= txtRequestCategory.ClientID %>').value = $('<%= ddlType.ClientID %>').value;
    }
    $("containTitle").innerHTML = "<%=TemplateName%> - " + "<%=StepName%>";
</script>

<div id="MainDivID">
</div>
<div id="PopDivID" style="display: none; height: 100px; width: 180px;" class="PopDiv">
    <div class="PopDivTitle">
        请选择：
    </div>
    <table class="table_form_noborder">
        <tr>
            <td style="text-align: right; width: 79px; height: 50px">
                流程类型：
            </td>
            <td style="width: 100px;">
                <cc1:FSDropDownList ID="ddlType" runat="server" CssClass="dropdownlist_green" Width="90px">
                    <asp:ListItem Selected="True" Value="请示">请示</asp:ListItem>
                    <asp:ListItem Value="报告">报告</asp:ListItem>
                </cc1:FSDropDownList>
            </td>
        </tr>
    </table>
    <div style="text-align: center">
        <input type="button" value="确定" class="btn" onclick="SelectRequestCategory();hiddenPopDiv('MainDivID','PopDivID')" />
    </div>
</div>
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
                                <span class="label_title_bold">拟稿日期:</span>
                            </td>
                            <td>
                                <asp:Label ID="lblNiGaoRiQi" runat="server" CssClass="txtPreview"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span style="color: Red;">*</span><span class="label_title_bold">主 送:</span>
                            </td>
                            <td style="width: 150px">
                                <cc1:FSDropDownList ID="ddlZhuSong" runat="server" CssClass="dropdownlist_yellow"
                                    Width="140px" DataTextField="Name" DataValueField="ID">
                                </cc1:FSDropDownList>
                            </td>
                            <td style="width: 60px;">
                                <span class="label_title_bold">编 号:</span>
                            </td>
                            <td style="width: 150px">
                                <cc1:FSTextBox ID="txtBianHao" CssClass="txtbox_bgreen" runat="server" Width="140px"
                                    ReadOnly="true">
                                </cc1:FSTextBox>
                            </td>
                            <td style="width: 70px;">
                                <span class="label_title_bold">编制部门:</span>
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
                                <span class="label_title_bold">抄 送:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtChaoSong" CssClass="txtbox_yellow" TextMode="MultiLine" runat="server"
                                    Style="width: 584px; height: 30px">
                                </cc1:FSTextBox><uc:OASelectUC ID="OASelectUC1" runat="server" Visible="false" />
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
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
                                <span style="color: Red;">*</span><span class="label_title_bold">内 容:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtNeiRong" CssClass="txtbox_yellow" runat="server" Style="width: 584px;
                                    height: 200px" TextMode="MultiLine" MaxLength="2000">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">领导批示:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtLingDaoPiShi" CssClass="txtbox_bgreen" runat="server" Style="width: 584px;
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
                                <span class="label_title_bold">承办部门:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtChengBanBuMen" CssClass="txtbox_bgreen" runat="server" Style="width: 298px;"
                                    ReadOnly="true"></cc1:FSTextBox>
                                <uc:OASelectUC ID="OASelectUC5" runat="server" Visible="false" />
                            </td>
                        </tr>
                    </table>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">承办情况:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtChuLiYiJian" CssClass="txtbox_bgreen" runat="server" Style="width: 584px;
                                    height: 60px" TextMode="MultiLine" ReadOnly="true">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                        <tr id="TrBanLiYiJian" runat="server" visible="false">
                            <td class="LeftSpan">
                                <span class="label_title_bold">办理意见:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtBanLiYiJian" CssClass="txtbox_yellow" runat="server" Style="width: 584px;
                                    height: 60px" TextMode="MultiLine">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <table class="SheetContent">
                        <tr id="TrChenBan" runat="server" visible="false">
                            <td class="LeftSpan">
                                <span class="label_title_bold">部门领导:</span>
                            </td>
                            <td style="width: 80px;">
                                <cc1:FSTextBox ID="txtBuMenLingDao" runat="server" CssClass="txtbox_bgreen" Width="70px"
                                    ReadOnly="true">
                                </cc1:FSTextBox>
                                <asp:Label ID="lblDirector" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                            <td style="width: 60px;">
                                <span class="label_title_bold">科室承办:</span>
                            </td>
                            <td style="width: 110px;">
                                <cc1:FSDropDownList ID="ddlKeShiLingDao" CssClass="dropdownlist_yellow" runat="server"
                                    Width="100px" ReadOnly="true">
                                </cc1:FSDropDownList>
                                <asp:Label ID="lblSection" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                            <td style="width: 60px;">
                                <span class="label_title_bold">直接承办:</span>
                            </td>
                            <td style="width: 100px;">
                                <cc1:FSDropDownList ID="ddlChengBanRen" CssClass="dropdownlist_yellow" runat="server"
                                    Width="90px" ReadOnly="true">
                                </cc1:FSDropDownList>
                                <asp:Label ID="lblMember" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                            <td style="width: 60px;">
                                <span class="label_title_bold">承办日期:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtChengBanRiQi" CssClass="txtbox_bgreen" runat="server" Width="100px"
                                    ReadOnly="true">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
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
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">部门负责人:</span>
                            </td>
                            <td style="width: 95px;">
                                <cc1:FSDropDownList ID="ddlFuZeRen" runat="server" CssClass="dropdownlist_yellow"
                                    Width="90px">
                                </cc1:FSDropDownList>
                                <asp:Label ID="lbLeader" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                            <td style="width: 50px;">
                                <span class="label_title_bold">核稿人:</span>
                            </td>
                            <td style="width: 110px;">
                                <cc1:FSTextBox ID="txtHeGaoRen" CssClass="txtbox_yellow" runat="server" Width="80px">
                                </cc1:FSTextBox>
                                <uc:OASelectUC ID="OASelectUC3" runat="server" Visible="false" />
                                <asp:Label ID="lbHeGaoRen" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                            <td style="width: 50px;">
                                <span class="label_title_bold">拟稿人:</span>
                            </td>
                            <td style="width: 90px;">
                                <cc1:FSTextBox ID="txtNiGaoRen" CssClass="txtbox_bgreen" runat="server" Width="80px"
                                    ReadOnly="true">
                                </cc1:FSTextBox>
                                 <asp:Label ID="lbNiGaoRen" runat="server" Text="" Visible="false"></asp:Label>
                           </td>
                            <td style="width: 60px;">
                                <span class="label_title_bold">签发日期:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtQianFaRiQi" CssClass="txtbox_bgreen" runat="server" Width="115px"
                                    ReadOnly="true">
                                </cc1:FSTextBox>
                                 <asp:Label ID="lbQianFaRiQi" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table id="tbChuanYue" runat="server" visible="false">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">传阅:</span>
                            </td>
                            <td>
                                <div>
                                    部门：</div>
                                <cc1:FSTextBox ID="txtDeptName" runat="server" CssClass="txtbox_yellow" Style="width: 584px;
                                    height: 50px" TextMode="MultiLine">
                                </cc1:FSTextBox>
                                <div>
                                    公司领导：</div>
                                <cc1:FSTextBox ID="txtUserName" runat="server" CssClass="txtbox_yellow" Style="width: 584px;"
                                    TextMode="SingleLine">
                                </cc1:FSTextBox>
                                <uc:OASelectUC ID="OASelectUC4" runat="server" Visible="false" />
                                <asp:HiddenField ID="hDeptID" runat="server" />
                                <asp:HiddenField ID="hUserID" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
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
                        <cc1:FSButton ID="btnChengBanFenFa" runat="server" CssClass="btn" Text="承办分发" UseSubmitBehavior="false"
                            Visible="false" />
                        <cc1:FSButton ID="btnTongGuoShenHe" runat="server" CssClass="btn" OnClientClick="DisableButtons()"
                            Text="通过审核" UseSubmitBehavior="false" Visible="false" />
                        <cc1:FSButton ID="btnLingDaoPiShi" runat="server" CssClass="btn" OnClientClick="DisableButtons()"
                            Text="领导批示" UseSubmitBehavior="false" Visible="false" />
                        <cc1:FSButton ID="btnZhuRenShenHe" runat="server" CssClass="btn" OnClientClick="DisableButtons()"
                            Text="主任审核" UseSubmitBehavior="false" Visible="false" />
                        <cc1:FSButton ID="btnFenFa" runat="server" CssClass="btn" Text="提交审核" UseSubmitBehavior="false"
                            Visible="false" />
                        <cc1:FSButton ID="btnTiJiaoQianFa" runat="server" Text="提交签发" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" Visible="false" />
                        <cc1:FSButton ID="btnTiJiaoHeGao" runat="server" Text="提交核稿" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" Visible="false" />
                        <cc1:FSButton ID="btnTuiHui" runat="server" CssClass="btn" OnClientClick="DisableButtons()"
                            Text="退回" UseSubmitBehavior="false" Visible="false" />
                        <cc1:FSButton ID="btnCancel" runat="server" Text="撤销" CssClass="btn" UseSubmitBehavior="false"
                            Visible="false" />
                        <cc1:FSButton ID="btnSave" runat="server" Text="保存" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" Visible="false" />
                        <cc1:FSButton ID="btnJiaoBanKeshi" runat="server" CssClass="btn" OnClientClick="DisableButtons()"
                            Text="交办科室" UseSubmitBehavior="false" Visible="false" />
                        <cc1:FSButton ID="btnJiaoBanRenYuan" runat="server" CssClass="btn" OnClientClick="DisableButtons()"
                            Text="交办人员" UseSubmitBehavior="false" Visible="false" />
                        <cc1:FSButton ID="btnWanCheng" runat="server" CssClass="btn" OnClientClick="DisableButtons()"
                            Text="完成" UseSubmitBehavior="false" Visible="false" />
                        <cc1:FSButton ID="btnZhuiJiaFenFa" runat="server" CssClass="btn" OnClientClick="DisableButtons()"
                            Text="追加分发" UseSubmitBehavior="false" Visible="false" />
                        <cc1:FSButton ID="btnWanChengGuiDang" runat="server" CssClass="btn" Text="完成归档" UseSubmitBehavior="false"
                            Visible="false" />
                        <cc1:FSButton ID="btn_GuiDang" runat="server" CssClass="btn" Text="归档" UseSubmitBehavior="false"
                            Visible="false" onclick="btn_GuiDang_Click" />
                        <uc:UCPrint ID="ucPrint" runat="server" />
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
</div>
<span id="idShow"></span>
<input type="hidden" id="IsMoTai" value="true" />
<input type="hidden" id="IsDrag" value="true" />
<input type="hidden" id="IsMiddle" value="true" />
<input type="hidden" id="IsResize" value="true" />
<input type="hidden" id="imgLoading" value="true" />

<script language="javascript" type="text/javascript">
    function ShowMyDiv() {
        if ($('<%= txtRequestCategory.ClientID %>').value == "") {
            ShowPopDiv("PopDivID", "PopDivID");
        }
    }
</script>

<asp:Label ID="lbJs" runat="server"></asp:Label>