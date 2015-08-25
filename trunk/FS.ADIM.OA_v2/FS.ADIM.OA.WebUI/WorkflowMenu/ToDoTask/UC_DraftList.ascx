<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_DraftList.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.ToDoTask.UC_DraftList" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Import Namespace="FS.ADIM.OA.BLL.Common" %>
<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="false"
    EnableScriptLocalization="false">
</asp:ScriptManager>
<div class="divProgress">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <img alt="" src="Img/loading.gif" style="float:right" />
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
                <td style="width: 69px;">
                    <span class="label_title_bold">文件标题：</span>
                </td>
                <td style="width: 225px;">
                    <cc1:FSTextBox ID="txtDocumentTitle" runat="server" Width="216" MaxLength="200" CssClass="txtbox_yellow">
                    </cc1:FSTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="label_title_bold ">保存日期：</span>
                </td>
                <td colspan="3">
                    <cc1:FSCalendar ID="txtStartDate" runat="server" Width="100px" CssClass="txtbox_yellow" />~
                    <cc1:FSCalendar ID="txtEndDate" runat="server" Width="100px" CssClass="txtbox_yellow" />
                    <cc1:FSButton ID="btnSearch" runat="server" CssClass="btn" OnClick="btnSearch_Click"
                        Text="查询" />
                </td>
            </tr>
        </table>
        <div style="width: 100%">
            <cc1:FSGridView ID="gvDraftList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                ShowEmptyHeader="true" ShowRadioButton="false" OnRowDataBound="gvDraftList_RowDataBound"
                OnExteriorPaging="gvDraftList_ExteriorPaging" OnExteriorSorting="gvDraftList_ExteriorSorting"
                CellSpacing="1" BackColor="White">
                <Columns>
                    <asp:BoundField HeaderText=" " HeaderStyle-Width="12px" ItemStyle-CssClass="td_Center">
                    </asp:BoundField>
                    <asp:HyperLinkField DataNavigateUrlFields="ProcessID,DEF_NAME,ID" DataNavigateUrlFormatString="~/Container.aspx?ProcessID={0}&&TemplateName={1}&StepName=1&TBID={2}&MS=7"
                        HeaderText="操作" Text="处理任务" HeaderStyle-Width="55px" ItemStyle-CssClass="td_Center">
                    </asp:HyperLinkField>
                    <asp:TemplateField HeaderText="流程类型" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <%#SysString.GetPTDisplayName(Eval("DEF_NAME").ToString())%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="文号" HeaderText="文号" HeaderStyle-Width="100px"></asp:BoundField>
                    <asp:BoundField DataField="文件标题" HeaderText="文件标题"></asp:BoundField>
                    <asp:BoundField DataField="创建日期" HeaderText="创建日期" HeaderStyle-Width="120px"></asp:BoundField>
                    <asp:BoundField DataField="修改日期" HeaderText="修改日期" HeaderStyle-Width="120px"></asp:BoundField>
                    <asp:TemplateField HeaderText="删除" HeaderStyle-Width="35px" ItemStyle-CssClass="td_Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnDel" runat="server" Text="删除" CommandName='<%# DataBinder.Eval(Container.DataItem, "DEF_NAME") %>'
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' OnClick="lbtnDel_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle CssClass="gv_alternatingrow" />
            </cc1:FSGridView>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
