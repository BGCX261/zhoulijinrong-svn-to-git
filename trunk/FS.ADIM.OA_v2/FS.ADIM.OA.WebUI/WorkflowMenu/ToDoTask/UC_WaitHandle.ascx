<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_WaitHandle.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.ToDoTask.UC_WaitHandle" %>
<%@ Import Namespace="FS.ADIM.OA.BLL.Common" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../Circulate/UC_CirculateList.ascx" TagName="UCCirculateList" TagPrefix="uc" %>
<asp:ScriptManager ID="SM" runat="server" EnableScriptGlobalization="false" EnableScriptLocalization="false">
</asp:ScriptManager>
<div class="divProgress">
    <asp:UpdateProgress ID="UP" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
                <td style="width: 250px;">
                    <cc1:FSCalendar ID="txtStartDate" runat="server" Width="100px" CssClass="txtbox_yellow"
                        MaxLength="10" />~
                    <cc1:FSCalendar ID="txtEndDate" runat="server" Width="100px" CssClass="txtbox_yellow"
                        MaxLength="10" />
                </td>
            </tr>
            <tr>
                <td>
                    <span class="label_title_bold">文件标题：</span>
                </td>
                <td colspan="3">
                    <cc1:FSTextBox ID="txtDocumentTitle" runat="server" Width="292" CssClass="txtbox_yellow"
                        MaxLength="200">
                    </cc1:FSTextBox>
                </td>
                <td>
                    <span class="label_title_bold">发起人：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtSponsor" runat="server" Width="100px" CssClass="txtbox_yellow"
                        MaxLength="20">
                    </cc1:FSTextBox>
                    <cc1:FSButton ID="btnSearch" runat="server" CssClass="btn" OnClick="btnSearch_Click"
                        Text="查询" Style="margin-left: 7px;" />
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
                    <asp:HyperLinkField DataNavigateUrlFields="PROC_INST_ID,WORK_ITEM_ID,PDEF_NAME,STEP_NAME,TBID"
                        ItemStyle-CssClass="td_Center" DataNavigateUrlFormatString="~/Container.aspx?ProcessID={0}&amp;WorkItemID={1}&amp;TemplateName={2}&amp;StepName={3}&amp;TBID={4}&amp;MS=1"
                        HeaderText="操作" Text="处理任务" HeaderStyle-Width="50px"></asp:HyperLinkField>
                    <asp:TemplateField HeaderText="流程类型" SortExpression="PDEF_NAME" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <%#SysString.GetPTDisplayName(Eval("PDEF_NAME").ToString())%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="STEP_NAME" SortExpression="STEP_NAME" HeaderText="当前步骤"
                        HeaderStyle-Width="100px"></asp:BoundField>
                    <asp:BoundField DataField="DocumentNo" SortExpression="DocumentNo" HeaderText="文号"
                        HeaderStyle-Width="130px"></asp:BoundField>
                    <asp:BoundField DataField="DocumentTitle" SortExpression="DocumentTitle" HeaderText="文件标题">
                    </asp:BoundField>
                    <asp:BoundField DataField="Drafter" SortExpression="Drafter" HeaderText="发起人" HeaderStyle-Width="50px"
                        Visible="false"></asp:BoundField>
                    <asp:BoundField DataField="STARTED_DATE" SortExpression="STARTED_DATE" HeaderText="发起日期"
                        HeaderStyle-Width="120px"></asp:BoundField>
                    <asp:TemplateField HeaderText="流程图" HeaderStyle-Width="42px" ItemStyle-CssClass="td_Center">
                        <ItemTemplate>
                            <img alt="流程图" onclick="window.open('/AgilePoint/ProcessViewer.aspx?PIID=<%#Eval("Proc_Inst_ID") %>');"
                                src="../../AgilePoint/resource/en-us/Task.gif" style="cursor: hand" />
                            <asp:Label ID="lblParent" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="传阅单" HeaderStyle-Width="42px" ItemStyle-CssClass="td_Center">
                        <ItemTemplate>
                            <uc:UCCirculateList ID="ucCirculateList" runat="server" UCProcessID='<%#Eval("PROC_INST_ID") %>'
                                UCTemplateName='<%#Eval("PDEF_NAME") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle CssClass="gv_alternatingrow" />
            </cc1:FSGridView>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
