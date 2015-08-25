<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_HuiQian.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.ProgramFile.UC_HuiQian" %>
<table class="table_border" style="margin-bottom: 5px; text-align:center">
    <tr style="height: 25px" class="trListHead">
        <td style="width: 14%">
            会签部门
        </td>
        <td style="width: 13%">
            操作人
        </td>
        <td style="width: 12%">
            同意/否决
        </td>
        <td style="width: 13%">
            日期
        </td>
        <td>
            意见
        </td>
        <td id="td" runat="server" visible="false">
            落实情况
        </td>
        <td id="tdDate" runat="server" visible="false" style="width: 13%">
            日期
        </td>
        <td style="width: 36px">
            删除
        </td>
    </tr>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
</table>
<asp:Button ID="btnAdd" runat="server" Text="新增" OnClick="btnAdd_Click" CssClass="btn" />
<asp:Button ID="btnRemove" runat="server" Text="删除" OnClick="btnRemove_Click" CssClass="btn" />
