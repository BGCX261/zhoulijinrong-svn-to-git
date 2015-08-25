<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_HuiQianOne.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.WorkRelation.UC_HuiQianOne" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="UC_CommentInfo.ascx" TagName="UC_CommentInfo" TagPrefix="uc" %>
<tr style="height: 25px;">
    <td>
        <asp:DropDownList ID="ddlSignDept" runat="server" Width="125px" AutoPostBack="True"
            OnSelectedIndexChanged="ddlSignDept_SelectedIndexChanged" CssClass="dropdownlist_yellow">
        </asp:DropDownList>
    </td>
    <td>
        <asp:Label ID="lblUserName" runat="server"></asp:Label>
    </td>
    <td>
        <asp:Label ID="lblTongYi" runat="server"></asp:Label>
    </td>
    <td>
        <asp:Label ID="lblDate" runat="server"></asp:Label>
    </td>
    <td id="tdYiJian" runat="server">
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
    <td style="width: 1px">
        <cc1:FSCheckBox ID="cb" runat="server" />
    </td>
</tr>
<asp:HiddenField ID="hfLuoShi" runat="server" />
<asp:Label ID="lblTBID" runat="server" Visible="False"></asp:Label>
<asp:Label ID="lblUserID" runat="server" CssClass="hidden"></asp:Label>
<uc:UC_CommentInfo ID="ucCommentInfo" runat="server" />
