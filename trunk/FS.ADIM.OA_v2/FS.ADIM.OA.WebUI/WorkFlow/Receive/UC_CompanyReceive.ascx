<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_CompanyReceive.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.Receive.UC_CompanyReceive" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../../PageWF/UC_CommentList.ascx" TagName="CommentList" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_FileControl.ascx" TagName="FileControl" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_Print.ascx" TagName="Print" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_OASelect.ascx" TagName="OASelect" TagPrefix="uc" %>

<script type="text/javascript">
    function checkChuanYue() {
        if ($('<%= txtCirculateDeptName.ClientID %>').value == "" && $('<%= txtCirculatePeopleName.ClientID %>').value == "") {
            alert("没有选择分发范围！");
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
                    <cc1:FSTextBox ID="txtLeaderComment" Style="width: 600px; height: 90px;" TextMode="MultiLine"
                        CssClass="txtbox_yellow" runat="server" ToolTip="2000字符以内"></cc1:FSTextBox>
                    <div id='divLeaderPlot'>
                        <cc1:FSButton ID="btnComplete" Text="提交" runat="server" class="btn" OnClick="SubmitEvents"
                            UseSubmitBehavior="false" OnClientClick="DisableButtons(this)" />
                        <cc1:FSButton ID="btnInstructionSave" Text="保存" runat="server" class="btn" OnClick="SubmitEvents"
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
        <div class="divSection" id="divVicePresident">
            <div class="divTitle">
                办公室批阅
            </div>
            <div>
                <table class="table_add_mid offsetRight" cellpadding="2">
                    <tr>
                        <td style="width: 90px;">
                            <span class="label_title_bold">公司办主任：</span>
                        </td>
                        <td>
                            <cc1:FSDropDownList ID="ddlOfficer" CssClass="dropdownlist_yellow" runat="server"
                                Width="180px" ToolTip="公司办主任">
                            </cc1:FSDropDownList>
                            <asp:Label ID="lbOfficer" runat="server" Text="" Visible="true"></asp:Label>
                       </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="label_title_bold">意见：</span>
                        </td>
                        <td>
                            <cc1:FSTextBox ID="txtOfficerComment" runat="server" CssClass="txtbox_yellow" Style="width: 582px;
                                height: 30px;" TextMode="MultiLine" ToolTip="2000字符以内">
                            </cc1:FSTextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="divSection">
            <div class="divTitle">
                领导批示
            </div>
            <div>
                <table class="table_add_mid offsetRight" cellpadding="2">
                    <tr>
                        <td style="width: 90px;">
                            <span class="label_title_bold">公司领导：</span>
                        </td>
                        <td>
                            <cc1:FSDropDownList ID="ddlLeadShip" CssClass="dropdownlist_yellow" runat="server"
                                Width="180px" ToolTip="公司领导">
                            </cc1:FSDropDownList>
                            <asp:Label ID="lbLeadShip" runat="server" Text="" Visible="true"></asp:Label>
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
        </div>
        <div class="divSection">
            <div class="divTitle">
                承办
            </div>
            <div>
                <table class="table_add_mid offsetRight" cellpadding="2">
                    <tr>
                        <td style="width: 90px;">
                            <asp:Label ID="lblDeptOrPeoplr" runat="server" Text="承办部门" CssClass="label_title_bold"></asp:Label>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td style="width: 180px">
                                        <cc1:FSTextBox ID="txtUnderTakeDeptName" runat="server" CssClass="txtbox_yellow"
                                            Style="width: 150px" ToolTip="部门领导">
                                        </cc1:FSTextBox>
                                        <uc:OASelect ID="OASelectUC1" runat="server" Visible="False" />
                                    </td>
                                    <td>
                                        <span class="label_title_bold">承办人员：</span>
                                    </td>
                                    <td>
                                        <cc1:FSTextBox ID="txtUnderTakeUserName" runat="server" CssClass="txtbox_yellow"
                                            Style="width: 150px">
                                        </cc1:FSTextBox><uc:OASelect ID="OASelectUC2" runat="server" Visible="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="label_title_bold">承办意见：</span>
                        </td>
                        <td>
                            <cc1:FSTextBox ID="txtUnderTakeComment" runat="server" CssClass="txtbox_yellow" Style="width: 582px;
                                height: 30px;" TextMode="MultiLine">
                            </cc1:FSTextBox>
                        </td>
                    </tr>
                </table>
            </div>
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
            <div>
                <table class="table_add_mid offsetRight" cellpadding="2">
                    <tr>
                        <td style="width: 90px;">
                            <span class="label_title_bold">附件真：</span>
                        </td>
                        <td style="width: 600px;">
                            <uc:FileControl ID="ucAttachment" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="divSection">
            <div class="divTitle">
                意见列表
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
        <div class="divSection" id="divUndertake" runat="server" visible="false">
            <div class="divTitle">
                处室承办
            </div>
            <div>
                <table class="table_add_mid offsetRight" cellpadding="2">
                    <tr>
                        <td id="td1" runat="server" style="width: 90px;">
                            <span class="label_title_bold">科室/人员：</span>
                        </td>
                        <td>
                            <asp:Label ID="lbldept" runat="server" Text="科室："></asp:Label>
                            <cc1:FSDropDownList ID="dllUnderDept" runat="server" CssClass="dropdownlist_yellow"
                                Width="114px">
                            </cc1:FSDropDownList>
                            <asp:Label ID="lblmember" runat="server" Text="人员："></asp:Label>
                            <cc1:FSDropDownList ID="ddlUnderPeople" runat="server" CssClass="dropdownlist_yellow"
                                Width="114px">
                            </cc1:FSDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 90px;">
                            <span class="label_title_bold">意见：</span>
                        </td>
                        <td>
                            <cc1:FSTextBox ID="txtUnderComment" runat="server" CssClass="txtbox_blue" Style="width: 582px;
                                height: 40px;" TabIndex="-1" TextMode="MultiLine" ToolTip="2000字符以内">
                            </cc1:FSTextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="divSubmit" id="divSubmit" style="text-align: center">
            <cc1:FSButton ID="btnDistribute" runat="server" CssClass="btn" UseSubmitBehavior="false"
                Text="分发" Visible="false" OnClientClick="DisableButtons()" OnClick="SubmitEvents" />
            <cc1:FSButton ID="btnGuiDang" runat="server" CssClass="btn" Text="归档" Visible="false"
                 OnClick="btnGuiDang_Click" UseSubmitBehavior="false" />
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
        <div style="display: none">
            隐藏区域,记录流程数据<br />
            <cc1:FSTextBox ID="txtUnderTakeDeptID" runat="server">
            </cc1:FSTextBox>
            <cc1:FSTextBox ID="txtDrafter" runat="server">
            </cc1:FSTextBox>
            <cc1:FSTextBox ID="txtDraftDate" runat="server">
            </cc1:FSTextBox>
            <cc1:FSTextBox ID="txtUnderTakeUserID" runat="server" Style="font-weight: 700">
            </cc1:FSTextBox>
            <cc1:FSTextBox ID="txtCurrStep" runat="server">
            </cc1:FSTextBox>
            <cc1:FSTextBox ID="txtCSUnderUserID" runat="server">
            </cc1:FSTextBox>
            <cc1:FSTextBox ID="txtCdeptID" runat="server">
            </cc1:FSTextBox>
            <cc1:FSTextBox ID="txtCPeopleID" runat="server">
            </cc1:FSTextBox>
            <cc1:FSTextBox ID="txtJinJi" runat="server">
            </cc1:FSTextBox>
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

