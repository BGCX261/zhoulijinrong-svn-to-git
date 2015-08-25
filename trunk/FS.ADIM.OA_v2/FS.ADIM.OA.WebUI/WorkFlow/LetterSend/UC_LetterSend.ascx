<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_LetterSend.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.LetterSend.UC_LetterSend" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../../PageWF/UC_Print.ascx" TagName="UCPrint" TagPrefix="uc" %>
<%@ Register Src="../../PageWF/UC_FileControl.ascx" TagName="FileControlUC" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_Company.ascx" TagName="UCCompany" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_CompanyMore.ascx" TagName="UCCompanyMore" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_OASelect.ascx" TagName="OASelectUC" TagPrefix="uc" %>
<%@ Register Src="../../PageOU/UC_Role.ascx" TagName="RoleUC" TagPrefix="uc" %>

<script type="text/javascript">
    $("containTitle").innerHTML = "<%=TemplateName%> - " + "<%=StepName%>";
</script>

<div class="divCenter">
    <table class="table_form_border" style="width: 750px;" border="1">
        <tr>
            <td class="td_grey" style="height: 30px;">
                <span class="label_title_darkblue">海南核电有限公司函件发文单</span>
            </td>
        </tr>
        <tr>
            <td>
                <table class="table_form_noborder" style="text-align: left">
                    <tr>
                        <td style="width: 90px">
                            <asp:CheckBox ID="chkJinJi" runat="server" Text="紧急" /><asp:CheckBox ID="chkHuiZhi"
                                runat="server" Text="回复" />
                        </td>
                        <td style="width: 150px">
                            <span class="label">页数：</span><asp:Label ID="lblPages" runat="server" class="label_title_red"
                                Text="*"></asp:Label>
                            <cc1:FSTextBox ID="txtPages" CssClass="txtbox_yellow" Style="width: 50px" runat="server"
                                MaxLength="5"></cc1:FSTextBox>
                        </td>
                        <td style="width: 100px">
                            <span class="label_title_bold">我方发文号：</span><asp:Label ID="lblOurRef" runat="server"
                                class="label_title_red" Text="*"></asp:Label>
                        </td>
                        <td>
                            <cc1:FSTextBox ID="txtOurRef" CssClass="txtbox_yellow" Style="width: 172px" runat="server"></cc1:FSTextBox>
                            <div>
                                <asp:Button ID="btnSetNo" runat="server" Text="生成" OnClick="btnSetNo_Click" />
                                <asp:Button ID="btnCheck" runat="server" Text="检查" OnClick="btnCheck_Click" /></div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 5px;">
                            <div style="border-bottom: 1px #ccc dashed">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="label_title_bold">函件类型：</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpHanJian" runat="server" CssClass="dropdownlist_yellow" Style="width: 262px">
                                <asp:ListItem>一般信函</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <span class="label_title_bold">对方发文号：</span>
                        </td>
                        <td>
                            <cc1:FSTextBox ID="txtYourRef" CssClass="txtbox_yellow" Style="width: 172px" runat="server"></cc1:FSTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 5px;">
                            <div style="border-bottom: 1px #ccc dashed">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="label_title_bold">设备代码：</span>
                        </td>
                        <td>
                            <cc1:FSTextBox ID="txtEquipmentCode" CssClass="txtbox_yellow" Style="width: 172px"
                                runat="server"></cc1:FSTextBox>
                        </td>
                        <td>
                            <span class="label_title_bold">对应合同号：</span>
                        </td>
                        <td>
                            <cc1:FSTextBox ID="txtContractNo" CssClass="txtbox_yellow" Style="width: 172px" runat="server"></cc1:FSTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 5px;">
                            <div style="border-bottom: 1px #ccc dashed">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table class="table_form_noborder">
                                <tr>
                                    <td>
                                        <fieldset style="height: 80px;">
                                            <legend>签发</legend>
                                            <table>
                                                <tr>
                                                    <td style="height: 26px; width: 57px;">
                                                        <span class="label_title_bold">签发人：<span class="label_title_red">*</span></span>
                                                    </td>
                                                    <td style="width: 144px">
                                                        <cc1:FSTextBox ID="txtQianFaRen" CssClass="txtbox_bgreen" Style="width: 110px" runat="server"></cc1:FSTextBox>
                                                        <uc:RoleUC ID="UCQianFa" runat="server" />
                                                        <asp:Label ID="lbQianFaRen" runat="server" Text="" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="label_title_bold">日期：</span>
                                                    </td>
                                                    <td>
                                                        <cc1:FSTextBox ID="txtSignDate" CssClass="txtbox_bgreen" Style="width: 110px" runat="server"></cc1:FSTextBox>
                                                        <asp:Label ID="lbSignDate" runat="server" Text="" Visible="false"></asp:Label>
                                                   </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 80px;">
                                            <legend>会签</legend>
                                            <table>
                                                <tr>
                                                    <td style="height: 26px; width: 57px;">
                                                        <span class="label_title_bold">会签人：<span class="label_title_red">*</span></span>
                                                    </td>
                                                    <td style="width: 144px">
                                                        <cc1:FSTextBox ID="txtHuiQianRen" CssClass="textarea_bgreen" Style="height: 42px"
                                                            Width="110px" runat="server" TextMode="MultiLine"></cc1:FSTextBox>
                                                        <uc:RoleUC ID="UCHuiQian" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr style="display: none;">
                                                    <td>
                                                        <span class="label_title_bold">日期：</span>
                                                    </td>
                                                    <td>
                                                        <cc1:FSTextBox ID="txtHuiQianRenDates" CssClass="textarea_bgreen" Style="height: 30px"
                                                            Width="110px" runat="server" TextMode="MultiLine"></cc1:FSTextBox>
                                                        <asp:Label ID="lbHuiQianRenDates" runat="server" Text="" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 80px;">
                                            <legend>核稿</legend>
                                            <table>
                                                <tr>
                                                    <td style="height: 26px; width: 57px;">
                                                        <span class="label_title_bold">核稿人：</span>
                                                    </td>
                                                    <td style="width: 144px">
                                                        <cc1:FSTextBox ID="txtHeGaoRen" CssClass="txtbox_bgreen" Style="width: 110px" runat="server"></cc1:FSTextBox>
                                                        <uc:OASelectUC ID="UCHeGao" runat="server" />
                                                        <asp:Label ID="lbHeGaoRen" runat="server" Text="" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="label_title_bold">日期：</span>
                                                    </td>
                                                    <td>
                                                        <cc1:FSTextBox ID="txtHeGaoRenDate" CssClass="txtbox_bgreen" Style="width: 110px"
                                                            runat="server"></cc1:FSTextBox>
                                                        <asp:Label ID="lbHeGaoRenDate" runat="server" Text="" Visible="false"></asp:Label>
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
                        <td colspan="4" style="height: 5px;">
                            <div style="border-bottom: 1px #ccc dashed">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="label_title_bold">拟稿/日期：</span>
                        </td>
                        <td>
                            <cc1:FSTextBox ID="txtNiGaoRenDate" runat="server" CssClass="txtbox_bgreen" Style="width: 260px"></cc1:FSTextBox>
                            <asp:Label ID="lbNiGaoRen" runat="server" Text="" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <span class="label_title_bold">发文部门：</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpSendDept" runat="server" CssClass="dropdownlist_green" Style="width: 165px"
                                OnSelectedIndexChanged="drpSendDept_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 5px;">
                            <div style="border-bottom: 1px #ccc dashed">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="label_title_bold">标题：</span><span class="label_title_red">*</span>
                        </td>
                        <td colspan="3">
                            <cc1:FSTextBox ID="txtSubject" runat="server" CssClass="txtbox_yellow" Style="width: 583px"
                                MaxLength="100"></cc1:FSTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 5px;">
                            <div style="border-bottom: 1px #ccc dashed">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="label_title_bold">主送单位：</span><span class="label_title_red">*</span>
                        </td>
                        <td>
                            <cc1:FSTextBox ID="txtCompany" runat="server" CssClass="txtbox_yellow" Style="width: 260px"
                                MaxLength="500"></cc1:FSTextBox>
                            <uc:UCCompany ID="UCCompany" runat="server" />
                        </td>
                        <td>
                            <span class="label_title_bold">主送人：</span>
                        </td>
                        <td>
                            <cc1:FSTextBox ID="txtTo" runat="server" CssClass="txtbox_yellow" Style="width: 163px"
                                MaxLength="500"></cc1:FSTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 5px;">
                            <div style="border-bottom: 1px #ccc dashed">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="label_title_bold">抄送单位：</span>
                        </td>
                        <td colspan="3">
                            <cc1:FSTextBox ID="txtccCompany" runat="server" CssClass="txtbox_yellow" Style="width: 583px"
                                MaxLength="500" ></cc1:FSTextBox>
                            <uc:UCCompany ID="UCCompanycs" runat="server" />
                            <uc:UCCompanyMore ID="UCCompanycc" runat="server" Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 5px;">
                            <div style="border-bottom: 1px #ccc dashed">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="label_title_bold">抄送部门：</span>
                        </td>
                        <td colspan="3">
                            <cc1:FSTextBox ID="txtccDept" runat="server" CssClass="txtbox_yellow" Style="width: 583px"
                                MaxLength="500"></cc1:FSTextBox>
                            <uc:OASelectUC ID="UCDeptcc" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 5px;">
                            <div style="border-bottom: 1px #ccc dashed">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="label_title_bold">抄送领导：</span>
                        </td>
                        <td colspan="3">
                            <cc1:FSTextBox ID="txtccLeader" runat="server" CssClass="txtbox_yellow" Style="width: 583px"
                                MaxLength="500"></cc1:FSTextBox>
                            <uc:RoleUC ID="UCccLingDao" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 5px;">
                            <div style="border-bottom: 1px #ccc dashed">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="label_title_bold">正文：<span class="label_title_red">*</span></span>
                        </td>
                        <td colspan="3">
                            <cc1:FSTextBox ID="txtContent" runat="server" CssClass="txtbox_yellow" Style="width: 583px"
                                MaxLength="100" Height="240px" TextMode="MultiLine"></cc1:FSTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 5px;">
                            <div style="border-bottom: 1px #ccc dashed">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="label_title_bold">附件：</span>
                        </td>
                        <td colspan="3" style="width: 600px;">
                            <uc:FileControlUC ID="ucAttachment" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 5px;">
                            <div style="border-bottom: 1px #ccc dashed">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="label_title_bold">意见列表：</span>
                        </td>
                        <td colspan="3">
                            <table cellpadding="3" cellspacing="1" style="border: 1px solid #000000; margin-bottom: 5px;
                                width: 587px; table-layout: fixed">
                                <tr style="background-color: #ADD9E6; height: 20px;">
                                    <td style="width: 30px; text-align: center">
                                        序号
                                    </td>
                                    <td style="width: 120px; text-align: center">
                                        步骤名
                                    </td>
                                    <td style="width: 50px; text-align: center">
                                        参与人
                                    </td>
                                    <td style="width: 120px; text-align: center">
                                        完成时间
                                    </td>
                                    <td style="text-align: center">
                                        意见
                                    </td>
                                </tr>
                                <asp:Repeater ID="Repeater1" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="border-bottom: 1px #ccc dashed;">
                                                <%# Container.ItemIndex + 1 %>
                                            </td>
                                            <td style="border-bottom: 1px #ccc dashed;">
                                                <%# Eval("ViewName")%>
                                            </td>
                                            <td style="border-bottom: 1px #ccc dashed;">
                                                <%# Eval("UserName")%>
                                            </td>
                                            <td style="border-bottom: 1px #ccc dashed;">
                                                <%# Eval("FinishTime")%>
                                            </td>
                                            <td style="border-bottom: 1px #ccc dashed; text-align: left;" class="lblcontent">
                                                <%# Eval("Content")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 5px;">
                            <div style="border-bottom: 1px #ccc dashed">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblYiJian" runat="server" CssClass="label_title_bold" Text="意见"></asp:Label><span
                                class="label">：</span>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtComment" runat="server" CssClass="txtbox_yellow" Height="65px"
                                TextMode="MultiLine" Style="width: 583px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<div class="divSubmit" id="divSubmit">
    <asp:Button ID="btnGD" runat="server" Text="归档" CssClass="btn" UseSubmitBehavior="false"
         Visible="true" OnClick="btnGD_Click" />
    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn" OnClick="SubmitEvents"
        UseSubmitBehavior="false" OnClientClick="DisableButtons()" Visible="true" />
    <asp:Button ID="btnSubmitHeGao" runat="server" Text="提交核稿" CssClass="btn" OnClick="SubmitEvents"
        UseSubmitBehavior="false" OnClientClick="DisableButtons()" />
    <asp:Button ID="btnSubmitHuiQian" runat="server" Text="提交会签" CssClass="btn" OnClick="SubmitEvents"
        UseSubmitBehavior="false" OnClientClick="DisableButtons()" />
    <asp:Button ID="btnSumitQianFa" runat="server" Text="提交签发" CssClass="btn" OnClick="SubmitEvents"
        UseSubmitBehavior="false" OnClientClick="DisableButtons()" />
    <asp:Button ID="btnCancel" runat="server" Text="撤销" CssClass="btn" OnClick="SubmitEvents" UseSubmitBehavior="false"
        Visible="true" />
    <asp:Button ID="btnQianFa" runat="server" Text="签发" CssClass="btn" OnClick="SubmitEvents"
        UseSubmitBehavior="false" OnClientClick="DisableButtons()" />
    <asp:Button ID="btnWanCheng" runat="server" Text="完成" CssClass="btn" OnClick="SubmitEvents"
        UseSubmitBehavior="false" OnClientClick="DisableButtons()" Visible="true" />
    <asp:Button ID="btnBack" runat="server" Text="退回" CssClass="btn" OnClick="SubmitEvents"
        UseSubmitBehavior="false" OnClientClick="DisableButtons()" Visible="true" />
    <asp:Button ID="btnSencondFenfa" runat="server" Text="二次分发" CssClass="btn" OnClick="SubmitEvents"
        UseSubmitBehavior="false" OnClientClick="DisableButtons()" Visible="true" />
    <asp:Button ID="btnAddFenfa" runat="server" Text="追加分发" CssClass="btn" OnClick="SubmitEvents"
        UseSubmitBehavior="false" OnClientClick="DisableButtons()" Visible="true" />
    <uc:UCPrint ID="ucPrint" runat="server" />
