<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_GoOnCirculate.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.Circulate.UC_GoOnCirculate" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../../PageOU/UC_OASelect.ascx" TagName="OASelectUC" TagPrefix="uc" %>
<div class="divCenter">
    <table class="table_form_noborder" border="0" cellpadding="0" style="text-align: left">
        <tr id="tr1" runat="server">
            <td style="width: 80px;">
                <span class="label_title_bold">分发范围：</span>
            </td>
            <td>
                <cc1:FSTextBox ID="txtCirculateNames" runat="server" CssClass="txtbox_yellow" TextMode="MultiLine"
                    Style="width: 573px; height: 50px"></cc1:FSTextBox>
                <uc:OASelectUC ID="UCDeptMember1" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="width: 80px;">
                <asp:Label ID="lblComment" runat="server" Text="传阅意见：" CssClass="label_title_bold"
                    Visible="false"></asp:Label>
            </td>
            <td>
                <cc1:FSTextBox ID="txtCommentView" CssClass="txtbox_blue" Style="width: 280px; height: 50px;"
                    runat="server" TextMode="MultiLine"></cc1:FSTextBox>
                <cc1:FSTextBox ID="txtCommentEdit" CssClass="txtbox_yellow" Style="width: 283px; height: 50px;"
                    runat="server" TextMode="MultiLine"></cc1:FSTextBox>
            </td>
        </tr>
    </table>
</div>
<asp:HiddenField ID="txtCirculatesIDs" runat="server" />
<asp:HiddenField ID="txtCirculatesDeptIDs" runat="server" />
