<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_LDHuiQianOne.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.ProgramFile.UC_LDHuiQianOne" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register src="UC_CommentInfo.ascx" tagname="UC_CommentInfo" tagprefix="uc" %>
  
<tr>
    <td>
        <asp:DropDownList ID="drpUser" runat="server" Width="70px" CssClass="dropdownlist_yellow">
        </asp:DropDownList>
        <asp:Label ID="lbUser" runat="server" Text="" Visible='false'></asp:Label>
    </td>
    <td>
        <asp:Label ID="lblTongYi" runat="server" Text=""></asp:Label>
    </td>
    <td>
        <asp:Label ID="lblDate1" runat="server" Text=""></asp:Label>
    </td>
    <td id="tdLeaderYiJian" runat="server" title="点击查看历史记录">
        <asp:Repeater ID="rptCurrentList" runat="server" Visible="false">
            <ItemTemplate>
                <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName")%>'></asp:Label>
                <asp:Label ID="lblFinishTime" runat="server" Text='<%# Eval("FinishTime")%>'></asp:Label>
                <asp:Label ID="lblContent" runat="server" Text=' <%# Eval("Content")%>'></asp:Label>
                <asp:Label ID="lblDealCondition" runat="server" Text=' <%# Eval("DealCondition")%>'></asp:Label>
                <asp:Label ID="lblUserID" runat="server" Text='<%# Eval("UserID")%>'></asp:Label>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Label ID="lblComment" runat="server"></asp:Label>
    </td>
    <td id="td" runat="server" visible="false">
        <asp:Panel ID="pnlInputAndShow" runat="server">
            <asp:Label ID="lblYiJian" runat="server" Text=""></asp:Label>
        </asp:Panel>
    </td>
    <td id="tdDate" runat="server" visible="false">
        <asp:Label ID="lblDate2" runat="server" Text=""></asp:Label>
    </td>
</tr>
<asp:HiddenField ID="hfLuoShi" runat="server" />
<asp:Label ID="lblTBID" runat="server" Visible="False"></asp:Label>
<uc:UC_CommentInfo ID="ucCommentInfo" runat="server" />
