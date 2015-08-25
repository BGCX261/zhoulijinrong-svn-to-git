<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_LetterReceived.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.Receive.UC_LetterReceived" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../../PageOU/UC_Role.ascx" TagName="Role" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_FileControl.ascx" TagName="FileControl" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_OASelect.ascx" TagName="OUSelect" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_Print.ascx" TagName="Print" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_CommentList.ascx" TagName="CommentList" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_Company.ascx" TagName="Company" TagPrefix="uc" %>
<style>
    fieldset { padding-bottom: 5px; }
    LEGEND { font-size: 14px; }
    LEGEND SPAN { font-size: 14px; }
</style>

<script type="text/javascript">
    var title = "（待办）";

    if ("<%=base.IsPreview%>".toLowerCase() == "true") {
        if ("<%=base.EntryAction%>" == "2") {
            title = "（已被他人获取）";
        } else {
            if ("<%=base.EntryAction%>" != "3") {
                title = "（已办）";
            } else {
                if ('<%=Request.QueryString["IsRead"]%>'.toLowerCase() == "true") {
                    title = "（已阅）";
                } else {
                    title = "（待阅）";
                }
            }
        }
    }
    $("containTitle").innerHTML = "<%=TemplateName%> - " + "<%=StepName%>" + title;
</script>

<script type="text/javascript">
    function checkChuanYue() {
        if ($("<%=this.txtCirculateDeptName.ClientID%>").value == "" && $("<%=this.txtCirculateLeader.ClientID%>").value == "") {
            return confirm("没有选择传阅部门/人员是否继续?");
        }
        return true;
    }
    function checkUnderDept() {
        if ($("<%=this.txtUnderTake.ClientID%>").value == "") {
            return confirm("没有选择承办部门是否继续?");
        }
        return true;
    }
</script>