</div>
<div class="divHide" style="display: none">
    <asp:Panel ID="wfPanel" runat="server" Height="50px" Width="125px">
        ProcessID<asp:TextBox ID="wfProcessID" runat="server"></asp:TextBox>
        WorkItemID<asp:TextBox ID="wfWorkItemID" runat="server"></asp:TextBox>
        抄送领导IDs<asp:TextBox ID="txtccLeaderIDs" runat="server"></asp:TextBox>
        抄送部门领导IDs<asp:TextBox ID="txtDeptLeaderIDs" runat="server"></asp:TextBox>
        抄送部门IDs<asp:TextBox ID="txtccDeptIDs" runat="server"></asp:TextBox>
        传阅人IDs<asp:TextBox ID="wfChuanYueRenIDs" runat="server"></asp:TextBox>
        对方单位编码<br />
        <asp:TextBox ID="txtCompanyID" runat="server"></asp:TextBox>
        发起人ID<asp:TextBox ID="wfFaQiRenID" runat="server"></asp:TextBox>
        发起人<asp:TextBox ID="wfFaQiRen" runat="server"></asp:TextBox>
        发起时间<asp:TextBox ID="txt_UserDate" runat="server"></asp:TextBox>
        核稿人ID<asp:TextBox ID="wfHeGaoRenID" runat="server"></asp:TextBox>
        会签人IDs<asp:TextBox ID="wfHuiQianRenIDs" runat="server"></asp:TextBox>
        签发人ID<asp:TextBox ID="wfQianFaRenID" runat="server"></asp:TextBox>
        职务:
        <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
        步骤:
        <asp:TextBox ID="txtStep" runat="server"></asp:TextBox>
        <asp:Button ID="btnStep" runat="server" Text="设置步骤" OnClick="btnStep_Click" />
        历史<asp:CheckBox ID="chkHistory" runat="server" AutoPostBack="True" OnCheckedChanged="chkHistory_CheckedChanged" />
    </asp:Panel>
</div>
