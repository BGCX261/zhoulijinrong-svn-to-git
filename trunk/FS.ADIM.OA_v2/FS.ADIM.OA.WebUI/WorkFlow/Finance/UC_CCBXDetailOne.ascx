<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_CCBXDetailOne.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.Finance.UC_CCBXDetailOne" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<tr style="height: 25px;">
    <td style="text-align: center">
        <cc1:FSCalendar ID="txtStartMD" CssClass="txtbox_yellow" runat="server" MaxLength="10" Width="90%">
        </cc1:FSCalendar>
    </td>
    <td>
        <cc1:FSTextBox ID="txtQiCheng" CssClass="txtbox_yellow" runat="server" MaxLength="20" Width="90%">
        </cc1:FSTextBox>
    </td>
    <td>
        <cc1:FSCalendar ID="txtEndMD" CssClass="txtbox_yellow" runat="server" MaxLength="10" Width="90%">
        </cc1:FSCalendar>
    </td>
    <td>
        <cc1:FSTextBox ID="txtDaoDa" CssClass="txtbox_yellow" runat="server" MaxLength="20" Width="90%">
        </cc1:FSTextBox>
    </td>
    <td id="tdJiPiaoZheKou" runat="server">
        <cc1:FSTextBox ID="txtJiPiaoZheKou" CssClass="txtbox_yellow" runat="server" MaxLength="5" Width="90%">
        </cc1:FSTextBox>
    </td>
    <td>
        <cc1:FSTextBox ID="txtCheChuanPiao" CssClass="txtbox_yellow" runat="server" MaxLength="10" Width="90%">
        </cc1:FSTextBox>
    </td>
    <td>
        <cc1:FSTextBox ID="txtShiNeiJiaoTong" CssClass="txtbox_yellow" runat="server" MaxLength="10" Width="90%">
        </cc1:FSTextBox>
    </td>
</tr>
<asp:Label ID="lblTBID" runat="server" Visible="False"></asp:Label>
<asp:Label ID="lblUserID" runat="server" CssClass="hidden"></asp:Label>
