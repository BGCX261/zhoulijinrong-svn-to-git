<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC2_ProgramFile.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.ProgramFile.UC2_ProgramFile" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../../PageWF/UC_FileControl.ascx" TagName="UCFileControl" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_OASelect.ascx" TagName="OASelectUC" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_Print.ascx" TagName="UCPrint" TagPrefix="uc" %>
<%@ Register Src="UC_LDHuiQian.ascx" TagName="UC_LDHuiQian" TagPrefix="uc" %>
<%@ Register Src="UC_HuiQian.ascx" TagName="UC_HuiQian" TagPrefix="uc" %>
<%@ Register Src="UC_CommentInfo.ascx" TagName="UC_CommentInfo" TagPrefix="uc" %>

<script type="text/javascript">
    $("containTitle").innerHTML = "<%=TemplateName%> - " + "<%=StepName%>";
    function ApproveSubmitConfirm() {

        var hfDeptSignCount = $('<%=hfDeptSignCount.ClientID %>');
        var hfLeaderSignCount = $('<%=hfLeaderSignCount.ClientID %>');
        var programSort = $('<%=wfSort.ClientID %>');
        if (programSort.value == "管理程序" || programSort.value == "部门级管理程序") {
            if (hfDeptSignCount.value != "0" || hfLeaderSignCount.value != "0") {
                if (confirm('在提交批准之前，请确认是否进行会签？\n点击“确定”继续提交批准，点击“取消”，则取消当前操作。') == false) {
                    return false;
                } else {
                    DisableCtrls();
                }
            }
        } else {
            DisableCtrls();
        }
    }
    function OpenConditionDialog(dealCondition, ucID) {
        var encodeValue = encodeURIComponent(dealCondition);
        var isHistory = "<%=IsPreview %>";

        var condition = window.showModalDialog("Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkFlow.ProgramFile.PGAddInfo&content=" + encodeValue + "&ucID=" + ucID + "&isHistory=" + isHistory, "", "dialogWidth:410px;dialogHeight:220px;center:yes;scroll=no;status=no");

        if (typeof (condition) == "undefined") {
            return false;
        } else {
            $(ucID).value = condition;
        }
    }
    function piZhunTiShi() {
        if ('<%=StepName%>' == '批准') {
            return confirm('添加意见表示否决！');
        }
        else {
            return true;
        }
    }
    //根据是否存在交办人判断交办按钮是否执行
    function CheckIsSubmit() {
        var assignMember = $("<%=txtAssignMember.ClientID %>").value;
        if (assignMember == "") {
            alert("请选择交办人员。");
            return false;
        }
        var childProcessID = $("<%=wfChildProcessID.ClientID %>").value;
        var assignedMember = $("<%=wfAssignUserName.ClientID %>").value;
        if (childProcessID != "") {
            alert("已交办" + assignedMember + "。");
            return false;
        } else {
            DisableCtrls();
        }
    }
    //是否忽略子流程提示
    function IsIgnoreChildProcess() {
        var childProcessID = $("<%=wfChildProcessID.ClientID %>").value;
        //alert(childProcessID);
        var strStepName = '<%=StepName%>';
        if (childProcessID != "" && strStepName == "部门会签") {
            if (confirm('在提交之前，请确认是否等待协办人员回复信息？\n点击“确定”继续提交，点击“取消”，则取消当前操作。') == false) {
                return false;
            } else {
                DisableCtrls();
            }
        } else {
            DisableCtrls();
        }
    }
</script>

<asp:ScriptManager ID="ScriptManager" runat="server" EnableScriptGlobalization="false"
    EnableScriptLocalization="false">
