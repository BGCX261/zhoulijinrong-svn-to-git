<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_WorkRelation.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.WorkRelation.UC_WorkRelation" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../../PageWF/UC_Print.ascx" TagName="UCPrint" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_FileControl.ascx" TagName="FileControl" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_OASelect.ascx" TagName="OASelectUC" TagPrefix="uc" %>
<%@ Register Src="UC_HuiQian.ascx" TagName="UC_HuiQian" TagPrefix="uc" %>

<style type="text/css">
    .style1
    {
        height: 894px;
    }
</style>

<script type="text/javascript">
    $("containTitle").innerHTML = "<%=TemplateName%> - " + "<%=StepName%>";
    function checkChaoSong() {
        if ($('<%= txtChaoSong.ClientID %>').value == "") {
            return confirm("没有选择抄送,是否继续？");
        } else {
            return true;
        }
    }
    function checkChuanYue() {
        if ($('<%= txtChuanYueRenYuan.ClientID %>').value == "") {
            return confirm("没有选择传阅,是否继续？");
        } else {
            return true;
        }
    }
    //    function OpenConditionDialog(dealCondition, ucID) {
    //        var encodeValue = encodeURIComponent(dealCondition);
    //        var isHistory = "<%=IsPreview %>";
    //        var condition = window.showModalDialog("Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkFlow.WorkRelation.PGAddInfo&content=" + encodeValue + "&ucID=" + ucID + "&isHistory=" + isHistory, "", "dialogWidth:410px;dialogHeight:220px;center:yes;scroll=no;status=no");

    //        if (typeof (condition) == "undefined") {
    //            event.returnValue = false;
    //            return false;
    //        } else {
    //            $(ucID).value = condition;
    //        }
    //        event.returnValue = false;
    //    }
</script>