<div class="divCenter">
    <table runat="server" id="tblPlot" visible="false">
        <tr>
            <td valign="top">
                <div style="background: #deeef4; border: #64b7d7 1px solid; text-align: center; font-weight: bold;
                    height: 25px; line-height: 25px;">
                    领导批示</div>
                <div style="border: 1px solid #64b7d7;">
                    <cc1:FSTextBox ID="txtLeaderComment" Style="width: 600px; height: 90px;" TextMode="MultiLine"
                        CssClass="txtbox_blue" runat="server" ToolTip="2000字符以内"></cc1:FSTextBox>
                    <div id='divLeaderPlot'>
                        <cc1:FSButton ID="btnInstructionSave" Text="保存草稿" runat="server" class="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons(this)" OnClick="SubmitEvents" />
                        <cc1:FSButton ID="btnComplete" Text="提交" runat="server" class="btn" UseSubmitBehavior="false"
                            OnClientClick="DisableButtons(this)" OnClick="SubmitEvents" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:Panel ID="FormPanel" runat="server" Style="width: 700px; text-align: left">
        <fieldset>
            <legend>收文基本信息</legend>
            <table class="table_add_mid offsetRight1" cellpadding="2">
                <tr style="height: 25px;">
                    <td class="leftSpan1">
                        <span class="label_title_bold">收文号：</span>
                    </td>
                    <td style="width: 145px;">
                        <cc1:FSTextBox ID="txtReceiveNo" runat="server" CssClass="txtPreview" TabIndex="-1"
                            Style="width: 135px">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                    </td>
                    <td style="width: 75px;">
                        <span class="label_title_bold">收文日期：</span>
                    </td>
                    <td style="width: 140px;">
                        <cc1:FSTextBox ID="txtReceiveTime" runat="server" CssClass="txtPreview" TabIndex="-1"
                            Style="width: 130px">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                        <cc1:FSCalendar ID="txtReceiveDate" runat="server" CssClass="txtPreview" Style="width: 135px"
                            Visible="False" />
                    </td>
                    <td style="width: 75px;">
                        <span class="label_title_bold">紧急程度：</span>
                    </td>
                    <td style="width: 120px;">
                        <cc1:FSTextBox ID="txtUrgentDegree" runat="server" CssClass="txtPreview" TabIndex="-1"
                            Style="width: 123px">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                        <cc1:FSDropDownList ID="ddlJinJi" runat="server" CssClass="txtPreview" Width="114px"
                            Visible="False">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>普通</asp:ListItem>
                            <asp:ListItem>紧急</asp:ListItem>
                        </cc1:FSDropDownList>
                    </td>
                </tr>
                <tr style="height: 25px;">
                    <td>
                        <span class="label_title_bold">文件编码：</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFileEncoding" runat="server" CssClass="txtPreview" TabIndex="-1"
                            Style="width: 135px"></asp:TextBox>
                    </td>
                    <td>
                        <span class="label_title_bold">来文单位：</span>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtCommunicationUnit" runat="server" CssClass="txtPreview" Style="width: 363px"
                            TabIndex="-1"></asp:TextBox>
                        <uc:Company ID="ucCompany" runat="server" Visible="False" />
                    </td>
                </tr>
                <tr style="height: 25px;">
                    <td>
                        <span class="label_title_bold">文件标题：</span>
                    </td>
                    <td colspan="5">
                        <asp:TextBox ID="txtDocumentTitle" runat="server" CssClass="txtPreview" Style="width: 583px"
                            TabIndex="-1"></asp:TextBox>
                    </td>
                </tr>
                <tr style="height: 25px;">
                    <td>
                        <span class="label_title_bold">备注：</span>
                    </td>
                    <td colspan="5">
                        <cc1:FSTextBox ID="txtRemark" runat="server" CssClass="txtPreview" ReadOnly="true"
                            TabIndex="-1" Style="width: 583px"></cc1:FSTextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>附件</legend>
            <table class="table_add_mid offsetRight1" cellpadding="2">
                <tr>
                    <td class="leftSpan1">
                        <span class="label_title_bold">附件：</span>
                    </td>
                    <td style="width: 600px">
                        <uc:FileControl ID="ucAttachment" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>拟办</legend>
            <table class="table_add_mid offsetRight1" cellpadding="2" style="table-layout: fixed">
                <tr>
                    <td class="leftSpan1">
                        <span class="label_title_bold">拟办人：</span>
                    </td>
                    <td style="width: 152px;">
                        <cc1:FSTextBox ID="txtPlotMember" runat="server" CssClass="txtbox_yellow" Style="width: 120px"
                            ToolTip="函件拟办组">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                        <asp:Label ID="lbPlotMember" runat="server" Text="" Visible="false"></asp:Label>
                        <uc:Role ID="ucRole" runat="server" />
                    </td>
                    <td style="width: 70px;">
                        <span class="label_title_bold">拟办时间：</span>
                    </td>
                    <td style="width: 152px;">
                        <cc1:FSTextBox ID="txtPlotTime" runat="server" Style="width: 120px;" TabIndex="-1"
                            CssClass="txtbox_blue">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                        <asp:Label ID="lbPlotTime" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                    <td style="width: 70px;">
                        <span class="label_title_bold">批示领导：</span>
                    </td>
                    <td style="width: 144px">
                        <cc1:FSDropDownList ID="ddlLeadShip" CssClass="dropdownlist_yellow" runat="server"
                            Width="122px">
                        </cc1:FSDropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="label_title_bold">传阅领导：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtCirculateLeader" runat="server" CssClass="txtbox_yellow" Style="width: 120px;">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                        <uc:OUSelect ID="OASelectUC1" runat="server" />
                    </td>
                    <td>
                        <span class="label_title_bold">传阅部门：</span>
                    </td>
                    <td colspan="3">
                        <cc1:FSTextBox ID="txtCirculateDeptName" runat="server" CssClass="txtbox_yellow"
                            Style="width: 340px;">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="label_title_bold">承办部门：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtUnderTake" runat="server" CssClass="txtbox_yellow" Style="width: 120px">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox><uc:OUSelect ID="OASelectUC2" runat="server" />
                    </td>
                    <td>
                        <span class="label_title_bold">协办部门：</span>
                    </td>
                    <td colspan="3">
                        <cc1:FSTextBox ID="txtAssistance" runat="server" CssClass="txtbox_yellow" Style="width: 340px"
                            ToolTip="部门领导">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox><uc:OUSelect ID="OASelectUC3" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="label_title_bold">其他意见：</span>
                    </td>
                    <td colspan="5">
                        <cc1:FSTextBox ID="txtPlotComment" runat="server" CssClass="txtbox_yellow" Style="width: 562px;">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>领导批示</legend>
            <table class="table_add_mid offsetRight1" cellpadding="2">
                <tr>
                    <td class="leftSpan1">
                        <span class="label_title_bold">公司领导：</span>
                    </td>
                    <td style="width: 152px;">
                        <cc1:FSTextBox ID="txtLeadShipName" runat="server" Style="width: 120px;" TabIndex="-1"
                            CssClass="txtbox_blue">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                         <asp:Label ID="lbLeadShipName" runat="server" Text="" Visible="false"></asp:Label>
                   </td>
                    <td style="width: 70px;" rowspan="2">
                        <span class="label_title_bold">意见：</span>
                    </td>
                    <td rowspan="2">
                        <cc1:FSTextBox ID="txtLSComment" runat="server" Style="width: 352px; height: 50px;"
                            TextMode="MultiLine" CssClass="txtbox_yellow">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="label_title_bold">批示时间：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtLeadShipTime" runat="server" Style="width: 120px;" TabIndex="-1"
                            CssClass="txtbox_blue">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                        <asp:Label ID="lbLeadShipTime" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>
                <asp:Label ID="lblUnderTakeTitle" runat="server" Text="承办"></asp:Label>
            </legend>
            <table class="table_add_mid offsetRight1" cellpadding="2">
                <tr>
                    <td class="leftSpan1">
                        <asp:Label ID="lblAssignLable" CssClass="label_title_bold" runat="server" Text="承办部门："></asp:Label>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtAssign" runat="server" CssClass="txtbox_yellow" Style="width: 120px">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                    </td>
                   
                </tr>
                <tr>
                    <td class="leftSpan1">
                        <span class="label_title_bold">部门领导：</span>
                    </td>
                    <td style="width: 152px;">
                        <cc1:FSTextBox ID="txtUDDeptLeader" runat="server" Style="width: 120px;" TabIndex="-1"
                            CssClass="txtbox_blue">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                        <asp:Label ID="lblchushi" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                    <td style="width: 70px;">
                        <span class="label_title_bold">科室领导：</span>
                    </td>
                    <td style="width: 152px;">
                        <cc1:FSTextBox ID="txtUDSectionLeader" runat="server" Style="width: 120px;" TabIndex="-1"
                            CssClass="txtbox_blue" ToolTip="部门负责人">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                        <uc:OUSelect ID="OASelectUC4" runat="server" />
                        <asp:Label ID="lblkeshi" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td style="width: 80px;">
                        <span class="label_title_bold">直接承办人：</span>
                    </td>
                    <td style="width: 144px">
                        <cc1:FSTextBox ID="txtUDPeople" runat="server" Style="width: 120px;" TabIndex="-1"
                            CssClass="txtbox_blue">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                        <uc:OUSelect ID="OASelectUC5" runat="server" />
                        <asp:Label ID="lblrenyuan" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="label_title_bold">时间：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtUDDeptLeaderTime" runat="server" CssClass="txtbox_blue" Style="width: 120px;"
                            TabIndex="-1">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                        <asp:Label ID="lbUDDetpLeaderTime" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <span class="label_title_bold">时间：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtUDSectionLeaderTime" runat="server" CssClass="txtbox_blue"
                            Style="width: 120px;" TabIndex="-1">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                        <asp:Label ID="lbUDSectionLeaderTime" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <span class="label_title_bold">时间：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtUDPeopleTime" runat="server" CssClass="txtbox_blue" Style="width: 120px;"
                            TabIndex="-1">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                        <asp:Label ID="lbUDPeopleTime" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                 <td>
                        <span class="label_title_bold">承办意见：</span>
                    </td>
                    <td colspan="5">
                        <cc1:FSTextBox ID="txtUnderTakeComment" runat="server" Style="width: 575px;height: 50px;"
                                TextMode="MultiLine" CssClass="txtbox_blue">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset style="display: none">
            <legend>提示信息</legend>
            <table class="table_add_mid offsetRight1" cellpadding="2">
                <tr>
                    <td class="leftSpan1">
                        <span class="label_title_bold">提示信息：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtPrompt" CssClass="txtbox_blue" ReadOnly="True" TabIndex="-1"
                            runat="server" Style="width: 600px; height: 90px;" TextMode="MultiLine">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                        <cc1:FSTextBox ID="txtPromptEdit" runat="server" Style="width: 600px;" MaxLength="150">
                        &nbsp;&nbsp;
                        </cc1:FSTextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>意见列表</legend>
            <table class="table_add_mid offsetRight1" cellpadding="2">
                <tr>
                    <td class="leftSpan1">
                        <span class="label_title_bold">意见：</span>
                    </td>
                    <td style="width: 600px;">
                        <uc:CommentList ID="ucCommentList" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <div id="divCirculatePerson" runat="server" visible="false">
            <fieldset>
                <legend>传阅</legend>
                <table class="table_add_mid offsetRight1" cellpadding="2">
                    <tr>
                        <td class="leftSpan1">
                            <span class="label_title_bold">分发范围：</span>
                        </td>
                        <td>
                            <cc1:FSTextBox ID="txtCirculatePerson" runat="server" Style="width: 258px;" CssClass="txtbox_yellow">
                            &nbsp;&nbsp;
                            </cc1:FSTextBox>
                            <uc:OUSelect ID="OASelectUC6" runat="server" />
                            <cc1:FSButton ID="btnCY" runat="server" CssClass="btn" OnClick="SubmitEvents" OnClientClick="DisableButtons()" UseSubmitBehavior="false" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 90px;">
                            <span class="label_title_bold">传阅意见：</span>
                        </td>
                        <td>
                            <cc1:FSTextBox ID="txtCirculateComment" runat="server" Style="width: 583px; height: 30px;"
                                TextMode="MultiLine" CssClass="txtbox_yellow">
                            &nbsp;&nbsp;
                            </cc1:FSTextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div class="divSubmit" id="divSubmit" style="text-align: center">
            <cc1:FSButton ID="btnGuidang" runat="server" CssClass="btn" UseSubmitBehavior="false"
                Visible="false" OnClick="btnGuidang_Click" Text="归档" />
            <cc1:FSButton ID="btnCommon1" runat="server" CssClass="btn" UseSubmitBehavior="false"
                Visible="false" OnClientClick="DisableButtons()" OnClick="SubmitEvents" />
            <cc1:FSButton ID="btnCommon2" runat="server" CssClass="btn" UseSubmitBehavior="false"
                Visible="false" OnClientClick="DisableButtons()" OnClick="SubmitEvents" />
            <cc1:FSButton ID="btnCommon3" runat="server" CssClass="btn" UseSubmitBehavior="false"
                Visible="false" OnClientClick="DisableButtons()" OnClick="SubmitEvents" />
            <cc1:FSButton ID="btnCommon4" runat="server" CssClass="btn" UseSubmitBehavior="false"
                Visible="false" OnClientClick="DisableButtons()" OnClick="SubmitEvents" />
            <cc1:FSButton ID="btnCommon5" runat="server" CssClass="btn" UseSubmitBehavior="false"
                Visible="false" OnClientClick="DisableButtons()" OnClick="SubmitEvents" />
            <uc:Print ID="ucPrint" runat="server" />
        </div>
    </asp:Panel>
</div>
<div style="display: none">
    隐藏区域,记录流程数据<br />
    <cc1:FSTextBox ID="txtUnderTakeID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtDrafter" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtDraftDate" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtPlotMemberID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtSecondPlotID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtAssistID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtAssignID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtAssignPersonID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtCirculateDeptID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtCirculateLeaderID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtCirculatePersonID" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtPageNum" runat="server">
    </cc1:FSTextBox>
    <cc1:FSTextBox ID="txtShiefID" runat="server">
    </cc1:FSTextBox>
</div>
