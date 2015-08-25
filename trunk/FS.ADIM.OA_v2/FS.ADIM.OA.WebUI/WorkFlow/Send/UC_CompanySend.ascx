<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_CompanySend.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.Send.UC_CompanySend" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../../PageOU/UC_OASelect.ascx" TagName="OASelect" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_Role.ascx" TagName="Role" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_FileControl.ascx" TagName="FileControl" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_Comment.ascx" TagName="Comment" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_Print.ascx" TagName="Print" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_SendCard.ascx" TagName="SendCard" TagPrefix="uc" %>
<script type="text/javascript">
    function checkChuanYue() {
        if ($('<%= txtDeptName.ClientID %>').value == "" && $('<%= txtUserName.ClientID %>').value == "") {
            return confirm("没有选择分发范围,是否继续？");
        } else {
            return true;
        }
    }
    function checkJiXuChuanYue() {
        if ($('<%= txtDeptName1.ClientID %>').value == "" && $('<%= txtUserName1.ClientID %>').value == "") {
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
                            <span class="label_title_darkblue">海南核电有限公司发文单</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table class="table_form_noborder">
                                <tr>
                                    <td>
                                        <span class="label_title_bold">紧急程度：</span>
                                    </td>
                                    <td style="width: 114px">
                                        <cc1:FSDropDownList ID="ddlUrgentDegree" CssClass="dropdownlist_yellow" Style="width: 100px;"
                                            runat="server">
                                            <asp:ListItem>普通</asp:ListItem>
                                            <asp:ListItem>紧急</asp:ListItem>
                                        </cc1:FSDropDownList>
                                    </td>
                                    <td id="TdYearNum" runat="server" style="width: 35%" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <span class="label_title_bold">年度：</span><span id="RedSpan_Year" runat="server" class="label_title_red"
                                                        visible="false">*</span>
                                                </td>
                                                <td>
                                                    <cc1:FSTextBox ID="txtDocumentYear" runat="server" CssClass="txtbox_bgreen" ReadOnly="True"
                                                        Style="width: 50px">
                                                    </cc1:FSTextBox>
                                                </td>
                                                <td>
                                                    <span class="label_title_bold">序号：</span><span id="RedSpan_Num" runat="server" class="label_title_red"
                                                        visible="false">*</span>
                                                </td>
                                                <td>
                                                    <cc1:FSTextBox ID="txtDocumentNum" runat="server" CssClass="txtbox_bgreen" ReadOnly="True"
                                                        Style="width: 50px">
                                                    </cc1:FSTextBox>
                                                </td>
                                            </tr>
                                        </table>
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
                                                    <td style="width: 40px">
                                                        <span class="label_title_bold">会签：</span>
                                                    </td>
                                                    <td rowspan="2">
                                                        <cc1:FSTextBox ID="txtLeadSigners" CssClass="textarea_bgreen" Style="width: 248px;
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
                                            <legend>部门会签</legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span class="label_title_bold">会签：</span>
                                                    </td>
                                                    <td rowspan="2">
                                                        <cc1:FSTextBox ID="txtDeptSigners" CssClass="textarea_bgreen" Style="width: 248px;
                                                            height: 40px" runat="server" TextMode="MultiLine" TabIndex="-1">
                                                        </cc1:FSTextBox>
                                                    </td>
                                                    <td>
                                                        <uc:OASelect ID="ucDeptCounterSigns" runat="server" Visible="false" />
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
                                                        <span class="label_title_bold">签发：</span>
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
                                                        <cc1:FSTextBox ID="txtSignCommentView" CssClass="txtbox_bgreen" Style="width: 400px;
                                                            height: 40px;" runat="server" ReadOnly="True" TextMode="MultiLine">
                                                        </cc1:FSTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="label_title_bold">日期：</span>
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
                                            <legend>办公室核稿</legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span class="label_title_bold">秘书：</span>
                                                    </td>
                                                    <td>
                                                        <cc1:FSTextBox ID="txtSecretaryChecker" CssClass="txtbox_bgreen" Style="width: 50px"
                                                            runat="server" ReadOnly="True">
                                                        </cc1:FSTextBox>
                                                        <asp:Label ID="lbHeGaoRenMiShu" runat="server" Text="" Visible="false"></asp:Label>
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
                                                <tr>
                                                    <td>
                                                        <span class="label_title_bold">主任：</span>
                                                    </td>
                                                    <td>
                                                        <cc1:FSTextBox ID="txtDirectorChecker" CssClass="txtbox_bgreen" Style="width: 50px"
                                                            runat="server" ReadOnly="True">
                                                        </cc1:FSTextBox>
                                                        <asp:Label ID="lbHeGaoZhuRen" runat="server" Text="" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <span class="label_title_bold">日期：</span>
                                                    </td>
                                                    <td>
                                                        <cc1:FSTextBox ID="txtDirectorCheckDate" CssClass="txtbox_bgreen" Style="width: 120px"
                                                            runat="server" ReadOnly="True">
                                                        </cc1:FSTextBox>
                                                        <asp:Label ID="lbHeGaoZhuRenDate" runat="server" Text="" Visible="false"></asp:Label>
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
                                                        <cc1:FSDropDownList ID="ddlVerifier" CssClass="dropdownlist_green" Style="width: 100px"
                                                            runat="server" Enabled="False">
                                                        </cc1:FSDropDownList>
                                                        <asp:Label ID="lbShenGaoRen" runat="server" Text="" Visible="true"></asp:Label>
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
                                                            runat="server" AutoPostBack="True" Enabled="False" ToolTip="拟稿人所属处室" OnSelectedIndexChanged="ddlHostDept_SelectedIndexChanged">
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
                                                    <td
                                                        <span class="label_title_bold">拟稿人：</span>
                                                    </td>
                                                    <td>
                                                        <cc1:FSTextBox ID="txtDrafter" runat="server" CssClass="txtbox_bgreen" Style="width: 96px"
                                                            ReadOnly="True">
                                                        </cc1:FSTextBox>
                                                        <asp:Label ID="lbNiGaoRen" runat="server" Text="" Visible="true"></asp:Label>
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
                            <div style="border-bottom: 1px #ccc dashed">
                            </div>
                            <table class="table_form_noborder" style="width: 100%">
                                <tr>
                                    <td>
                                        <span class="label_title_bold">标题：</span><span id="RedSpanTitle" runat="server" visible="false"
                                            class="label_title_red">*</span>
                                    </td>
                                    <td colspan="5">
                                        <cc1:FSTextBox ID="txtDocumentTitle" runat="server" CssClass="txtbox_yellow" Style="width: 584px"
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
                                        <cc1:FSTextBox ID="txtSubjectWord" runat="server" CssClass="txtbox_yellow" Style="width: 584px"
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
                                        <cc1:FSTextBox ID="txtMainSender" runat="server" CssClass="textarea_yello" Style="width: 584px"
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
                                        <cc1:FSTextBox ID="txtCopySender" runat="server" CssClass="textarea_yello" Style="width: 584px"
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
                                    <td>
                                        <cc1:FSCalendar ID="txtSendDate" CssClass="txtbox_yellow" Style="width: 90px" runat="server">
                                        </cc1:FSCalendar>
                                    </td>
                                    <td colspan="4">
                                        <span class="label">共印</span>
                                        <cc1:FSTextBox ID="txtShareCount" CssClass="txtbox_bgreen" runat="server" Style="width: 30px"
                                            ReadOnly="True" MaxLength="50">
                                        </cc1:FSTextBox><span class="label">份，每份</span><cc1:FSTextBox ID="txtSheetCount"
                                            CssClass="txtbox_bgreen" runat="server" Style="width: 30px" ReadOnly="True" MaxLength="50">
                                        </cc1:FSTextBox><span class="label">张</span>
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
                                    <td>
                                        <cc1:FSTextBox ID="txtTypist" CssClass="txtbox_bgreen" Style="width: 90px" runat="server"
                                            ReadOnly="True" MaxLength="50">
                                        </cc1:FSTextBox>
                                    </td>
                                    <td>
                                        <span class="label_title_bold">校对：</span>
                                    </td>
                                    <td>
                                        <cc1:FSTextBox ID="txtChecker" CssClass="txtbox_bgreen" Style="width: 90px" runat="server"
                                            ReadOnly="True" MaxLength="50">
                                        </cc1:FSTextBox>
                                        <asp:Label ID="lbJiaoDuiRen" runat="server" Text="" Visible="true"></asp:Label>
                                    </td>
                                    <td>
                                        <span class="label_title_bold">复核：</span>
                                    </td>
                                    <td>
                                        <cc1:FSTextBox ID="txtReChecker" CssClass="txtbox_bgreen" Style="width: 90px" runat="server"
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
                                    <td colspan="5" style="width: 600px;">
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
                                    <td colspan="5">
                                        <cc1:FSTextBox ID="txtAllPrompt" CssClass="txtbox_bgreen" Style="width: 298px; height: 60px;"
                                            runat="server" ReadOnly="True" TextMode="MultiLine" TabIndex="-1">
                                        </cc1:FSTextBox>
                                        <cc1:FSTextBox ID="txtMyPrompt" CssClass="txtbox_yellow" Style="width: 276px; height: 60px;"
                                            runat="server" TextMode="MultiLine">
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
                                            <cc1:FSTextBox ID="txtDeptName" runat="server" CssClass="txtbox_yellow" Style="width: 584px;
                                                height: 30px;" TextMode="MultiLine"></cc1:FSTextBox>
                                            <div>
                                                公司领导:</div>
                                            <cc1:FSTextBox ID="txtUserName" runat="server" CssClass="txtbox_yellow" Style="width: 584px;"
                                                TextMode="SingleLine"></cc1:FSTextBox>
                                            <uc:OASelect ID="ucOUCirculate" runat="server" />
                                            <asp:HiddenField ID="hDeptID" runat="server" />
                                            <asp:HiddenField ID="hUserID" runat="server" />
                                        </div>
                                        <div id="trAddFenFa" runat="server" visible="false">
                                            <div>
                                                部门:</div>
                                            <cc1:FSTextBox ID="txtDeptName1" runat="server" CssClass="txtbox_yellow" Style="width: 584px;
                                                height: 30px;" TextMode="MultiLine"></cc1:FSTextBox>
                                            <div>
                                                公司领导:</div>
                                            <cc1:FSTextBox ID="txtUserName1" runat="server" CssClass="txtbox_yellow" Style="width: 584px;"
                                                TextMode="SingleLine"></cc1:FSTextBox>
                                            <uc:OASelect ID="ucOUCirculateAppend" runat="server" />
                                            <asp:HiddenField ID="hDeptID1" runat="server" />
                                            <asp:HiddenField ID="hUserID1" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="divSubmit" id="divSubmit">
                                <cc1:FSButton ID="btnComplete" runat="server" CssClass="btn" OnClientClick="DisableButtons()"
                                    Text="完成" UseSubmitBehavior="false" Visible="false" />
                                <cc1:FSButton ID="btnCheckDraft" runat="server" class="btn" UseSubmitBehavior="false"
                                    Text="提交审稿" OnClientClick="DisableButtons()" Visible="false" />
                                <cc1:FSButton ID="btnDeptSign" runat="server" class="btn" OnClientClick="DisableButtons()"
                                    Text="部门会签" UseSubmitBehavior="false" Visible="false" />
                                <cc1:FSButton ID="btnLeadSign" runat="server" class="btn" OnClientClick="DisableButtons()"
                                    Text="领导会签" UseSubmitBehavior="false" Visible="false" />
                                <cc1:FSButton ID="btnVerify" runat="server" class="btn" OnClientClick="DisableButtons()"
                                    Text="提交核稿" UseSubmitBehavior="false" Visible="false" />
                                <cc1:FSButton ID="btnZhuRenSign" runat="server" class="btn" OnClientClick="DisableButtons()"
                                    Text="主任核稿" UseSubmitBehavior="false" Visible="false" />
                                <cc1:FSButton ID="btnSign" runat="server" class="btn" OnClientClick="DisableButtons()"
                                    Text="提交签发" UseSubmitBehavior="false" Visible="false" />
                                <cc1:FSButton ID="btnCheck" runat="server" class="btn" OnClientClick="DisableButtons()"
                                    Text="提交校对" UseSubmitBehavior="false" Visible="false" />
                                <cc1:FSButton ID="btnCompleteAll" runat="server" UseSubmitBehavior="false" class="btn" Text="完成归档" Visible="false" />
                                <cc1:FSButton ID="btn_GuiDang" runat="server" CssClass="btn" Text="归档" Visible="false"
                                    UseSubmitBehavior="false" OnClick="btn_GuiDang_Click" />
                                <cc1:FSButton ID="btnAddFenFa" runat="server" class="btn" OnClientClick="DisableButtons()"
                                    Text="追加分发" UseSubmitBehavior="false" Visible="False" />
                                <cc1:FSButton ID="btnCancel" runat="server" class="btn" OnClientClick="DisableButtons()"
                                    Text="撤销" UseSubmitBehavior="false" Visible="false" />
                                <cc1:FSButton ID="btnBack" runat="server" class="btn" OnClientClick="DisableButtons()"
                                    Text="退回" UseSubmitBehavior="false" Visible="false" />
                                <cc1:FSButton ID="btnSave" runat="server" class="btn" OnClientClick="DisableButtons()"
                                    Text="保存" UseSubmitBehavior="false" Visible="false" />
                                <uc:Print ID="ucPrint" runat="server" />
                                <uc:SendCard ID="ucSendCard" runat="server" Visible="false" />
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
            <cc1:FSTextBox ID="txtCounterSigners" Height="120px" Width="212px" TextMode="MultiLine"
                CssClass="textarea_bgreen" runat="server" ReadOnly="True">
            </cc1:FSTextBox>
            <div style="padding: 5px;">
                意 见：</div>
            <cc1:FSTextBox ID="txtComment" Height="50px" Width="212px" TextMode="MultiLine" CssClass="textarea_yello"
                runat="server" MaxLength="200">
            </cc1:FSTextBox>
            <div class="divSubmit">
                <cc1:FSButton ID="btnCompleteSign" Text="完成" runat="server" class="btn" UseSubmitBehavior="false"
                    OnClientClick="DisableButtons(this)" Visible="false" />
                <cc1:FSButton ID="btnDistribution" Text="签发" runat="server" class="btn" UseSubmitBehavior="false"
                    OnClientClick="DisableButtons(this)" Visible="false" />
                <cc1:FSButton ID="btnBackVerify" Text="退回" runat="server" class="btn" UseSubmitBehavior="false"
                    OnClientClick="DisableButtons(this)" Visible="false" />
                <cc1:FSButton ID="btnSaveSign" Text="保存" runat="server" class="btn" UseSubmitBehavior="false"
                    OnClientClick="DisableButtons(this)" Visible="false" />
            </div>
        </td>
    </tr>
</table>
<div style="display: none">
    是否核稿退回<asp:TextBox ID="txtIsDeny" runat="server"></asp:TextBox>
    处室会签领导IDs<asp:TextBox ID="txtDeptCounterSignLeaders" runat="server"></asp:TextBox>
    处室会签领导<asp:TextBox ID="txtDeptCounterSignLeaderNames" runat="server"></asp:TextBox>
    会签部门IDs<asp:TextBox ID="txtCounterSignDept" runat="server"></asp:TextBox>
    会签部门负责人IDs<asp:TextBox ID="txtCounterSignDeptLeaders" runat="server"></asp:TextBox>
    公司会签领导IDs<asp:TextBox ID="txtComCounterSignLeaders" runat="server"></asp:TextBox>
    拟稿人ID<asp:TextBox ID="txtDrafterID" runat="server"></asp:TextBox>
    核稿人ID<asp:TextBox ID="txtDraftCheckerID" runat="server"></asp:TextBox>
    审核人ID<asp:TextBox ID="txtVerifierID" runat="server"></asp:TextBox>
    主任核稿人ID<asp:TextBox ID="txtDirectorCheckerID" runat="server"></asp:TextBox>
    分发人IDs<asp:TextBox ID="txtDistributeUserID" runat="server"></asp:TextBox>
</div>
