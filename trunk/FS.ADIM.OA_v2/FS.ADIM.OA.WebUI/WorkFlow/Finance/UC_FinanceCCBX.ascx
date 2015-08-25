<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_FinanceCCBX.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.Finance.UC_FinanceCCBX" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../../PageWF/UC_Print.ascx" TagName="UCPrint" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_FileControl.ascx" TagName="FileControl" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_OASelect.ascx" TagName="OASelectUC" TagPrefix="uc" %>
<%@ Register Src="UC_CCBXDetail.ascx" TagName="UC_CCBXDetail" TagPrefix="uc" %>
<script type="text/javascript">

    function ShowMyDiv() {
        if ($('<%= txtType.ClientID %>').value == "") {
            ShowPopDiv("PopDivID", "PopDivID");
        }
    }
    function SelectType() {
        $('<%= txtType.ClientID %>').value = $('<%= ddlType.ClientID %>').value;
    }
    $("containTitle").innerHTML = "出差(培训)报销单 - " + "<%=StepName%>";
</script>
<asp:ScriptManager ID="ScriptManager" runat="server" EnableScriptGlobalization="true"
    EnableScriptLocalization="true">
</asp:ScriptManager>
<asp:UpdateProgress ID="UpdateProgress1" runat="server">
    <progresstemplate>
        <div id="iLoading" style="left: 20px; position: absolute; top: 600px; left: 450px">
            <img id="imgLoading" src="../../Img/loading.gif" alt="" />
        </div>
    </progresstemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="UpdatePanel" runat="server">
    <contenttemplate>