</asp:ScriptManager>
<asp:UpdateProgress ID="UpdateProgress1" runat="server">
    <ProgressTemplate>
        <div id="iLoading" style="left: 20px; position: absolute; top: 600px; left: 450px">
            <img id="imgLoading" src="../../Img/loading.gif" alt="" />
        </div>
        <div id="Div1" style="left: 20px; position: absolute; top: 300px; left: 450px">
            <img id="imgLoading" src="../../Img/loading.gif" alt="" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hfDeptSignCount" runat="server" />
        <asp:HiddenField ID="hfLeaderSignCount" runat="server" />
        <uc:UC_CommentInfo ID="ucCommentInfo" runat="server" />
        <div class="divCenter">
            <table class="SheetBorder2" style="margin-top: 5px;">
                <tr>
                    <td style="width: 720px" valign="top">
                        <asp:Panel ID="FormPanel" runat="server" Style="width: 720px; border: #64b7d7 1px solid;">
                            <table class="table_form_border" style="width: 720px">
                                <tr>
                                    <td>
                                        <table class="table_form_noborder">
                                            <tr>
                                                <td>
                                                    <fieldset>
                                                        <legend>变更信息</legend>
                                                        <table>
                                                            <tr>
                                                                <td style="text-align: right;">
                                                                    <span id="Span4" class="label">申请类型：</span>
                                                                </td>
                                                                <td style="text-align: left;">
                                                                    <cc1:FSLabel ID="lblApplyStyle" runat="server"></cc1:FSLabel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right;">
                                                                    <span class="label">编制/修订说明：</span>
                                                                </td>
                                                                <td style="text-align: left;">
                                                                    <cc1:FSTextBox ID="txtWriteExplain" runat="server" CssClass="txtbox_yellow" Height="50px"
                                                                        TextMode="MultiLine" Width="537px"></cc1:FSTextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <fieldset>
                                                        <legend>基本信息</legend>
                                                        <table>
                                                            <tr>
                                                                <td style="text-align: right; width: 65px">
                                                                    <span id="Span3" class="label">程序名称：</span>
                                                                </td>
                                                                <td style="text-align: left;">
                                                                    <cc1:FSTextBox ID="txtName" runat="server" Width="160px" ReadOnly="true" CssClass="txtbox_blue"></cc1:FSTextBox>
                                                                </td>
                                                                <td style="text-align: right;">
                                                                    <span id="Span5" class="label">编码：</span>
                                                                </td>
                                                                <td style="text-align: left;">
                                                                    <cc1:FSTextBox ID="txtCode" runat="server" Width="160px" ReadOnly="true" CssClass="txtbox_blue"></cc1:FSTextBox>
                                                                </td>
                                                                <td style="text-align: right;">
                                                                    <span id="Span6" class="label">版次：</span>
                                                                </td>
                                                                <td style="text-align: left;">
                                                                    <cc1:FSTextBox ID="txtEdition" runat="server" Width="40px" ReadOnly="true" CssClass="txtbox_blue"></cc1:FSTextBox>
                                                                </td>
                                                                <td style="text-align: right;">
                                                                    <span id="Span17" class="label">总页数：</span>
                                                                </td>
                                                                <td style="text-align: left;">
                                                                    <cc1:FSTextBox ID="txtPages" runat="server" Width="40px" MaxLength="3"></cc1:FSTextBox>
                                                                    <cc1:FSLabel ID="needPages" runat="server" Text="*" ForeColor="White"></cc1:FSLabel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <fieldset style="text-align: center; padding-bottom: 10px">
                                                        <legend>编校审批</legend>
                                                        <table class="table_border" style="text-align: center">
                                                            <tr>
                                                                <td style="width: 10%; height: 25px">
                                                                </td>
                                                                <td style="width: 15%">
                                                                    <span id="Span7" class="label">操作人</span>
                                                                </td>
                                                                <td style="width: 12%">
                                                                    <span id="Span8" class="label">同意/否决 </span>
                                                                </td>
                                                                <td style="width: 12%">
                                                                    <span id="Span10" class="label">日期</span>
                                                                </td>
                                                                <td>
                                                                    <span id="Span9" class="label">意见</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 10%; height: 25px">
                                                                    <span id="Span11" class="label">批准</span>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSDropDownList ID="ddlApprove" runat="server" Width="70px" CssClass="dropdownlist_yellow">
                                                                    </cc1:FSDropDownList>
                                                                    <cc1:FSLabel ID="needApprove" runat="server" Text="*" visible='false'></cc1:FSLabel>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSLabel ID="lblApproveAgree" runat="server"></cc1:FSLabel>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSLabel ID="lblApproveDate" runat="server"></cc1:FSLabel>
                                                                </td>
                                                                <td id="tdApprove" runat="server" title="点击查看">
                                                                    <div style="width: 95%; float: left">
                                                                        <cc1:FSLabel ID="lblApproveComment" runat="server"></cc1:FSLabel>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 10%; height: 25px">
                                                                    <span id="Span12" class="label">审核</span>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSDropDownList ID="ddlAudit" runat="server" Width="70px" CssClass="dropdownlist_yellow">
                                                                    </cc1:FSDropDownList>
                                                                    <cc1:FSLabel ID="needAudit" runat="server" Text="*" visible='false'></cc1:FSLabel>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSLabel ID="lblAuditAgree" runat="server"></cc1:FSLabel>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSLabel ID="lblAuditDate" runat="server"></cc1:FSLabel>
                                                                </td>
                                                                <td id="tdAuditComment" runat="server" title="点击查看">
                                                                    <div style="width: 95%; float: left">
                                                                        <cc1:FSLabel ID="lblAuditComment" runat="server"></cc1:FSLabel>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 10%; height: 25px">
                                                                    <span id="Span13" class="label">校核</span>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSTextBox ID="txtCheckName" runat="server" Width="50px"></cc1:FSTextBox>
                                                                    <cc1:FSLabel ID="needCheck" runat="server" Text="*" visible = 'false'></cc1:FSLabel>
                                                                    <uc:OASelectUC ID="ucSelectAuditor" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <cc1:FSLabel ID="lblCheckAgree" runat="server"></cc1:FSLabel>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSLabel ID="lblCheckDate" runat="server"></cc1:FSLabel>
                                                                </td>
                                                                <td id="tdCheckComment" runat="server" title="点击查看">
                                                                    <div style="width: 95%; float: left">
                                                                        <cc1:FSLabel ID="lblCheckComment" runat="server"></cc1:FSLabel>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 10%; height: 25px">
                                                                    <span id="Span14" class="label">编制</span>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSLabel ID="lblWrite" runat="server"></cc1:FSLabel>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSLabel ID="lblWriteAgree" runat="server"></cc1:FSLabel>
                                                                </td>
                                                                <td>
                                                                    <cc1:FSLabel ID="lblWriteDate" runat="server"></cc1:FSLabel>
                                                                </td>
                                                                <td>
                                                                    <div style="width: 95%; float: left">
                                                                        <cc1:FSLabel ID="lblWriteComment" runat="server"></cc1:FSLabel>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <asp:Panel ID="pnlQGRegion" runat="server">
                                                <tr>
                                                    <td>
                                                        <fieldset style="text-align: center">
                                                            <legend>质保审查</legend>
                                                            <table class="table_border" style="margin-bottom: 5px; text-align: center">
                                                                <tr style="height: 25px">
                                                                    <td style="width: 12%">
                                                                        操作人
                                                                    </td>
                                                                    <td style="width: 10%">
                                                                        同意/否决
                                                                    </td>
                                                                    <td style="width: 12%">
                                                                        日期
                                                                    </td>
                                                                    <td>
                                                                        意见
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 25px">
                                                                    <td style="width: 12%">
                                                                        <cc1:FSLabel ID="lblQG" runat="server"></cc1:FSLabel>
                                                                    </td>
                                                                    <td style="width: 10%">
                                                                        <cc1:FSLabel ID="lblQGAgree" runat="server"></cc1:FSLabel>
                                                                    </td>
                                                                    <td style="width: 12%">
                                                                        <cc1:FSLabel ID="lblQGDate" runat="server"></cc1:FSLabel>
                                                                    </td>
                                                                    <td id="tdQulityComment" runat="server" title="点击查看">
                                                                        <div style="width: 95%; float: left">
                                                                            <cc1:FSLabel ID="lblQGComment" runat="server"></cc1:FSLabel>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                            </asp:Panel>
                                            <tr>
                                                <td>
                                                    <div style="text-align: left">
                                                        <asp:CheckBox ID="cbIsPrint" runat="server" Checked="false" Enabled="False" Text="需要工程公司会签" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center">
                                                    <asp:RadioButtonList ID="rdolstSignStyle" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rdolstSignStyle_SelectedIndexChanged"
                                                        RepeatDirection="Horizontal" Visible="false">
                                                        <asp:ListItem Selected="True">部门会签</asp:ListItem>
                                                        <asp:ListItem>领导会签</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlDeptSign" runat="server">
                                                        <fieldset style="text-align: center">
                                                            <legend>部门会签</legend>
                                                            <uc:UC_HuiQian ID="ucBuMenHuiQian" runat="server" />
                                                        </fieldset></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlLeaderSign" runat="server">
                                                        <fieldset style="text-align: center">
                                                            <legend>领导会签 </legend>
                                                            <uc:UC_LDHuiQian ID="ucLDHuiQian" runat="server" />
                                                        </fieldset></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <fieldset style="text-align: center">
                                                        <legend>附件</legend>
                                                        <table style="width: 90%">
                                                            <tr>
                                                                <td style="text-align: right;">
                                                                    <uc:UCFileControl ID="ucFileControl" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlCirculate" runat="server" Visible="false">
                                                        <fieldset style="text-align: center">
                                                            <legend>传阅</legend>
                                                            <table style="width: 90%; height: 30px;">
                                                                <tr>
                                                                    <td>
                                                                        <table class="table_add_mid offsetRight" cellpadding="2" style="float: left">
                                                                            <tr>
                                                                                <td style="width: 90px;">
                                                                                    <span class="label">
                                                                                        <asp:Label ID="lblCriculateDispense" runat="server" Text="传阅范围"></asp:Label>
                                                                                    </span>
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                    部门：<br />
                                                                                    <cc1:FSTextBox ID="txtCirculateDeptName" runat="server" CssClass="txtbox_yellow"
                                                                                        TextMode="MultiLine" Style="width: 378px; height: 50px" Height="30px"></cc1:FSTextBox>
                                                                                    <br />
                                                                                    人员：<br />
                                                                                    <cc1:FSTextBox ID="txtCirculateUserName" runat="server" CssClass="txtbox_yellow"
                                                                                        TextMode="MultiLine" Style="width: 378px; height: 50px"></cc1:FSTextBox>
                                                                                    <uc:OASelectUC ID="ucSelectCirculate" runat="server" />
                                                                                    <asp:HiddenField ID="hfCirculateDeptID" runat="server" />
                                                                                    <asp:HiddenField ID="hfCirculateUserID" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 90px;">
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                    <cc1:FSLabel ID="lblCirculateMsg" runat="server" Text="部门、人员至少一项必须选择数据" ForeColor="Red"></cc1:FSLabel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlSend" runat="server">
                                                        <fieldset style="text-align: center">
                                                            <legend>分发</legend>
                                                            <table style="width: 90%; height: auto">
                                                                <tr>
                                                                    <td>
                                                                        <table class="table_add_mid offsetRight" cellpadding="2" style="float: left">
                                                                            <tr>
                                                                                <td style="width: 90px;">
                                                                                    <span class="label">
                                                                                        <asp:Label ID="lblRange" runat="server" Text="分发范围"></asp:Label>
                                                                                    </span>
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                    部门：<br />
                                                                                    <cc1:FSTextBox ID="txtSendDeptName" runat="server" CssClass="txtbox_yellow" TextMode="MultiLine"
                                                                                        Style="width: 378px; height: 50px" Height="30px"></cc1:FSTextBox>
                                                                                    <br />
                                                                                    人员：<br />
                                                                                    <cc1:FSTextBox ID="txtSendUserName" runat="server" CssClass="txtbox_yellow" TextMode="MultiLine"
                                                                                        Style="width: 378px; height: 50px"></cc1:FSTextBox>
                                                                                    <uc:OASelectUC ID="ucSelectSender" runat="server" />
                                                                                    <asp:HiddenField ID="hfSendDeptID" runat="server" />
                                                                                    <asp:HiddenField ID="hfSendUserID" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 90px;">
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                    <cc1:FSLabel ID="lblPrompt" runat="server" Text="部门、人员至少一项必须选择数据" ForeColor="Red"></cc1:FSLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 90px;">
                                                                                    <asp:Label ID="lblYiJian" runat="server" Text="分发意见:" Visible="False"></asp:Label>
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                    <cc1:FSTextBox ID="txtSendComemnt" runat="server" CssClass="textarea_blue" TextMode="MultiLine"
                                                                                        MaxLength="500" Height="60px" Visible="false" Width="450px" ReadOnly="True"></cc1:FSTextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlDealSign" runat="server">
                                                        <fieldset style="text-align: center">
                                                            <legend>意见信息</legend>
                                                            <table style="width: 90%">
                                                                <tr>
                                                                    <td>
                                                                        <div id="divDept">
                                                                            <asp:Panel ID="pnlDeptComment" runat="server" Visible="false">
                                                                                <div class="dealSignBar">
                                                                                    部门会签意见
                                                                                </div>
                                                                                <div class="dealSignFrame">
                                                                                    <table style="border-top: none; width: 100%;">
                                                                                        <tr class="dealSign_tr">
                                                                                            <td class="dealSign_td_num">
                                                                                                序号
                                                                                            </td>
                                                                                            <td class="dealSign_td_user">
                                                                                                参与人
                                                                                            </td>
                                                                                            <td class="dealSign_td_comment">
                                                                                                意见
                                                                                            </td>
                                                                                            <td class="dealSign_td_condition">
                                                                                                落实情况
                                                                                            </td>
                                                                                        </tr>
                                                                                        <asp:Repeater ID="rptDept" runat="server" OnItemDataBound="rptDept_ItemDataBound">
                                                                                            <ItemTemplate>
                                                                                                <tr class="tr_itm">
                                                                                                    <td class="dealSign_td_num2">
                                                                                                        <%# Container.ItemIndex + 1 %>
                                                                                                    </td>
                                                                                                    <td class="dealSign_td_user2">
                                                                                                        <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName")%>'></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="dealSign_td_comment2">
                                                                                                        <asp:Label ID="lblContent" runat="server" Text='<%# Eval("Content")%>' CssClass="lblcontent"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="dealSign_td_condition2">
                                                                                                        <table style="border: 0px; width: 100%;">
                                                                                                            <tr>
                                                                                                                <td style="width: 82%; background-color: #F2F2F4;">
                                                                                                                    <asp:TextBox ID="txtCondition" runat="server" Text='<%#Eval("DealCondition") %>'
                                                                                                                        Width="200px" BackColor="#F2F2F4" BorderStyle="None" CssClass="lblcontent"></asp:TextBox>
                                                                                                                </td>
                                                                                                                <td style="width: 18%; background-color: #F2F2F4;">
                                                                                                                    <cc1:FSLabel ID="lblDeal" runat="server" Text='填写' Style="color: Blue; cursor: pointer"></cc1:FSLabel>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <span style="display: none">
                                                                                                            <asp:Label ID="lblUserID" runat="server" Text='<%# Eval("UserID")%>'></asp:Label>
                                                                                                            <asp:Label ID="lblSubmitTime" runat="server" Text='<%#Eval("FinishTime") %>'></asp:Label>
                                                                                                            <asp:Label ID="lblTBID" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
                                                                                                            <asp:Label ID="lblDeptID" runat="server" Text='<%#Eval("DeptID") %>'></asp:Label>
                                                                                                            <asp:Label ID="lblDeptName" runat="server" Text='<%# Eval("DeptName")%>'></asp:Label>
                                                                                                        </span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </ItemTemplate>
                                                                                        </asp:Repeater>
                                                                                    </table>
                                                                                </div>
                                                                            </asp:Panel>
                                                                        </div>
                                                                        <div id="divLeader">
                                                                            <asp:Panel ID="pnlLeaderComment" runat="server" Visible="false">
                                                                                <div class="dealSignBar">
                                                                                    领导会签意见
                                                                                </div>
                                                                                <div class="dealSignFrame">
                                                                                    <table style="border-top: none; width: 100%">
                                                                                        <tr class="dealSign_tr">
                                                                                            <td class="dealSign_td_num">
                                                                                                序号
                                                                                            </td>
                                                                                            <td class="dealSign_td_user">
                                                                                                参与人
                                                                                            </td>
                                                                                            <td class="dealSign_td_comment">
                                                                                                意见
                                                                                            </td>
                                                                                            <td class="dealSign_td_condition">
                                                                                                落实情况
                                                                                            </td>
                                                                                        </tr>
                                                                                        <asp:Repeater ID="rptLeader" runat="server" OnItemDataBound="rptLeader_ItemDataBound">
                                                                                            <ItemTemplate>
                                                                                                <tr class="tr_itm">
                                                                                                    <td class="dealSign_td_num2">
                                                                                                        <%# Container.ItemIndex + 1 %>
                                                                                                    </td>
                                                                                                    <td class="dealSign_td_user2">
                                                                                                        <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName")%>'></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="dealSign_td_comment2">
                                                                                                        <asp:Label ID="lblContent" runat="server" Text='<%# Eval("Content")%>' CssClass="lblcontent"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="dealSign_td_condition2">
                                                                                                        <table style="border: 0px; width: 100%;">
                                                                                                            <tr>
                                                                                                                <td style="width: 85%; background-color: #F2F2F4;">
                                                                                                                    <asp:TextBox ID="txtCondition" runat="server" Text='<%#Eval("DealCondition") %>'
                                                                                                                        Width="200px" BackColor="#F2F2F4" BorderStyle="None" CssClass="lblcontent"></asp:TextBox>
                                                                                                                </td>
                                                                                                                <td style="width: 15%; background-color: #F2F2F4;">
                                                                                                                    <cc1:FSLabel ID="lblDeal" runat="server" Text='填写' Style="color: Blue; cursor: pointer"></cc1:FSLabel>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <span style="display: none">
                                                                                                            <asp:Label ID="lblUserID" runat="server" Text='<%# Eval("UserID")%>'></asp:Label>
                                                                                                            <asp:Label ID="lblSubmitTime" runat="server" Text='<%#Eval("FinishTime") %>'></asp:Label>
                                                                                                            <asp:Label ID="lblTBID" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
                                                                                                        </span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </ItemTemplate>
                                                                                        </asp:Repeater>
                                                                                    </table>
                                                                                </div>
                                                                            </asp:Panel>
                                                                        </div>
                                                                        <div id="divQG">
                                                                            <asp:Panel ID="pnlQGComment" runat="server" Visible="false">
                                                                                <div class="dealSignBar">
                                                                                    质保审查意见
                                                                                </div>
                                                                                <div class="dealSignFrame">
                                                                                    <table style="border-top: none; width: 100%">
                                                                                        <tr class="dealSign_tr">
                                                                                            <td class="dealSign_td_num">
                                                                                                序号
                                                                                            </td>
                                                                                            <td class="dealSign_td_user">
                                                                                                参与人
                                                                                            </td>
                                                                                            <td class="dealSign_td_comment">
                                                                                                意见
                                                                                            </td>
                                                                                            <td class="dealSign_td_condition">
                                                                                                落实情况
                                                                                            </td>
                                                                                        </tr>
                                                                                        <asp:Repeater ID="rptQG" runat="server" OnItemDataBound="rptQG_ItemDataBound">
                                                                                            <ItemTemplate>
                                                                                                <tr class="tr_itm">
                                                                                                    <td class="dealSign_td_num2">
                                                                                                        <%# Container.ItemIndex + 1 %>
                                                                                                    </td>
                                                                                                    <td class="dealSign_td_user2">
                                                                                                        <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName")%>'></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="dealSign_td_comment2">
                                                                                                        <asp:Label ID="lblContent" runat="server" Text='<%# Eval("Content")%>' CssClass="lblcontent"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="dealSign_td_condition2">
                                                                                                        <table style="border: 0px; width: 100%;">
                                                                                                            <tr>
                                                                                                                <td style="width: 85%; background-color: #F2F2F4;">
                                                                                                                    <asp:TextBox ID="txtCondition" runat="server" Text='<%#Eval("DealCondition") %>'
                                                                                                                        Width="200px" BackColor="#F2F2F4" BorderStyle="None" CssClass="lblcontent"></asp:TextBox>
                                                                                                                </td>
                                                                                                                <td style="width: 15%; background-color: #F2F2F4;">
                                                                                                                    <asp:Label ID="lblDeal" runat="server" Text='填写' Style="color: Blue; cursor: pointer"></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <span style="display: none">
                                                                                                            <asp:Label ID="lblTBID" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
                                                                                                        </span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </ItemTemplate>
                                                                                        </asp:Repeater>
                                                                                    </table>
                                                                                </div>
                                                                            </asp:Panel>
                                                                        </div>
                                                                        <div id="divApprove">
                                                                            <asp:Panel ID="pnlApproveComment" runat="server" Visible="false" Width="611px">
                                                                                <div class="dealSignBar">
                                                                                    批准意见
                                                                                </div>
                                                                                <div class="dealSignFrame">
                                                                                    <table style="border-top: none; width: 100%">
                                                                                        <tr class="dealSign_tr">
                                                                                            <td class="dealSign_td_num">
                                                                                                序号
                                                                                            </td>
                                                                                            <td class="dealSign_td_user">
                                                                                                参与人
                                                                                            </td>
                                                                                            <td class="dealSign_td_comment">
                                                                                                意见
                                                                                            </td>
                                                                                            <td class="dealSign_td_condition">
                                                                                                落实情况
                                                                                            </td>
                                                                                        </tr>
                                                                                        <asp:Repeater ID="rptPiZhun" runat="server" OnItemDataBound="rptPiZhun_ItemDataBound">
                                                                                            <ItemTemplate>
                                                                                                <tr class="tr_itm">
                                                                                                    <td class="dealSign_td_num2">
                                                                                                        <%# Container.ItemIndex + 1 %>
                                                                                                    </td>
                                                                                                    <td class="dealSign_td_user2">
                                                                                                        <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName")%>'></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="dealSign_td_comment2">
                                                                                                        <asp:Label ID="lblContent" runat="server" Text='<%# Eval("Content")%>' CssClass="lblcontent"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="dealSign_td_condition2">
                                                                                                        <table style="border: 0px; width: 100%;">
                                                                                                            <tr>
                                                                                                                <td style="width: 85%; background-color: #F2F2F4;">
                                                                                                                    <asp:TextBox ID="txtCondition" runat="server" Text='<%#Eval("DealCondition") %>'
                                                                                                                        Width="200px" BackColor="#F2F2F4" BorderStyle="None" CssClass="lblcontent"></asp:TextBox>
                                                                                                                </td>
                                                                                                                <td style="width: 15%; background-color: #F2F2F4;">
                                                                                                                    <cc1:FSLabel ID="lblDeal" runat="server" Text='填写' Style="color: Blue; cursor: pointer"></cc1:FSLabel>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <span style="display: none">
                                                                                                            <asp:Label ID="lblUserID" runat="server" Text='<%# Eval("UserID")%>'></asp:Label>
                                                                                                            <asp:Label ID="lblSubmitTime" runat="server" Text='<%#Eval("FinishTime") %>'></asp:Label>
                                                                                                            <asp:Label ID="lblTBID" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
                                                                                                        </span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </ItemTemplate>
                                                                                        </asp:Repeater>
                                                                                    </table>
                                                                                </div>
                                                                            </asp:Panel>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset></asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td><!--111-->
                                    <!--111-->
                                </tr>
                                <tr>
                                    <td>
                                        <div id="divButtomBtns" class="divSubmit" runat="server">
                                            <cc1:FSButton ID="btnSubmitCheck" runat="server" CssClass="btn" Text="提交校核" UseSubmitBehavior="false"
                                                OnClientClick="DisableCtrls();" />
                                            <cc1:FSButton ID="btnSend" runat="server" Text="分发完成" CssClass="btn" UseSubmitBehavior="false"
                                                OnClientClick="DisableCtrls();" />
                                            <cc1:FSButton ID="btnBack" runat="server" Text="退回" OnClientClick="DisableCtrls()"
                                                UseSubmitBehavior="false" CssClass="btn" />
                                            <cc1:FSButton ID="btnArchive" runat="server" CssClass="btn" 
                                                Text="归档" UseSubmitBehavior="false"/>
                                            <cc1:FSButton ID="btnSave" runat="server" CssClass="btn" Text="保存" OnClientClick="DisableCtrls();"
                                                UseSubmitBehavior="false" />
                                            <uc:UCPrint ID="ucPrint" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td id="tdSignRegion" runat="server" valign="top" style="width: 250px">
                        <asp:Panel ID="pnlPromptInfo" runat="server">
                            <div style="height: 20px; font-size: 12px; width: 185px; padding-left: 0px; padding-right: 0px;
                                padding-top: 10px; padding-bottom: 10px; background: #deeef4; border-right: #64b7d7 1px solid;
                                border-top: #64b7d7 2px solid; border-bottom: #64b7d7 0px solid; border-left: #64b7d7 1px solid;
                                text-align: center; font-weight: bold;">
                                <%=StepName%></div>
                            <div id="Div1" style="font-size: 12px; width: 185px; border: 1px solid #64b7d7; text-align: center;
                                vertical-align: top; font-size: 12px;">
                                &nbsp;<br />
                                意 见：
                                <cc1:FSTextBox ID="txtInfo" Height="150px" Width="165px" TextMode="MultiLine" CssClass="textarea_yello"
                                    runat="server" MaxLength="100"></cc1:FSTextBox>
                                <br />
                                <div class="divSubmit" id="divSubmit">
                                    <cc1:FSButton ID="btnSubmit" runat="server" CssClass="btn" Text="提交" UseSubmitBehavior="false"
                                        OnClientClick="DisableCtrls();" />
                                    <cc1:FSButton ID="btnDeptSign" runat="server" CssClass="btn" Text="部门会签" UseSubmitBehavior="false"
                                        OnClientClick="DisableCtrls();" />
                                    <cc1:FSButton ID="btnLeaderSign" runat="server" CssClass="btn" Text="领导会签" UseSubmitBehavior="false"
                                        OnClientClick="DisableCtrls();" />
                                    <cc1:FSButton ID="btnAuditApprove" runat="server" CssClass="btn" Text="提交批准" UseSubmitBehavior="false"
                                        OnClientClick="if(ApproveSubmitConfirm()==false){return false;};" />
                                    <asp:Button ID="btnBack2" runat="server" Text="退回" CssClass="btn" UseSubmitBehavior="false"
                                        OnClientClick="DisableCtrls();" />
                                    <cc1:FSButton ID="btnSave2" runat="server" CssClass="btn" Text="保存" OnClientClick="DisableCtrls();"
                                        UseSubmitBehavior="false" />
                                    <asp:Button ID="btnAuditCirculate" UseSubmitBehavior="false" runat="server" CssClass="btn"
                                        Text="传阅" OnClientClick="DisableCtrls();" />
                                    <uc:UCPrint ID="UCPrint3" runat="server" />
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlComments" runat="server">
                            <div style="height: 20px; font-size: 12px; width: 100%; padding-left: 0px; padding-right: 0px;
                                padding-top: 10px; padding-bottom: 10px; background: #deeef4; border-right: #64b7d7 1px solid;
                                border-top: #64b7d7 2px solid; border-bottom: #64b7d7 0px solid; border-left: #64b7d7 1px solid;
                                text-align: center; font-weight: bold;">
                                添加意见</div>
                            <div style="font-size: 12px; width: 100%; border: 1px solid #64b7d7; text-align: center;
                                vertical-align: top; font-size: 12px;">
                                <asp:Panel ID="pnlComment" runat="server" Visible="false">
                                    <div style="padding-left: 0px; text-align: center">
                                        <cc1:FSTextBox ID="txtInfo2" Height="150px" Width="165px" TextMode="MultiLine" CssClass="textarea_yello"
                                            runat="server" MaxLength="100"></cc1:FSTextBox>
                                    </div>
                                </asp:Panel>
                                <div style="font-size: 13px; width: 250px">
                                    <table style="border-top: none; width: 100%">
                                        <tr id="trYiJianHead" runat="server" visible="false" style="line-height: 1.5em; font-size: 12px;
                                            font-weight: bold; background: #deeef4;">
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
                                                <tr style="line-height: 1.5em; font-size: 12px">
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
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="font_red">
                                        提示：填写意见表示否决</div>
                                    <div style="text-align: center; margin: 2px; padding: 2px;">
                                        <cc1:FSButton ID="btnAdd" runat="server" CssClass="btn" Text="添加意见" OnClientClick="return piZhunTiShi();" />
                                        <cc1:FSButton ID="btnCancel" runat="server" Text="取消" CssClass="btn" Visible="false" />
                                        <cc1:FSButton ID="btnConfirm" runat="server" Text="确定" CssClass="btn" Visible="false" /></div>
                                </div>
                            </div>
                            <div class="divSubmit">
                                <cc1:FSButton ID="btnSubmits" runat="server" CssClass="btn" OnClientClick="if(IsIgnoreChildProcess()==false){return false;};"
                                    Text="提交" UseSubmitBehavior="false" />
                                <cc1:FSButton ID="btnDeptSign2" runat="server" CssClass="btn" Text="部门会签" OnClientClick="DisableCtrls();"
                                    UseSubmitBehavior="false" />
                                <cc1:FSButton ID="btnLeaderSign2" runat="server" CssClass="btn" Text="领导会签" OnClientClick="DisableCtrls();"
                                    UseSubmitBehavior="false" />
                                <cc1:FSButton ID="btnQGApprove" runat="server" CssClass="btn" OnClientClick="if(ApproveSubmitConfirm()==false){return false;};"
                                    UseSubmitBehavior="false" Text="提交批准" />
                                <cc1:FSButton ID="btnSubmitFenFa" runat="server" CssClass="btn" Text="提交分发" UseSubmitBehavior="false"
                                    Visible="False" OnClientClick="if(confirm('是否提交分发？')){DisableCtrls();}else{return false;}" />
                                <cc1:FSButton ID="btnSave3" runat="server" CssClass="btn" Text="保存" OnClientClick="DisableCtrls();"
                                    UseSubmitBehavior="false" />
                                <cc1:FSButton ID="btnQGBack" runat="server" CssClass="btn" Text="退回" OnClientClick="DisableCtrls();"
                                    UseSubmitBehavior="false" />
                                <cc1:FSButton ID="btnQGCirculate" runat="server" CssClass="btn" Text="传阅" OnClientClick="DisableCtrls();"
                                    UseSubmitBehavior="false" />
                                <uc:UCPrint ID="UCPrint2" runat="server" />
                            </div>
                        </asp:Panel>
                        <div id="divAssign" runat="server" visible="false" style="border: 1px solid #64b7d7;">
                            <div style="border-top: #64b7d7 1px solid; border-bottom: #64b7d7 1px solid; text-align: center;
                                padding-top: 10px; padding-bottom: 10px; background: #deeef4; width: 100%; font-size: 12px;
                                font-weight: bold">
                                会签交办</div>
                            <table style="width: 250px">
                                <tr id="trLeaderDeal" runat="server">
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblAssign" runat="server" Text="交办人："></asp:Label>
                                    </td>
                                    <td style="text-align: left">
                                        <cc1:FSTextBox ID="txtAssignMember" runat="server" CssClass="txtbox_blue" Width="80px"></cc1:FSTextBox>
                                        <uc:OASelectUC ID="ucSelectAssign" runat="server" />
                                    </td>
                                    <td>
                                        <cc1:FSButton ID="btnAssign" runat="server" Text="交办" UseSubmitBehavior="false" OnClientClick="if(CheckIsSubmit()==false){return false;}"
                                            CssClass="btn" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        意见内容：
                                    </td>
                                    <td colspan="2" style="text-align: center">
                                        <cc1:FSTextBox ID="txtAssistInfo" runat="server" CssClass="textarea_yello" Height="150px"
                                            MaxLength="200" TextMode="MultiLine" Width="165px"></cc1:FSTextBox>
                                    </td>
                                </tr>
                            </table>
                            <div id="divAssignDeal" style="margin: 2px; padding: 2px; text-align: center" runat="server">
                                <cc1:FSButton ID="btnFinish" runat="server" CssClass="btn" Text="完成" UseSubmitBehavior="false"
                                    OnClientClick="DisableCtrls();" />
                                <cc1:FSButton ID="btnAssignSave" runat="server" CssClass="btn" Text="保存" UseSubmitBehavior="false"
                                    OnClientClick="DisableCtrls();" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <cc1:FSHiddenField ID="hfEditIndex" runat="server" />
        <div style="display: none">
            隐藏区域,记录流程数据<br />
            ProcessID<asp:TextBox ID="wfProcessID" runat="server"></asp:TextBox>
            <br />
            WorkItemID<asp:TextBox ID="wfWorkItemID" runat="server"></asp:TextBox>
            <br />
            ProgramID<asp:TextBox ID="wfProgramID" runat="server"></asp:TextBox>
            <br />
            ReceiveUserID<asp:TextBox ID="wfReceiveUserID" runat="server"></asp:TextBox>
            ReceiveUserName<asp:TextBox ID="wfReceiveUserName" runat="server"></asp:TextBox>
            <br />
            程序发起人<asp:TextBox ID="wfDrafter" runat="server"></asp:TextBox>
            <br />
            程序发起人ID<asp:TextBox ID="wfDrafterID" runat="server"></asp:TextBox>
            <br />
            程序发起时间<asp:TextBox ID="wfDraftDate" runat="server"></asp:TextBox>
            <br />
            发起人所在部门（主办部门）ID<asp:TextBox ID="wfChiefDept" runat="server" Width="71px"></asp:TextBox>
            <br />
            程序类型<asp:TextBox ID="wfSort" runat="server"></asp:TextBox>
            <br />
            文件标题<asp:TextBox ID="wfDocTitle" runat="server"></asp:TextBox>
            <br />
            年份<asp:TextBox ID="wfYear" runat="server"></asp:TextBox>
            <br />
            流水号<asp:TextBox ID="wfSerialID" runat="server"></asp:TextBox>
            <br />
            是否同意<cc1:FSHiddenField ID="hfIsAgree" runat="server" />
            <br />
            质保审查人IDs<asp:TextBox ID="wfQualityIDs" runat="server"></asp:TextBox>
            <br />
            发送传阅时间<asp:TextBox ID="wfCirculateDate" runat="server" Width="70px"></asp:TextBox>
            <br />
            处室领导IDs<asp:TextBox ID="wfDeptLeaderIDs" runat="server"></asp:TextBox>
            <br />
            公司领导IDs<asp:TextBox ID="wfCoLeadersIDs" runat="server"></asp:TextBox>
            <br />
            分发人<asp:TextBox ID="wfSender" runat="server"></asp:TextBox>
            <br />
            分发人IDs<asp:TextBox ID="wfSenderIDs" runat="server"></asp:TextBox>
            <br />
            分发时间<asp:TextBox ID="wfSendDate" runat="server"></asp:TextBox>
            <br />
            分发范围<asp:TextBox ID="wfDispense" runat="server"></asp:TextBox>
            <br />
            编写人ID<asp:TextBox ID="wfWriteID" runat="server"></asp:TextBox>
            <br />
            校核人ID<asp:TextBox ID="wfCheckID" runat="server"></asp:TextBox>
            <br />
            ParentTBID<asp:TextBox ID="wfParentTBID" runat="server"></asp:TextBox>
            AssistContent<asp:TextBox ID="wfAssistContent" runat="server"></asp:TextBox>
            AssignUserName<asp:TextBox ID="wfAssignUserName" runat="server"></asp:TextBox>
            <br />
            AssignUserID<asp:TextBox ID="wfAssignUserID" runat="server"></asp:TextBox>
            ChildProcessID<asp:TextBox ID="wfChildProcessID" runat="server"></asp:TextBox>
            <br />
            TimesFlag<asp:TextBox ID="wfTimesFlag" runat="server"></asp:TextBox>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="ucPrint" />
        <asp:PostBackTrigger ControlID="UCPrint2" />
        <asp:PostBackTrigger ControlID="UCPrint3" />
    </Triggers>
</asp:UpdatePanel>
