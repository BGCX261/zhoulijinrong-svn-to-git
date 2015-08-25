<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_CompletedHandle.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.CompleteFiles.UC_CompletedHandle" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../Circulate/UC_CirculateList.ascx" TagName="UCCirculateList" TagPrefix="uc" %>
<%@ Import Namespace="FS.ADIM.OA.BLL.Common" %>
<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="false"
    EnableScriptLocalization="false">
</asp:ScriptManager>
<div class="divProgress">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <img alt="" src="Img/loading.gif" style="float: right" />
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table>
            <tr>
                <td style="width: 69px;">
                    <span class="label_title_bold ">流程类型：</span>
                </td>
                <td style="width: 115px;">
                    <cc1:FSDropDownList ID="ddlProcessTemplate" runat="server" AutoPostBack="true" DataTextField="Name"
                        DataValueField="Name" OnSelectedIndexChanged="ddlProcessTemplate_SelectedIndexChanged"
                        CssClass="dropdownlist_yellow" Width="110px">
                    </cc1:FSDropDownList>
                </td>
                <td style="width: 48px;">
                    <span class="label_title_bold">步骤：</span>
                </td>
                <td style="width: 140px;">
                    <cc1:FSDropDownList ID="ddlStepName" runat="server" DataTextField="Name" DataValueField="Name"
                        CssClass="dropdownlist_yellow" Width="130px">
                    </cc1:FSDropDownList>
                </td>
                <td style="width: 69px;">
                    <span class="label_title_bold">发起日期：</span>
                </td>
                <td style="width: 250px;" colspan="3">
                    <cc1:FSCalendar ID="txtStartDate" runat="server" Width="100px" CssClass="txtbox_yellow"
                        MaxLength="10" />
                    ~
                    <cc1:FSCalendar ID="txtEndDate" runat="server" Width="100px" CssClass="txtbox_yellow"
                        MaxLength="10" />
                </td>
            </tr>
            <tr>
                <td>
                    <span class="label_title_bold">文件标题：</span>
                </td>
                <td colspan="3">
                    <cc1:FSTextBox ID="txtDocumentTitle" runat="server" Width="300" CssClass="txtbox_yellow"
                        MaxLength="200"></cc1:FSTextBox>
                </td>
                            
            </tr>   
            <tr>
            <td>
                <span class="label_title_bold">文号：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtDocumentNo" runat="server" Width="106px" CssClass="txtbox_yellow"
                        MaxLength="50">
                    </cc1:FSTextBox>
                </td>
                <td>
                    <span class="label_title_bold">发起人：</span></td>
                <td>
                    <cc1:FSTextBox ID="txtSponsor" runat="server" Width="130px" CssClass="txtbox_yellow"
                        MaxLength="20">
                    </cc1:FSTextBox></td>    
            </tr>         
            <tr id="trGF" runat="server" visible="false">
                <td>
                    <span class="label_title_bold">主送单位：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtGFZhuSongDanWei" runat="server" Width="106px" CssClass="txtbox_yellow"
                        MaxLength="50">

                    &nbsp;&nbsp;

                    </cc1:FSTextBox>
                </td>
                <td>
                    <span class="label_title_bold">编制部门：</span>
                </td>
                <td colspan="5">
                    <asp:DropDownList ID="drpGFDept" runat="server" CssClass="dropdownlist_yellow" Width="130px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trGS" runat="server" visible="false">
                <td>
                    <span class="label_title_bold">来文单位：</span>
                </td>
                <td colspan="7">
                    <cc1:FSTextBox ID="txtGSReceiveUnit" runat="server" Width="106px" CssClass="txtbox_yellow"
                        MaxLength="50">

                    &nbsp;&nbsp;

                    </cc1:FSTextBox>
                </td>
            </tr>
            <tr id="trHS" runat="server" visible="false">
                <td>
                    <span class="label_title_bold">来文单位：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtHSReceiveUnit" runat="server" Width="106px" CssClass="txtbox_yellow"
                        MaxLength="50">

                    &nbsp;&nbsp;

                    </cc1:FSTextBox>
                </td>
                <td>
                    <span class="label_title_bold">通讯渠道号：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtHSFileEncoding" runat="server" Width="126px" CssClass="txtbox_yellow"
                        MaxLength="50">
                    &nbsp;&nbsp;
                    </cc1:FSTextBox>
                </td>
                <td>
                    <span class="label_title_bold">函件类型：</span>
                </td>
                <td style="color: #0099CC" colspan="3">
                    <asp:DropDownList ID="ddlHSLetterType" runat="server" CssClass="dropdownlist_yellow">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trHF" runat="server" visible="false">
                <td>
                    <span class="label_title_bold">主送单位：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtHFCompany" runat="server" Width="106px" CssClass="txtbox_yellow"
                        MaxLength="50">
                    &nbsp;&nbsp;
                    </cc1:FSTextBox>
                </td>
                <td>
                    <span class="label_title_bold">发文部门：</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddlHFDept" runat="server" CssClass="dropdownlist_yellow" Width="130px">
                    </asp:DropDownList>
                </td>
                <td>
                    <span class="label_title_bold">函件类型：</span>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlHFLetterType" runat="server" CssClass="dropdownlist_yellow"
                        Width="300px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trPF" runat="server" visible="false">
                <td>
                    <span class="label_title_bold">编制部门：</span>
                </td>
                <td colspan="7">
                    <asp:DropDownList ID="ddlPFDept" runat="server" CssClass="dropdownlist_yellow">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trRR" runat="server" visible="false">
                <td>
                    <span class="label_title_bold">主送领导：</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMainSendleader" runat="server" CssClass="dropdownlist_yellow"
                        Width="110px">
                    </asp:DropDownList>
                </td>
                <td>
                    <span class="label_title_bold">承办处室：</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddlUnderTakeDept" runat="server" CssClass="dropdownlist_yellow"
                        Width="130px">
                    </asp:DropDownList>
                </td>
                <td>
                    <span class="label_title_bold">编制部门：</span>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlHostDept" runat="server" CssClass="dropdownlist_yellow">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trWR" runat="server" visible="false">
                <td>
                    <span class="label_title_bold">编制部门：</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddlWRHostDept" runat="server" CssClass="dropdownlist_yellow"
                        Width="110px">
                    </asp:DropDownList>
                </td>
                <td>
                    <span class="label_title_bold">主送部门：</span>
                </td>
                <td colspan="5">
                    <asp:DropDownList ID="ddlWRMainSendDept" runat="server" CssClass="dropdownlist_yellow"
                        Width="130px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="label_title_bold">办理类型：</span>
                </td>
                <td colspan="3">
                    <cc1:FSCheckBox ID="chkHandled" runat="server" Text="办理" CssClass="chk_yellow" Checked="true" />
                    <cc1:FSCheckBox ID="chkUnderTake" runat="server" Text="承办" CssClass="chk_yellow"
                        Checked="true" OnCheckedChanged="chkUnderTake_CheckedChanged" AutoPostBack="true" />
                    <cc1:FSCheckBox ID="chkOwnCommon" runat="server" Text="自己公办" CssClass="chk_yellow"
                        Checked="true" />
                    <cc1:FSCheckBox ID="chkOtherCommon" runat="server" Text="他人公办" CssClass="chk_yellow"
                        Checked="true" />
                    <cc1:FSCheckBox ID="chkRead" runat="server" Text="阅知" CssClass="chk_yellow" Checked="false" />
                </td>
                <td>
                    <span class="label_title_bold">承办状态：</span>
                </td>
                <td colspan="3">
                    <cc1:FSDropDownList ID="ddlUnderTakeStatus" runat="server" AutoPostBack="true" CssClass="dropdownlist_yellow"
                        Width="106px">
                        <asp:ListItem Text="未办结" Value="InCompleted"></asp:ListItem>
                        <asp:ListItem Text="已办结" Value="Completed"></asp:ListItem>
                        <asp:ListItem Text="不限" Value="Arbitrary" Selected="True"></asp:ListItem>
                    </cc1:FSDropDownList>
                    <cc1:FSCheckBox ID="chkIsCurrentWare" runat="server" Text="现行库" Checked="true" Font-Bold="true" visible=false/>
                    <cc1:FSCheckBox ID="chkIsHistoryWare" runat="server" Text="历史库" Font-Bold="true" />
                    <cc1:FSButton ID="btnSearch" runat="server" CssClass="btn" OnClick="btnSearch_Click"
                        Text="查询" Style="margin-left: 8px;" />
                        <span class="label_title_bold" style="color:Red">一个月前完结的流程请在历史库中查询</span>
                </td>
            </tr>
        </table>
        <div style="width: 100%">
            <cc1:FSGridView ID="gvTaskList" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" ShowEmptyHeader="true" ShowRadioButton="false" PageType="ExteriorPage"
                OnRowDataBound="gvTaskList_RowDataBound" OnExteriorPaging="gvTaskList_ExteriorPaging"
                OnExteriorSorting="gvTaskList_ExteriorSorting" CellSpacing="1" BackColor="White">
                <Columns>
                    <asp:BoundField HeaderText=" " HeaderStyle-Width="12px" ItemStyle-CssClass="td_Center">
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-Width="30px" ItemStyle-CssClass="td_Center">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlkView" Text="查看"> </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Category" SortExpression="Category" HeaderText="性质" HeaderStyle-Width="50px"
                        ItemStyle-CssClass="td_Center"></asp:BoundField>
                    <asp:TemplateField HeaderText="流程类型" SortExpression="DEF_NAME" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <%#SysString.GetPTDisplayName(Eval("DEF_NAME").ToString())%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="StepName" SortExpression="StepName" HeaderText="步骤" HeaderStyle-Width="100px">
                    </asp:BoundField>
                    <asp:BoundField DataField="ReceiveUserName" HeaderText="处理人" HeaderStyle-Width="50px">
                    </asp:BoundField>
                    <asp:BoundField DataField="EditDate" SortExpression="EditDate" HeaderText="处理时间"
                        HeaderStyle-Width="120px"></asp:BoundField>
                    <asp:BoundField DataField="DocumentNo" SortExpression="DocumentNo" HeaderText="文号"
                        HeaderStyle-Width="100px" Visible="false"></asp:BoundField>
                    <asp:BoundField DataField="DocumentTitle" SortExpression="DocumentTitle" HeaderText="文件标题">
                    </asp:BoundField>
                    <asp:BoundField DataField="Drafter" SortExpression="Drafter" HeaderText="发起人" HeaderStyle-Width="50px"
                        Visible="false"></asp:BoundField>
                    <asp:TemplateField HeaderText="代理人" HeaderStyle-Width="50px" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblAgentUserName" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DraftDate" SortExpression="DraftDate" HeaderText="发起日期"
                        HeaderStyle-Width="120px" Visible="false"></asp:BoundField>
                    <asp:TemplateField HeaderStyle-Width="42px" HeaderText="流程图" ItemStyle-CssClass="td_Center">
                        <ItemTemplate>
                            <img alt="流程图" onclick="window.open('/AgilePoint/ProcessViewer.aspx?PIID=<%#Eval("ProcessID") %>');"
                                src="../../AgilePoint/resource/en-us/Task.gif" style="cursor: hand" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:HyperLinkField DataNavigateUrlFields="processid,DEF_NAME" DataNavigateUrlFormatString="../Process/PG_ProcessStep.aspx?PID={0}&amp;TID={1}"
                        Target="_blank" HeaderText="步骤" Text="查看" HeaderStyle-Width="32px" ItemStyle-CssClass="td_Center">
                    </asp:HyperLinkField>
                    <asp:TemplateField HeaderText="传阅单" HeaderStyle-Width="42px" ItemStyle-CssClass="td_Center">
                        <ItemTemplate>
                            <uc:UCCirculateList ID="ucCirculateList" UCProcessID='<%#Eval("ProcessID") %>' UCTemplateName='<%#Eval("DEF_NAME") %>'
                                runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="关联" HeaderStyle-Width="30px" ItemStyle-CssClass="td_Center">
                        <ItemTemplate>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle CssClass="gv_alternatingrow" />
            </cc1:FSGridView>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