<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<div class="divCenter">
    <table>
        <tr>
            <td style="text-align: left" class="style1">
                <div style="width: 720px; margin: 0px auto; border: #000000 1px solid;">
                    <table style="text-align: center; height: 50px; width: 100%">
                        <tr>
                            <td style="background-color: #DaDaDa">
                                <span class="label_title_darkblue">海南核电有限公司工作联系单</span>
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
                                </cc1:FSTextBox>
                                <uc:OASelectUC ID="OASelectUC1" runat="server" Visible="false" />
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
                                <span class="label_title_bold">答复或处理<br />
                                    意见:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtChuLiYiJian" CssClass="txtbox_bgreen" runat="server" Style="width: 584px;
                                    height: 60px" TextMode="MultiLine" ReadOnly="true">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                        <tr id="trBanLiYiJian" runat="server" visible="false">
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
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table id="tbChenBan" runat="server" visible="false" class="SheetContent">
                        <tr>
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
                            <td style="width: 85px;">
                                <cc1:FSDropDownList ID="ddlChengBanRen" CssClass="dropdownlist_yellow" runat="server"
                                    Width="80px" ReadOnly="true">
                                </cc1:FSDropDownList>
                                <asp:Label ID="lblMember" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                            <td style="width: 60px;">
                                <span class="label_title_bold">承办日期:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtChengBanRiQi" CssClass="txtbox_bgreen" runat="server" Width="115px"
                                    ReadOnly="true">
                                </cc1:FSTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <div style="border-bottom: 1px #ccc dashed">
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table class="SheetContent">
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
                            <td style="width: 60px;">
                                <span class="label_title_bold">核稿人:</span>
                            </td>
                            <td style="width: 110px;">
                                <cc1:FSTextBox ID="txtHeGaoRen" runat="server" CssClass="txtbox_yellow" Width="80px" Visible="true" > 
                                </cc1:FSTextBox><uc:OASelectUC ID="OASelectUC2" runat="server" Visible="false" />
                                <asp:Label ID="lbHeGaoRen" runat="server" Text="" Visible="true"></asp:Label>
                            </td>
                            <td style="width: 60px;">
                                <span class="label_title_bold">拟稿人:</span>
                            </td>
                            <td style="width: 85px;">
                                <cc1:FSTextBox ID="txtNiGaoRen" CssClass="txtbox_bgreen" runat="server" Width="75px"
                                    ReadOnly="true">
                                </cc1:FSTextBox>
                                 <asp:Label ID="lbNiGaoRen" runat="server" Text="" Visible="false"></asp:Label>
                           </td>
                            <td style="width: 60px;">
                                <asp:Label ID="lbRiQiTitle" runat="server" Text="" Visible="true" class="label_title_bold">签发日期</asp:Label>
                           </td>
                            <td>
                                <cc1:FSTextBox ID="txtQianFaRiQi" CssClass="txtbox_bgreen" runat="server" Width="115px"
                                    ReadOnly="true">
                                </cc1:FSTextBox>
                                 <asp:Label ID="lbQianFaRiQi" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                            <tr>
                                <td colspan="8">
                                    <div style="border-bottom: 1px #ccc dashed">
                                    </div>
                                </td>
                            </tr>
                        </tr>
                    </table>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table class="SheetContent">
                                <tr>
                                    <td class="LeftSpan">
                                        <span class="label_title_bold">部门会签：</span>
                                    </td>
                                    <td>
                                        <uc:UC_HuiQian ID="ucBuMenHuiQian" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div style="border-bottom: 1px #ccc dashed">
                    </div>
                    <table id="tbChenBanChuanYue" runat="server" visible="false" class="SheetContent">
                        <tr>
                            <td class="LeftSpan">
                                <span class="label_title_bold">承办传阅:</span>
                            </td>
                            <td>
                                <cc1:FSTextBox ID="txtChuanYueRenYuan" CssClass="txtbox_yellow" TextMode="MultiLine"
                                    runat="server" Style="width: 584px; height: 30px">
                                </cc1:FSTextBox><uc:OASelectUC ID="OASelectUC3" runat="server" Visible="false" />
                            </td>
                        </tr>
                    </table>
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
                    <div class="divSubmit" id="divSubmit">
                        <cc1:FSButton ID="btnTiJiaoHeGao" runat="server" Text="提交核稿" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" Visible="False" />
                        <cc1:FSButton ID="btnTiJiaoQianFa" runat="server" Text="提交签发" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" Visible="False" />
                        <cc1:FSButton ID="btnDeptSign" runat="server" Text="部门会签" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" Visible="False" />
                        <cc1:FSButton ID="btnJiaoBanKeShi" runat="server" Text="交办科室" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" Visible="False" />
                        <cc1:FSButton ID="btnJiaoBanRenYuan" runat="server" Text="交办人员" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" Visible="False" />
                        <cc1:FSButton ID="btnWanCheng" runat="server" Text="完成" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" Visible="False" />
                        <cc1:FSButton ID="btnFenFa" runat="server" Text="签发" CssClass="btn" UseSubmitBehavior="false"
                            Visible="False" />
                        <cc1:FSButton ID="btnCancel" runat="server" Text="撤销" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" Visible="False" />
                        <cc1:FSButton ID="btnTuiHui" runat="server" Text="退回" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" Visible="False" />
                        <cc1:FSButton ID="btnSave" runat="server" Text="保存" CssClass="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons()" Visible="False" />
                        <uc:UCPrint ID="ucPrint" runat="server" />
                    </div>
                </div>
            </td>
            <td id="tdDeptSign" valign="top" runat="server" visible="false" style="border: 1px solid #64b7d7;
                text-align: center;" class="style1">
                <div style="height: 20px; padding: 10px 0px; background: #deeef4; border: #64b7d7 1px solid;
                    font-weight: bold; width: 220px;">
                    添加意见</div>
                <div style="width: 100%">
                    <asp:Panel ID="pnlComment" runat="server" Visible="false">
                        <cc1:FSTextBox ID="txtInfo2" Height="80px" Width="212px" TextMode="MultiLine" CssClass="textarea_yello"
                            runat="server" MaxLength="100"></cc1:FSTextBox>
                    </asp:Panel>
                    <table>
                        <tr id="trYiJianHead" runat="server" visible="false" style="font-weight: bold; background: #deeef4;">
                            <td style="width: 30px">
                                序号
                            </td>
                            <td style="display: none">
                                参与人
                            </td>
                            <td id="tdhTime" runat="server" visible="false">
                                完成时间
                            </td>
                            <td style="width: 100px">
                                意见
                            </td>
                            <td style="width: 30px">
                            </td>
                        </tr>
                        <asp:Repeater ID="rptComment" runat="server" OnItemCommand="rptComment_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align: center; background-color: #F7F7F7; width: 30px">
                                        <%# Container.ItemIndex + 1 %>
                                    </td>
                                    <td style="background-color: #F7F7F7; display: none">
                                        <%# Eval("UserName")%>
                                    </td>
                                    <td id="tdTime" runat="server" visible="false">
                                        <%# Eval("FinishTime")%>
                                    </td>
                                    <td style="background-color: #F8F6EB">
                                        <asp:Label ID="lblContent" runat="server" Text='<%# Eval("Content")%>' CssClass="lblcontent"></asp:Label>
                                    </td>
                                    <td style="width: 30px; background-color: #F7F7F7">
                                        <cc1:FSLinkButton ID="lnkbtnDel" runat="server" CommandName="Delete" Text="删除" OnClientClick="return confirm('是否删除？')"></cc1:FSLinkButton>
                                        <cc1:FSLinkButton ID="lnkbtnEdit" runat="server" CommandName="Edit" Text="修改"></cc1:FSLinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    <div class="font_red" style="padding: 10px;">
                        提示：填写意见表示否决</div>
                    <div class="divSubmit">
                        <cc1:FSButton ID="btnAdd" runat="server" CssClass="btn" Text="添加意见" />
                        <cc1:FSButton ID="btnCancel1" runat="server" Text="取消" CssClass="btn" Visible="false" />
                        <cc1:FSButton ID="btnConfirm" runat="server" Text="确定" CssClass="btn" Visible="false" /></div>
                    <div class="divSubmit">
                        <cc1:FSButton ID="btnSubmits" runat="server" CssClass="btn" Text="提交" UseSubmitBehavior="false" />
                        <cc1:FSButton ID="btnSave3" runat="server" CssClass="btn" Text="保存" UseSubmitBehavior="false" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
<cc1:FSHiddenField ID="hfEditIndex" runat="server" />
<div style="display: none">
    <cc1:FSTextBox ID="txtIsBack" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtChaoSongDeptIDs" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtChaoSongIDs" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtBuMenLingDaoID" runat="server">
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
    <cc1:FSTextBox ID="txtChuanYueRenYuanID" runat="server">
    </cc1:FSTextBox>
    <asp:TextBox ID="wfReceiveUserID" runat="server"></asp:TextBox>
    <asp:TextBox ID="wfReceiveUserName" runat="server"></asp:TextBox>
    <asp:TextBox ID="wfTimesFlag" runat="server"></asp:TextBox>
</div>
