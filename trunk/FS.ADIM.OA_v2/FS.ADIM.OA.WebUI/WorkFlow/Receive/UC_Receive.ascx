<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Receive.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.Receive.UC_Receive" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../../PageWF/UC_CommentList.ascx" TagName="CommentList" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_FileControl.ascx" TagName="FileControl" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_Print.ascx" TagName="Print" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_OASelect.ascx" TagName="OASelect" TagPrefix="uc" %>
<div style="display: none">
    隐藏区域,记录流程数据<br />
    <cc1:FSTextBox ID="txtUnderTakeDeptID" runat="server"></cc1:FSTextBox>
    <cc1:FSTextBox ID="txtDrafter" runat="server"></cc1:FSTextBox>
    <cc1:FSTextBox ID="txtDraftDate" runat="server"></cc1:FSTextBox>
    <cc1:FSTextBox ID="txtCirculateDeptID" runat="server"></cc1:FSTextBox>
    <cc1:FSTextBox ID="txtCirculatePeopleID" runat="server"></cc1:FSTextBox>
    <cc1:FSTextBox ID="txtUrgentDegree" runat="server"></cc1:FSTextBox>
</div>

<script type="text/javascript">
    function checkChuanYue() {
        if ($('<%= txtCirculateDeptName.ClientID %>').value == "" && $('<%= txtCirculatePeopleName.ClientID %>').value == "") {
            alert("分发:没有选择分发范围！");
            return false;
        } else {
            return true;
        }
    }
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