<div id="MainDivID">
</div>
<div id="PopDivID" style="display: none; height: 100px; width: 180px;" class="PopDiv">
    <div class="PopDivTitle">
        请选择：
    </div>
    <table class="table_form_noborder">
        <tr>
            <td style="text-align: right; width: 79px; height: 50px">
                报销单类型：
            </td>
            <td style="width: 70px;">
                <cc1:FSDropDownList ID="ddlType" runat="server" Width="60px" CssClass="dropdownlist_yellow"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="出差">出差</asp:ListItem>
                    <asp:ListItem Value="培训">培训</asp:ListItem>
                </cc1:FSDropDownList>
            </td>
        </tr>
    </table>
    <div style="text-align: center">
        <input type="button" value="确定" class="btn" onclick="SelectType();hiddenPopDiv('MainDivID','PopDivID')" />
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
                                    <cc1:FSLabel ID="lblFileTitle" runat="server" Text="海南核电有限公司出差报销单"></cc1:FSLabel>
                                </span>
                            </td>
                        </tr>
                    </table>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">申请单编号:</span>
                            </td>
                            <td style="width: 160px">
                                <cc1:FSTextBox ID="txtChuChaDanHao" runat="server" Width="120px" 
                                    CssClass="txtbox_yellow" AutoPostBack="True"></cc1:FSTextBox>
                                <cc1:FSHyperLink ID="linkShow" runat="server" Text="查看" Style="Cursor:hand" Target="_blank"></cc1:FSHyperLink>
                            </td>
                            <td style="width: 70px;">
                                <span class="label_title_bold">报销单编号:</span>
                            </td>
                            <td style="width: 150px">
                                <cc1:FSTextBox ID="txtBianHao" runat="server" Width="130px" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                            <td style="width: 70px">
                                <span class="label_title_bold">日期:</span>
                            </td>
                            <td style="width: 140px">
                                <cc1:FSTextBox ID="txtNiGaoRiQi" runat="server" Width="130px" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">报销人:</span>
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
                                    DataValueField="ID" AutoPostBack="true" CssClass="dropdownlist_yellow" OnSelectedIndexChanged="ddlBianZhiBuMen_SelectedIndexChanged">
                                </cc1:FSDropDownList>

                               
                            </td>
                            <td style="width: 70px;">
                                <span class="label_title_bold">职务/职称:</span>
                            </td>
                            <td style="width: 140px">
                                 <cc1:FSDropDownList ID="ddlZhiCheng" runat="server" MaxLength="40" Width="134px"
                                    CssClass="dropdownlist_yellow">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                    <asp:ListItem Text="公司领导" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="处级干部或研高职称" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="科级干部或高级职称" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="普通员工" Value="4"></asp:ListItem>
                                </cc1:FSDropDownList>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">联系电话:</span>
                            </td>
                            <td style="width: 160px">
                                <cc1:FSTextBox ID="txtPhone" runat="server" Width="130px" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                            <td style="width: 70px;">
                                <span class="label_title_bold">个人借款:</span>
                            </td>
                            <td style="width: 150px">
                                <cc1:FSDropDownList ID="ddlJieKuan" runat="server" MaxLength="40" Width="144px" CssClass="dropdownlist_yellow">
                                    <asp:ListItem Text="否" Value="否" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="是" Value="是"></asp:ListItem>
                                </cc1:FSDropDownList>
                            </td>
                            <td style="width: 70px;">
                                <span class="label_title_bold">附单据张数:</span>
                            </td>
                            <td style="width: 140px">
                                <cc1:FSTextBox ID="txtDanJuZhangShu" runat="server" Width="130px" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">出差事由:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtShiYou" runat="server" Style="width: 600px; height: 30px" TextMode="MultiLine"
                                    MaxLength="2000" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <table class="SheetContent" id="tableCCCS" runat="server">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">出差城市选项</span>
                            </td>
                            <td>
                                <cc1:FSCheckBox ID="chkIsYiXian" runat="server" Text="出差城市是否在深圳、广州、上海、北京"></cc1:FSCheckBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="table_form_noborder" style="width: 720px">
                        <tr>
                            <td>
                                <fieldset style="text-align: center">
                                    <legend>出行明细</legend>
                                    <uc:UC_CCBXDetail ID="ucChuXingMingXi" runat="server" />
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <div style="height: 5px">
                    </div>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="table_form_noborder">
                        <tr>
                            <td style="width: 360px">
                                <fieldset style="height: 190px;">
                                    <legend>其他费用</legend>
                                    <table>
                                        <tr>
                                            <td style="width: 80px">
                                                摘要
                                            </td>
                                            <td>
                                                天数
                                            </td>
                                            <td>
                                                金额
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                住宿费
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtZhuSuRT" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtZhuSuJE" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                自行住宿
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtZiXingTianShu1" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                            <td>
                                               
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                托运费
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtTuoYunRT" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtTuoYunJE" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                其他
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtQiTaRT" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtQiTaJE" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                小计
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtQiTaXiaoJi" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td style="width: 360px">
                                <fieldset style="height: 190px;">
                                    <legend>出差补贴</legend>
                                    <table>
                                        <tr>
                                            <td style="width: 100px">
                                                项目
                                            </td>
                                            <td>
                                                天数
                                            </td>
                                            <td>
                                                金额
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                住勤补贴
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtZhuQinRT" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtZhuQinJE" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                住宿节约补贴
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtZhuSuJYRT" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtZhuSuJYJE" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                未乘坐卧铺补贴
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtWeiWoPuRT" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtWeiWoPuBuJE" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                在途补贴
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtZaiTuRT" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtZaiTuJE" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                自行住宿补贴
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtZiXingTianShu2" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtZiXingJinE" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                小计
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtBuTieXiaoJi" runat="server" Width="100px" CssClass="txtbox_yellow">
                                                </cc1:FSTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed" id="divPeiXun1" runat="server" visible="false">
                    </div>
                    <table class="SheetContent" id="tablePeiXun1" runat="server" visible="false">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">培训号/立项号:</span>
                            </td>
                            <td style="width: 150px;">
                                <cc1:FSTextBox ID="txtLiXiangHao" runat="server" Width="140px" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                            <td style="width: 70px;">
                                <span class="label_title_bold">预算内:</span>
                            </td>
                            <td style="width: 112px;">
                                <cc1:FSDropDownList ID="ddlYuSuanNei" runat="server" Width="60px" CssClass="dropdownlist_yellow">
                                    <asp:ListItem Text="" Value="" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="是" Value="是"></asp:ListItem>
                                    <asp:ListItem Text="否" Value="否"></asp:ListItem>
                                </cc1:FSDropDownList>
                            </td>
                            <td style="width: 130px;">
                                <span class="label_title_bold">预算或立项金额:</span>
                            </td>
                            <td style="width: 130px;">
                                <cc1:FSTextBox ID="txtLiXiangJE" runat="server" Width="120px" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                    <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">长期出差补贴:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtChangQiBuTie" runat="server" Style="width: 200px;" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                            <td style="width: 120px">

                            </td>
                            <td style="width: 250px">
                            </td>
                        </tr>
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">合计 ￥:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtHeJi" runat="server" Style="width: 200px;" CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                            <td style="width: 120px">
                                <cc1:FSButton ID="btnCal1" runat="server" Text="自动计算" CssClass="btn" UseSubmitBehavior="false"
                                    OnClientClick="DisableButtons()" ToolTip="计算出差补贴、小计、合计" OnClick="btnCal1_Click" />
                            </td>
                            <td style="width: 250px">
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">财务审核金额:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtCaiWuJE" runat="server" Style="width: 200px;" MaxLength="200"
                                    CssClass="txtbox_yellow">
                                </cc1:FSTextBox>
                            </td>
                            <td style="width: 120px">
                                <cc1:FSButton ID="btnCal2" runat="server" Text="大写转换" CssClass="btn" UseSubmitBehavior="false"
                                    OnClientClick="DisableButtons()" ToolTip="大写转换" OnClick="btnCal2_Click" />
                            </td>
                            <td style="width: 250px">
                            </td>
                        </tr>
                    </table>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">部门/主管领导:</span>
                            </td>
                            <td style="width: 150px;">
                                <cc1:FSDropDownList ID="ddlBuMenZhuGuan" runat="server" Width="140px" CssClass="dropdownlist_yellow">
                                </cc1:FSDropDownList>
                            </td>
                            <td style="width: 70px;">
                                <span class="label_title_bold">公司领导:</span>
                            </td>
                            <td style="width: 180px;">
                                <cc1:FSDropDownList ID="ddlGSLingDao" runat="server" Width="140px" CssClass="dropdownlist_yellow">
                                </cc1:FSDropDownList>
                            </td>
                            <td style="width: 40px;">
                                <span class="label_title_bold">财务:</span>
                            </td>
                            <td>
                                <cc1:FSDropDownList ID="ddlCaiWu" runat="server" Width="140px" CssClass="dropdownlist_yellow">
                                </cc1:FSDropDownList>
                            </td>
                        </tr>
                    </table>
                    <table class="SheetContent" id="tablePeiXunChu" runat="server" visible="false">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">培训处主管:</span>
                            </td>
                            <td style="width: 150px;">
                                <cc1:FSDropDownList ID="ddlPeiXunChu" runat="server" Width="140px" CssClass="dropdownlist_yellow">
                                </cc1:FSDropDownList>
                            </td>
                            <td style="width: 70px;">
                                <span class="label_title_bold">立项审签:</span>
                            </td>
                            <td style="width: 150px;">
                            </td>
                            <td style="width: 70px;">
                            </td>
                            <td style="width: 150px;">
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
                        <cc1:FSButton ID="btnBuMenZhuGuan" runat="server" CssClass="btn_long" Text="主管领导审核"
                            UseSubmitBehavior="false" OnClientClick="DisableButtons()" ToolTip="主管领导审核" />
                        <cc1:FSButton ID="btnPeiXunChu" runat="server" CssClass="btn_long" Text="培训处审核" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" ToolTip="培训处审核" />
                        <cc1:FSButton ID="btnGongSiLingDao" runat="server" CssClass="btn_long" Text="公司领导审核"
                            UseSubmitBehavior="false" OnClientClick="DisableButtons()" ToolTip="公司领导审核" />
                        <cc1:FSButton ID="btnCaiWu" runat="server" CssClass="btn" Text="财务审核" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" ToolTip="财务审核" />
                        <cc1:FSButton ID="btnCaiWuPass" runat="server" CssClass="btn" Text="审核通过" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" ToolTip="审核通过" />
                        <cc1:FSButton ID="btnWanCheng" runat="server" CssClass="btn" Text="完成" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" ToolTip="完成" />
                        <cc1:FSButton ID="btnTuiHui" runat="server" CssClass="btn" Text="退回" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" ToolTip="退回" />
                        <cc1:FSButton ID="btnCancel" runat="server" Text="撤销" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" ToolTip="撤销" />
                        <uc:UCPrint ID="ucPrint" runat="server" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
<div style="display:none">
    <cc1:FSTextBox ID="txtNiGaoRenID" runat="server"></cc1:FSTextBox>
    <cc1:FSTextBox ID="txtIsBack" runat="server"></cc1:FSTextBox>
    <cc1:FSTextBox ID="txtType" runat="server"></cc1:FSTextBox>
    <cc1:FSTextBox ID="txtChuShiID" runat="server"></cc1:FSTextBox>
    <asp:Label ID="lbJs" runat="server"></asp:Label>
    <br /> <br /> <br />

</div>
        </table>
    </contenttemplate>
    <triggers>
        <asp:PostBackTrigger ControlID="ucPrint" />
    </triggers>
</asp:UpdatePanel>