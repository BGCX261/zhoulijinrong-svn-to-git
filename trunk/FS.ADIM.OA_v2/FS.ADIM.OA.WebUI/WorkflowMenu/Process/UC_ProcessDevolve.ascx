<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_ProcessDevolve.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.Process.UC_ProcessDevolve" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
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
            </tr>
        </table>
        <div style="width: 100%">
            <cc1:FSGridView ID="gvProcessList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                ShowEmptyHeader="true" ShowRadioButton="false" OnRowDataBound="gvProcessList_RowDataBound"
                OnExteriorPaging="gvProcessList_ExteriorPaging" OnExteriorSorting="gvProcessList_ExteriorSorting"
                CellSpacing="1" BackColor="White">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="序号" HeaderStyle-Width="30px"></asp:BoundField>
                    <asp:BoundField DataField="SerialNo" HeaderText="流水号" HeaderStyle-Width="40px"></asp:BoundField>
                    <asp:BoundField DataField="DocumentNo" HeaderText="文号" HeaderStyle-Width="130px">
                    </asp:BoundField>
                    <asp:BoundField DataField="DocumentTitle" HeaderText="文件标题"></asp:BoundField>
                    <asp:BoundField DataField="DevolveDate" HeaderText="归档日期" HeaderStyle-Width="120px">
                    </asp:BoundField>
                    <asp:BoundField DataField="ProcessDate" HeaderText="接收日期" HeaderStyle-Width="120px">
                    </asp:BoundField>
                    <asp:BoundField DataField="Accepter" HeaderText="接收人" HeaderStyle-Width="50px"></asp:BoundField>
                    <asp:TemplateField HeaderText="查看" HeaderStyle-Width="30" ItemStyle-CssClass="td_Center">
                        <ItemTemplate>
                            <cc1:FSLinkButton ID="lbnView" runat="server" Text='查看' OnCommand="lbnView_Command"
                                CommandArgument='<%# Eval("OAID") +","+ Eval("StepName") +","+ Eval("ProcessID") +","+ Eval("WorkItemID")%>'
                                CommandName='<%# Eval("ProcessName")%>'></cc1:FSLinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle CssClass="gv_alternatingrow" />
            </cc1:FSGridView>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