<div class="divCenter">
    <table runat="server" id="tblPlot" visible="false">
        <tr>
            <td valign="top">
                <div style="background: #deeef4; border: #64b7d7 1px solid; text-align: center; font-weight: bold;
                    height: 25px; line-height: 25px;">
                    领导批示</div>
                <div style="border: 1px solid #64b7d7;">
                    <cc1:FSTextBox ID="txtLeadCommentEdit" Style="width: 600px; height: 90px;" TextMode="MultiLine"
                        CssClass="txtbox_yellow" runat="server" ToolTip="2000字符以内"></cc1:FSTextBox>
                    <div id='divLeaderPlot'>
                        <cc1:FSButton ID="btnInstructionSave" Text="保存" runat="server" class="btn" OnClick="btnSaveDraft_Click"
                            UseSubmitBehavior="false" OnClientClick="DisableButtons(this)" />
                        <cc1:FSButton ID="btnComplete" Text="提交" runat="server" class="btn" OnClick="btnSubmit_Click"
                            UseSubmitBehavior="false" OnClientClick="DisableButtons(this)" Style="margin-right: 3px;" /></div>
                </div>
            </td>
        </tr>
    </table>
    <asp:Panel ID="FormPanel" runat="server" Style="width: 722px; text-align: left">
        <div class="divSection">
            <div class="divTitle">
                收文基本信息
            </div>
            <table style="margin-left: 10px;" cellpadding="2">
                <tr>
                    <td style="width: 75px;">
                        <span class="label_title_bold">收文号：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtReceiveNo" runat="server" CssClass="txtPreview" TabIndex="-1"
                            ReadOnly="True" Style="width: 141px">
                        </cc1:FSTextBox>
                    </td>
                    <td style="width: 75px; padding-left: 15px;">
                        <span class="label_title_bold">收文日期：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtReceiveDate" runat="server" CssClass="txtPreview" TabIndex="-1"
                            ReadOnly="True" Style="width: 141px">
                        </cc1:FSTextBox>
                    </td>
                    <td style="width: 60px; padding-left: 15px;">
                        <span class="label_title_bold">原文号：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtSendLetterNo" runat="server" CssClass="txtPreview" TabIndex="-1"
                            ReadOnly="True" Style="width: 141px">
                        </cc1:FSTextBox>
                        <asp:TextBox ID="txtCommunicationUnit" runat="server" CssClass="txtPreview" Style="width: 363px"
                            TabIndex="-1" Visible="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="label_title_bold">卷号：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtPreVolumeNo" runat="server" CssClass="txtPreview" TabIndex="-1"
                            ReadOnly="True" Style="width: 141px; text-align: left;">
                        </cc1:FSTextBox>
                    </td>
                    <td style="padding-left: 15px;">
                        <span class="label_title_bold">文件名称：</span>
                    </td>
                    <td colspan="3">
                        <cc1:FSTextBox ID="txtDocumentTitle" runat="server" CssClass="txtPreview" TabIndex="-1"
                            ReadOnly="True" Style="width: 388px">
                        </cc1:FSTextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divSection">
            <div class="divTitle">
                拟办
            </div>
            <table class="table_add_mid offsetRight" cellpadding="2">
                <tr>
                    <td style="width: 90px;">
                        <span class="label_title_bold">党群工作处处长：</span>
                    </td>
                    <td>
                        <cc1:FSDropDownList ID="ddlPoliticalOfficer" CssClass="dropdownlist_yellow" runat="server"
                            Width="180px">
                        </cc1:FSDropDownList>
                        <asp:Label ID="lbParty" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="label_title_bold">意见：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtPoliticalOfficerComment" runat="server" CssClass="txtbox_yellow"
                            Style="width: 582px; height: 30px;" TextMode="MultiLine"></cc1:FSTextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divSection">
            <div class="divTitle">
                领导批示
            </div>
            <table class="table_add_mid offsetRight" cellpadding="2">
                <tr>
                    <td style="width: 90px;">
                        <span class="label_title_bold">公司领导：</span>
                    </td>
                    <td>
                        <cc1:FSDropDownList ID="ddlLeadShip" CssClass="dropdownlist_yellow" runat="server"
                            Width="180px" ToolTip="公司领导">
                        </cc1:FSDropDownList>
                        <asp:Label ID="lbLeadShip" runat="server" Text="" Visible="false"></asp:Label>
                   </td>
                </tr>
                <tr>
                    <td>
                        <span class="label_title_bold">意见：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtLeadCommentView" runat="server" CssClass="txtbox_blue" Style="width: 582px;
                            height: 30px;" TextMode="MultiLine" TabIndex="-1" ReadOnly="true">
                        </cc1:FSTextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divSection">
            <div class="divTitle">
                承办
            </div>
            <table class="table_add_mid offsetRight" cellpadding="2">
                <tr id="trUnderTakeDept" runat="server">
                    <td style="width: 90px;">
                        <span class="label_title_bold">承办部门：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtUnderTakeDeptName" runat="server" CssClass="txtbox_yellow"
                            Style="width: 294px" ToolTip="部门领导">
                        </cc1:FSTextBox>
                        <uc:OASelect ID="ucUnderTakeDept" runat="server" Visible="False" />
                    </td>
                </tr>
                <tr id="trUnderTakeSection" runat="server" visible="false">
                    <td style="width: 90px;">
                        <span class="label_title_bold">承办科室：</span>
                    </td>
                    <td>
                        <cc1:FSDropDownList ID="ddlUnderTakeSection" runat="server" CssClass="dropdownlist_yellow"
                            Width="180px">
                        </cc1:FSDropDownList>
                    </td>
                </tr>
                <tr id="trUnderTakeMember" runat="server" visible="false">
                    <td style="width: 90px;">
                        <span class="label_title_bold">承办人员：</span>
                    </td>
                    <td>
                        <cc1:FSDropDownList ID="ddlUnderTakePeople" runat="server" CssClass="dropdownlist_yellow"
                            Width="180px">
                        </cc1:FSDropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 90px;">
                        <span class="label_title_bold">承办意见：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtUnderTakeCommentEdit" runat="server" CssClass="txtbox_yellow"
                            Style="width: 582px; height: 30px;" TextMode="MultiLine"></cc1:FSTextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divSection">
            <div class="divTitle">
                承办意见列表
            </div>
            <table class="table_add_mid offsetRight" cellpadding="2">
                <tr>
                    <td style="width: 90px;">
                        <span class="label_title_bold">意见：</span>
                    </td>
                    <td style="width: 600px;">
                        <uc:CommentList ID="ucCommentList" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="divSection">
            <div class="divTitle">
                提示信息
            </div>
            <table class="table_add_mid offsetRight" cellpadding="2">
                <tr>
                    <td style="width: 90px;">
                        <span class="label_title_bold">提示信息：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtPrompt" runat="server" Style="width: 582px; height: 90px;"
                            CssClass="txtbox_blue" ReadOnly="true" TabIndex="-1" TextMode="MultiLine">
                        </cc1:FSTextBox>
                        <cc1:FSTextBox ID="txtPromptEdit" runat="server" CssClass="txtbox_yellow" MaxLength="150"
                            Style="width: 582px;" ToolTip="最多150字符">
                        </cc1:FSTextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divSection">
            <div class="divTitle">
                附件
            </div>
            <table class="table_add_mid offsetRight" cellpadding="2">
                <tr>
                    <td style="width: 90px;">
                        <span class="label_title_bold">附件：</span>
                    </td>
                    <td style="width: 600px;">
                        <uc:FileControl ID="ucAttachment" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="divSection" id="divCirculates" runat="server" visible="false">
            <div class="divTitle">
                分发
            </div>
            <table class="table_add_mid offsetRight" cellpadding="2">
                <tr>
                    <td style="width: 90px;">
                        <span class="label_title_bold">分发范围：</span>
                    </td>
                    <td>
                        <div>
                            部门:</div>
                        <cc1:FSTextBox ID="txtCirculateDeptName" runat="server" CssClass="txtbox_yellow"
                            Style="width: 560px; height: 30px;" TabIndex="-1" TextMode="MultiLine"></cc1:FSTextBox>
                        <div>
                            人员:</div>
                        <cc1:FSTextBox ID="txtCirculatePeopleName" runat="server" CssClass="txtbox_yellow"
                            Style="width: 560px; height: 30px;" TabIndex="-1" TextMode="MultiLine"></cc1:FSTextBox><uc:OASelect
                                ID="ucCirculatePeople" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="divSubmit" id="divSubmit">
            <cc1:FSButton ID="btnSumitInspect" runat="server" CssClass="btn" Text="提交批阅" OnClick="btnSumitInspect_Click"
                UseSubmitBehavior="false" OnClientClick="DisableButtons()" Visible="false" />
            <cc1:FSButton ID="btnSubmitInstruct" runat="server" CssClass="btn" Text="提交批示" OnClick="btnSubmitInstruct_Click"
                UseSubmitBehavior="false" OnClientClick="DisableButtons()" Visible="false" />
            <cc1:FSButton ID="btnSubmitUnderTake" runat="server" CssClass="btn" Text="提交承办" OnClick="btnSubmitUnderTake_Click"
                UseSubmitBehavior="false" Visible="false" />
            <cc1:FSButton ID="btnAssignChief" runat="server" CssClass="btn" Text="交办科室" OnClick="btnAssignChief_Click"
                UseSubmitBehavior="false" OnClientClick="DisableButtons()" Visible="false" />
            <cc1:FSButton ID="btnAssignMember" runat="server" CssClass="btn" Text="交办人员" OnClick="btnAssignMember_Click"
                UseSubmitBehavior="false" OnClientClick="DisableButtons()" Visible="false" />
            <cc1:FSButton ID="btnCirculate" runat="server" CssClass="btn" Text="追加分发" UseSubmitBehavior="false"
                OnClick="btnCirculate_Click" Visible="false" OnClientClick="if(!checkChuanYue()){return false;}else{DisableButtons();} " />
            <cc1:FSButton ID="btnSubmit" runat="server" CssClass="btn" Text="完成" OnClick="btnSubmit_Click"
                UseSubmitBehavior="false" OnClientClick="DisableButtons()" Visible="false" 
                style="height: 26px" />
            <cc1:FSButton ID="btnDeny" runat="server" CssClass="btn" Text="退回" OnClick="btnDeny_Click"
                UseSubmitBehavior="false" OnClientClick="DisableButtons()" Visible="false" />
            <cc1:FSButton ID="btnCancel" runat="server" CssClass="btn" Text="撤销" OnClick="btnCancel_Click"
                UseSubmitBehavior="false" OnClientClick="DisableButtons()" Visible="false" />
            <cc1:FSButton ID="btnSaveDraft" runat="server" CssClass="btn" Text="保存草稿" OnClick="btnSaveDraft_Click"
                UseSubmitBehavior="false" OnClientClick="DisableButtons()" Visible="false" />
            <cc1:FSButton ID="btnArchive" runat="server" CssClass="btn" Text="归档" Visible="false"
                UseSubmitBehavior="false" OnClick="btnArchive_Click" />
            <uc:Print ID="ucPrint" runat="server" />
        </div>
    </asp:Panel>
</div>

<script type="text/javascript">
    var Plot = $("divLeaderPlot");
    if (Plot != null) {
        var Print = $('btnP');
        Plot.insertAdjacentElement("beforeEnd", Print);
    }
</script>

