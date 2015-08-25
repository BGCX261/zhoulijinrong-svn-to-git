<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_LDHuiQian.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.ProgramFile.UC_LDHuiQian" %>
<table class="table_border" style="margin-bottom: 5px; text-align: center">
    <tr style="height: 25px" class="trListHead">
        <td style="width: 12%">
            公司领导
        </td>
        <td style="width: 10%">
            同意/否决
        </td>
        <td style="width: 12%">
            日期
        </td>
        <td>
            意见
        </td>
        <td id="td" runat="server" visible="false">
            落实情况
        </td>
        <td id="tdDate" runat="server" visible="false">
            日期
        </td>
    </tr>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
</table>
<asp:Button ID="btnAdd" runat="server" Text="新增" OnClick="btnAdd_Click" CssClass="btn" />
<asp:Button ID="btnRemove" runat="server" Text="删除" OnClick="btnRemove_Click" CssClass="btn" />