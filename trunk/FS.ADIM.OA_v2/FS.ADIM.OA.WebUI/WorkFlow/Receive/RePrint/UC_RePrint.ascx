<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_RePrint.ascx.cs" Inherits="FS.ADIM.OA.WebUI.WorkFlow.Receive.RePrint.UC_RePrint" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register src="../../../PageOU/UC_Company.ascx" tagname="UCCompany" tagprefix="uc" %>
<table class="table_add_mid offsetRight" cellpadding="2">
    <tr>
        <td style="width: 69px">
            <span class="label">收文号：</span>
        </td>
        <td>
            <span class="phrase">从</span>
            <cc1:FSTextBox ID="txtQueryDocNoFrom" runat="server" CssClass="txtbox_yellow" Style="width: 152px"
                MaxLength="10"></cc1:FSTextBox>
        </td>
        <td>
            <span class="phrase">至</span>
            <cc1:FSTextBox ID="txtQueryDocNoTo" runat="server" CssClass="txtbox_yellow" Style="width: 152px"
                MaxLength="10"></cc1:FSTextBox>
        </td>
        <td style="width: 69px">
            <span class="label">文件标题：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtQueryDocTitle" runat="server" CssClass="txtbox_yellow" Style="width: 141px"
                MaxLength="250"></cc1:FSTextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 69px">
            <span class="label">收文日期：</span>
        </td>
        <td>
            <span class="phrase">从</span>
            <cc1:FSCalendar ID="txtQueryRecDateFrom" runat="server" 
                CssClass="txtbox_yellow" Width="152px" />&nbsp;</td>
        <td>
            <span class="phrase">至</span>
            <cc1:FSCalendar ID="txtQueryRecDateTo" runat="server" 
                CssClass="txtbox_yellow" Width="152px" />&nbsp;</td>
        <td style="width: 69px">
            <span class="label">收文年份：</span>
        </td>
        <td>
            <cc1:FSDropDownList ID="ddlQueryRecYear" runat="server" CssClass="dropdownlist_yellow"
                Width="146px">
                <asp:ListItem Text=""></asp:ListItem>
            </cc1:FSDropDownList>
        </td>
    </tr>
    <tr>
        <td style="width: 69px">
            <span class="label">来文单位：</span>
        </td>
        <td colspan="2">
            <cc1:FSTextBox ID="txtQueryRecUnit" runat="server" CssClass="txtbox_yellow" Style="width: 355px;
                margin-left: 24px;" MaxLength="250"></cc1:FSTextBox>
            <uc:UCCompany ID="ucCompanyQuery" runat="server" />
        </td>
        <td style="width: 69px">
            <span class="label">状态：</span>
        </td>
        <td>
            <cc1:FSDropDownList ID="ddlQueryStatus" Width="146px" runat="server" CssClass="dropdownlist_yellow">
                <asp:ListItem Text=""></asp:ListItem>
                <asp:ListItem Text="未完成"></asp:ListItem>
                <asp:ListItem Text="已归档"></asp:ListItem>
            </cc1:FSDropDownList>
            <cc1:FSButton ID="btnQuery" runat="server" CssClass="btn" Text="查询" OnClick="btnQuery_Click" />
        </td>
    </tr>
</table>
<hr class="hrDashed" />
<div class="offsetRight" style="width: 95%">
    <cc1:FSGridView  ID="gdvList" runat="server" AutoGenerateColumns="False"
        AllowPaging="True" PageType="ExteriorPage" CellSpacing="1" BackColor="White" OnExteriorPaging="gvRegisterList_ExteriorPaging">
        <Columns>
            <asp:TemplateField HeaderText="序号" HeaderStyle-Width="35px">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1%></ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="收文年份" DataField="ReceiveYear" HeaderStyle-Width="80px" />
            <asp:BoundField HeaderText="收文号" DataField="ReceiveNo" HeaderStyle-Width="80px" />
            <asp:BoundField HeaderText="标题" DataField="DocumentTitle" HeaderStyle-Width="350px" />
            <cc1:FSBoundField HeaderText="收文日期" DataField="ReceiveDate" HeaderStyle-Width="90px"
                BoundFieldType="Date" />
            <asp:BoundField HeaderText="状态" DataField="ArchiveStatus" HeaderStyle-Width="60px" />
        </Columns>
        <RowStyle CssClass="td_j02" />
        <HeaderStyle CssClass="th_j02" />
    </cc1:FSGridView>
</div>
<div class="divSubmit" id="divSubmit" style="margin-left: 32px;">
    <cc1:FSButton ID="btnReturn" runat="server" CssClass="btn" Text="返回" OnClick="btnReturn_Click" />
</div>