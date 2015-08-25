<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Send.ascx.cs" Inherits="FS.ADIM.OA.WebUI.WorkFlow.Send.UC_Send" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../../PageWF/UC_Print.ascx" TagName="Print" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_OASelect.ascx" TagName="OASelect" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_FileControl.ascx" TagName="FileControl" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_Comment.ascx" TagName="Comment" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_Role.ascx" TagName="Role" TagPrefix="uc" %>

<script type="text/javascript">
    function checkChuanYue() {
        if ($('<%= txtDeptName.ClientID %>').value == "" && $('<%= txtUserName.ClientID %>').value == "") {
            return confirm("没有选择分发范围,是否继续？");
        } else {
            return true;
        }
    }
    $("containTitle").innerHTML = "<%=TemplateName%> - " + "<%=StepName%>";
</script>

<table style="width: 100%;">
    <tr>
        <td>
            <div class="divCenter">
                <table class="table_form_border" style="width: 720px;">
                    <tr>
                        <td class="td_grey" style="height: 30px">
                            <span class="label_title_darkblue">海南核电有限公司党纪工团发文单</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table class="table_form_noborder">
                                <tr>
                                    <td colspan="6">
                                        <table>
                                            <tr>
                                                <td>
                                                    <span class="label_title_bold">发文类型：</span>
                                                </td>
                                                <td>
                                                    <cc1:FSDropDownList ID="ddlType" AutoPostBack="True" CssClass="dropdownlist_yellow"
                                                        Style="width: 100px;" runat="server" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                        <asp:ListItem></asp:ListItem>
                                                        <asp:ListItem>党委发文</asp:ListItem>
                                                        <asp:ListItem>纪委发文</asp:ListItem>
                                                        <asp:ListItem>工会发文</asp:ListItem>
                                                        <asp:ListItem>团委发文</asp:ListItem>
                                                    </cc1:FSDropDownList>
                                                    <span class="label_title_red">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="label_title_bold">紧急程度：</span>
                                                </td>
                                                <td>
                                                    <cc1:FSDropDownList ID="ddlUrgentDegree" CssClass="dropdownlist_yellow" Style="width: 100px;"
                                                        runat="server">
                                                        <asp:ListItem>普通</asp:ListItem>
                                                        <asp:ListItem>紧急</asp:ListItem>
                                                    </cc1:FSDropDownList>
                                                </td>
                                                <td id="TdYearNum" runat="server" visible="false">
                                                    <span class="label_title_bold">年度：</span><span id="RedSpan_Year" runat="server" class="label_title_red"
                                                        visible="false">*</span>
                                                    <cc1:FSTextBox ID="txtDocumentYear" runat="server" CssClass="txtbox_bgreen" ReadOnly="True"
                                                        Style="width: 50px">
                                                    </cc1:FSTextBox>
                                                    <span class="label_title_bold">序号：</span><span id="RedSpan_Num" runat="server" class="label_title_red"
                                                        visible="false">*</span>
                                                    <cc1:FSTextBox ID="txtDocumentNum" runat="server" CssClass="txtbox_bgreen" ReadOnly="True"
                                                        Style="width: 50px">
                                                    </cc1:FSTextBox>
                                                </td>
                                                <td>
                                                    <span class="label_title_bold">发文号：</span><span id="RedSpan_No" class="label_title_red"
                                                        runat="server" visible="false">*</span>
                                                </td>
                                                <td>
                                                    <cc1:FSTextBox ID="txtDocumentNo" CssClass="txtbox_bgreen" Style="width: 150px" runat="server"
                                                        ReadOnly="True" MaxLength="50">
                                                    </cc1:FSTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <div style="border-bottom: 1px #ccc dashed">
                                        </div>
                                        <table class="table_form_noborder">
                                            <tr>
                                                <td>
                                                    <fieldset style="height: 80px;">
                                                        <legend>领导会签</legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span class="label_title_bold">会签：</span>
                                                                </td>
                                                                <td rowspan="2">
                                                                    <cc1:FSTextBox ID="txtLeadSigners" CssClass="textarea_bgreen" Style="width: 247px;
                                                                        height: 40px" runat="server" TextMode="MultiLine" TabIndex="-1">
                                                                    </cc1:FSTextBox>
                                                                </td>
                                                                <td>
                                                                    <uc:Role ID="ucRole" runat="server" Visible="false" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <uc:Comment ID="ucLeadCounterSignComments" runat="server" />
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                                <td>
                                                    <fieldset style="height: 80px;">
                                                        <legend>支部会签</legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span class="label_title_bold">会签：</span>
                                                                </td>
                                                                <td rowspan="2">
                                                                    <cc1:FSTextBox ID="txtDeptSigners" CssClass="textarea_bgreen" Style="width: 247px;
                                                                        height: 40px" runat="server" TextMode="MultiLine" TabIndex="-1">
                                                                    </cc1:FSTextBox>
                                                                </td>
                                                                <td>
                                                                    <uc:Role ID="ucDeptCounterSigns" runat="server" Visible="false" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <uc:Comment ID="ucDeptCounterSignComments" runat="server" />
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                        <div style="border-bottom: 1px #ccc dashed">
                                        </div>
                                        <table class="table_form_noborder">
                                            <tr>
                                                <td>
                                                    <fieldset style="height: 80px;">
                                                        <legend>签发</legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span class="label_title_bold">签发人：</span>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSDropDownList ID="ddlSigner" CssClass="dropdownlist_green" Style="width: 120px"
                                                                        runat="server" Enabled="False">
                                                                    </cc1:FSDropDownList>
                                                                    <asp:Label ID="lbSigner" runat="server" Text="" Visible="false"></asp:Label>
                                                                </td>
                                                                <td rowspan="2">
                                                                    <span class="label_title_bold">签发意见：</span>
                                                                </td>
                                                                <td rowspan="2">
                                                                    <cc1:FSTextBox ID="txtSignCommentView" CssClass="txtbox_bgreen" Style="width: 374px;
                                                                        height: 40px;" runat="server" ReadOnly="True" TextMode="MultiLine">
                                                                    </cc1:FSTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span class="label_title_bold">签发日期：</span>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSTextBox ID="txtSignDate" CssClass="txtbox_bgreen" Style="width: 116px" runat="server"
                                                                        ReadOnly="True">
                                                                    </cc1:FSTextBox>
                                                                    <asp:Label ID="lbSignDate" runat="server" Text="" Visible="false"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                        <div style="border-bottom: 1px #ccc dashed">
                                        </div>
                                        <table class="table_form_noborder">
                                            <tr>
                                                <td>
                                                    <fieldset style="height: 103px;">
                                                        <legend>核稿</legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span class="label_title_bold">秘书：</span>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSTextBox ID="txtSecretaryChecker" CssClass="txtbox_bgreen" Style="width: 50px"
                                                                        runat="server" ReadOnly="True">
                                                                    </cc1:FSTextBox>
                                                                     <asp:Label ID="lbChecker" runat="server" Text="" Visible="false"></asp:Label>
                                                               </td>
                                                                <td>
                                                                    <span class="label_title_bold">日期：</span>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSTextBox ID="txtSecretaryCheckDate" CssClass="txtbox_bgreen" Style="width: 120px"
                                                                        runat="server" ReadOnly="True">
                                                                    </cc1:FSTextBox>
                                                                     <asp:Label ID="lbSecretaryCheckDate" runat="server" Text="" Visible="false"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                                <td>
                                                    <fieldset style="height: 103px;">
                                                        <legend>主办部门</legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span class="label_title_bold">审稿人：</span><span id="RedSpanCheckDrafter" runat="server"
                                                                        class="label_title_red" visible="false">*</span>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSDropDownList ID="ddlCheckDrafter" CssClass="dropdownlist_green" Style="width: 100px"
                                                                        runat="server" Enabled="False">
                                                                    </cc1:FSDropDownList>
                                                                    <asp:Label ID="lbCheckDrafter" runat="server" Text="" Visible="false"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <span class="label_title_bold">审稿日期：</span>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSTextBox ID="txtVerifyDate" runat="server" CssClass="txtbox_bgreen" Style="width: 90px"
                                                                        ReadOnly="True">
                                                                    </cc1:FSTextBox>
                                                                    <asp:Label ID="lbVerifyDate" runat="server" Text="" Visible="false"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span class="label_title_bold">主办部门：</span>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSDropDownList ID="ddlHostDept" CssClass="dropdownlist_green" Style="width: 100px"
                                                                        runat="server" Enabled="False" ToolTip="拟稿人所属处室">
                                                                    </cc1:FSDropDownList>
                                                                </td>
                                                                <td>
                                                                    <span class="label_title_bold">拟稿日期：</span>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSTextBox ID="txtDraftDate" runat="server" CssClass="txtbox_bgreen" Style="width: 90px"
                                                                        ReadOnly="True">
                                                                    </cc1:FSTextBox>
                                                                    <asp:Label ID="lbDraftDate" runat="server" Text="" Visible="false"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span class="label_title_bold">拟稿人：</span>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSTextBox ID="txtDrafter" runat="server" CssClass="txtbox_bgreen" Style="width: 96px"
                                                                        ReadOnly="True">
                                                                    </cc1:FSTextBox>
                                                                    <asp:Label ID="lbDrafter" runat="server" Text="" Visible="false"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <span class="label_title_bold">电话：</span>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSTextBox ID="txtPhoneNum" runat="server" CssClass="txtbox_bgreen" Style="width: 90px"
                                                                        ReadOnly="True" MaxLength="50">
                                                                    </cc1:FSTextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <div style="border-bottom: 1px #ccc dashed">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="label_title_bold">标题：</span><span id="RedSpanTitle" runat="server" visible="false"
                                            class="label_title_red">*</span>
                                    </td>
                                    <td colspan="5">
                                        <cc1:FSTextBox ID="txtDocumentTitle" runat="server" CssClass="txtbox_yellow" Style="width: 583px"
                                            MaxLength="100">
                                        </cc1:FSTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <div style="border-bottom: 1px #ccc dashed">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="label_title_bold">主题词：</span><span id="RedSpanSubjectWord" runat="server"
                                            class="label_title_red" visible="false">*</span>
                                    </td>
                                    <td colspan="5">
                                        <cc1:FSTextBox ID="txtSubjectWord" runat="server" CssClass="txtbox_yellow" Style="width: 583px"
                                            MaxLength="100"></cc1:FSTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <div style="border-bottom: 1px #ccc dashed">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="label_title_bold">主送：</span><span id="RedSpanMainSender" runat="server"
                                            visible="false" class="label_title_red">*</span>
                                    </td>
                                    <td colspan="5">
                                        <cc1:FSTextBox ID="txtMainSender" runat="server" CssClass="textarea_yello" Style="width: 583px"
                                            MaxLength="500"></cc1:FSTextBox>
                                        <uc:OASelect ID="ucMainSender" runat="server" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <div style="border-bottom: 1px #ccc dashed">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="label_title_bold">抄送：</span>
                                    </td>
                                    <td colspan="5">
                                        <cc1:FSTextBox ID="txtCopySender" runat="server" CssClass="textarea_yello" Style="width: 583px"
                                            TextMode="MultiLine" MaxLength="500">
                                        </cc1:FSTextBox>
                                        <uc:OASelect ID="ucCopySender" runat="server" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <div style="border-bottom: 1px #ccc dashed">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="label_title_bold">发文日期：</span>
                                    </td>
                                    <td colspan="2">
                                        <cc1:FSCalendar ID="txtSendDate" CssClass="txtbox_yellow" Style="width: 120px" runat="server">
                                        </cc1:FSCalendar>
                                    </td>
                                    <td colspan="3">
                                        <span class="label">共印</span>
                                        <cc1:FSTextBox ID="txtShareCount" CssClass="txtbox_bgreen" runat="server" Style="width: 30px"
                                            ReadOnly="True" MaxLength="50">
                                        </cc1:FSTextBox>
                                        &nbsp;<span class="label">份，每份</span>
                                        <cc1:FSTextBox ID="txtSheetCount" CssClass="txtbox_bgreen" runat="server" Style="width: 30px"
                                            ReadOnly="True" MaxLength="50">
                                        </cc1:FSTextBox>
                                        <span class="label">张</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <div style="border-bottom: 1px #ccc dashed">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="label_title_bold">打字：</span>
                                    </td>
                                    <td style="text-align: left">
                                        <cc1:FSTextBox ID="txtTypist" CssClass="txtbox_bgreen" Style="width: 120px" runat="server"
                                            ReadOnly="True" MaxLength="50">
                                        </cc1:FSTextBox>
                                    </td>
                                    <td>
                                        <span class="label_title_bold">校对：</span>
                                    </td>
                                    <td>
                                        <cc1:FSTextBox ID="txtChecker" CssClass="txtbox_bgreen" Style="width: 120px" runat="server"
                                            ReadOnly="True" MaxLength="50">
                                        </cc1:FSTextBox>
                                        <asp:Label ID="lbCChecker" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <span class="label_title_bold">复核：</span>
                                    </td>
                                    <td>
                                        <cc1:FSTextBox ID="txtReChecker" CssClass="txtbox_bgreen" Style="width: 120px" runat="server"
                                            ReadOnly="True" MaxLength="50">
                                        </cc1:FSTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <div style="border-bottom: 1px #ccc dashed">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="label_title_bold">附件：</span>
                                    </td>
                                    <td colspan="5" style="text-align: left; width: 600px;">
                                        <uc:FileControl ID="ucAttachment" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <div style="border-bottom: 1px #ccc dashed">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="label_title_bold">提示信息：</span>
                                    </td>
                                    <td colspan="5" valign="top">
                                        <cc1:FSTextBox ID="txtAllPrompt" CssClass="txtbox_bgreen" Style="width: 298px; height: 60px;"
                                            runat="server" ReadOnly="True" TextMode="MultiLine">
                                        </cc1:FSTextBox>
                                        <cc1:FSTextBox ID="txtMyPrompt" CssClass="txtbox_yellow" Style="width: 276px; height: 60px;"
                                            runat="server" TextMode="MultiLine" EnableTheming="False">
                                        </cc1:FSTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <div style="border-bottom: 1px #ccc dashed">
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trChuanYue" runat="server" visible="false">
                                    <td>
                                        <span class="label_title_bold">分发：</span>
                                    </td>
                                    <td colspan="5">
                                        <div>
                                            <div>
                                                部门:</div>
                                            <cc1:FSTextBox ID="txtDeptName" runat="server" CssClass="txtbox_yellow" Style="width: 586px;
                                                height: 30px;" TextMode="MultiLine"></cc1:FSTextBox>
                                            <div>
                                                公司领导:</div>
                                            <cc1:FSTextBox ID="txtUserName" runat="server" CssClass="txtbox_yellow" Style="width: 586px;"
                                                TextMode="SingleLine"></cc1:FSTextBox>
                                            <uc:OASelect ID="ucOUCirculate" runat="server" />
                                            <asp:HiddenField ID="hDeptID" runat="server" />
                                            <asp:HiddenField ID="hUserID" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="divSubmit" id="divSubmit">
                                <cc1:FSButton ID="btnSave" Text="保存" runat="server" class="btn" UseSubmitBehavior="false"
                                    OnClientClick="DisableButtons()" Visible="false" />
                                <cc1:FSButton ID="btnComplete" runat="server" CssClass="btn" OnClientClick="DisableButtons()"
                                    Text="完成" UseSubmitBehavior="false" Visible="false" />
                                <cc1:FSButton ID="btnCancel" Text="撤销" runat="server" class="btn" UseSubmitBehavior="false"
                                    Visible="false" />
                                <cc1:FSButton ID="btnBack" runat="server" class="btn" OnClientClick="DisableButtons()"
                                    Text="退回" UseSubmitBehavior="false" Visible="false" />
                                <cc1:FSButton ID="btnCheckDraft" Text="提交审稿" runat="server" class="btn" UseSubmitBehavior="false"
                                    OnClientClick="DisableButtons()" Visible="false" />
                                <cc1:FSButton ID="btnDeptSign" runat="server" class="btn" OnClientClick="DisableButtons()"
                                    Text="部门会签" UseSubmitBehavior="false" Visible="false" />
                                <cc1:FSButton ID="btnLeadSign" runat="server" class="btn" OnClientClick="DisableButtons()"
                                    Text="领导会签" UseSubmitBehavior="false" Visible="false" />
                                <cc1:FSButton ID="btnVerify" runat="server" class="btn" OnClientClick="DisableButtons()"
                                    Text="提交核稿" UseSubmitBehavior="false" Visible="false" />
                                <cc1:FSButton ID="btnSign" runat="server" class="btn" OnClientClick="DisableButtons()"
                                    Text="提交签发" UseSubmitBehavior="false" Visible="false" />
                                <cc1:FSButton ID="btnCheck" runat="server" class="btn" OnClientClick="DisableButtons()"
                                    Text="提交校对" UseSubmitBehavior="false" Visible="false" />
                                <cc1:FSButton ID="btnCompleteAll" runat="server" class="btn" Text="完成归档" UseSubmitBehavior="false"
                                    Visible="false" />
                                <uc:Print ID="ucPrint" runat="server" />
                                <cc1:FSButton ID="btn_GuiDang" runat="server" CssClass="btn" Text="归档" UseSubmitBehavior="false"
                                    Visible="false" onclick="btn_GuiDang_Click" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </td>
        <td id="TdSign" valign="top" runat="server" visible="false" style="border: 1px solid #64b7d7;
            text-align: center;">
            <div style="height: 20px; padding: 10px 0px; background: #deeef4; border: #64b7d7 1px solid;
                font-weight: bold; width: 220px;">
                <%=StepName%></div>
            <div style="padding: 5px;">
                已会签人：</div>
            <cc1:FSTextBox ID="txtCounterSigners" Height="80px" Width="212px" TextMode="MultiLine"
                CssClass="textarea_bgreen" runat="server" ReadOnly="True">
            </cc1:FSTextBox>
            <div style="padding: 5px;">
                意 见：</div>
            <cc1:FSTextBox ID="txtComment" Height="50px" Width="212px" TextMode="MultiLine" CssClass="textarea_yello"
                runat="server" MaxLength="200">
            </cc1:FSTextBox>
            <div class="divSubmit">
                <cc1:FSButton ID="btnSaveSign" Text="保存" runat="server" class="btn" UseSubmitBehavior="false"
                    OnClientClick="DisableButtons(this)" Visible="false" />
                <cc1:FSButton ID="btnCompleteSign" Text="完成" runat="server" class="btn" UseSubmitBehavior="false"
                    OnClientClick="DisableButtons(this)" Visible="false" />
                <cc1:FSButton ID="btnDistribution" runat="server" class="btn" OnClientClick="DisableButtons(this)"
                    Text="签发" UseSubmitBehavior="false" Visible="false" />
                <cc1:FSButton ID="btnBackVerify" runat="server" class="btn" OnClientClick="DisableButtons(this)"
                    Text="退回" UseSubmitBehavior="false" Visible="false" />
            </div>
        </td>
    </tr>
</table>
<div style="display: none">
    是否核稿退回<asp:TextBox ID="wfIsDeny" runat="server"></asp:TextBox>
    部门会签负责人s<asp:TextBox ID="wfDeptSignLeaders" runat="server"></asp:TextBox>
    部门会签负责人IDs<asp:TextBox ID="wfDeptSignLeaderIDs" runat="server"></asp:TextBox>
    会签部门IDs<asp:TextBox ID="wfDeptSignIDs" runat="server"></asp:TextBox>
    公司会签领导IDs<asp:TextBox ID="wfLeaderSignIDs" runat="server"></asp:TextBox>
    拟稿人ID<asp:TextBox ID="wfDrafterID" runat="server"></asp:TextBox>
    核稿人ID<asp:TextBox ID="wfDraftCheckerID" runat="server"></asp:TextBox>
    审核人ID<asp:TextBox ID="wfVerifierID" runat="server"></asp:TextBox>
    分发人IDs<asp:TextBox ID="wfDistributeUserID" runat="server"></asp:TextBox>
</div>
