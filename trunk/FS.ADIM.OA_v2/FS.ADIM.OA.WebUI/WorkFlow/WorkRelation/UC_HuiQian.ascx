<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_HuiQian.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.WorkRelation.UC_HuiQian" %>
<asp:LinkButton ID="btnAdd" runat="server" Text="新增会签" OnClick="btnAdd_Click" Style="text-decoration: underline;" />
<asp:LinkButton ID="btnRemove" runat="server" Text="删除会签" OnClick="btnRemove_Click"
    Style="text-decoration: underline;" />
<table cellpadding="3" cellspacing="1" style="border: 1px solid #000000; table-layout: fixed;">
    <tr style="background-color: #ADD9E6; height: 20px;">
        <td style="width: 130px; text-align: center;">
            会签部门
        </td>
        <td style="width: 50px; text-align: center;">
            操作人
        </td>
        <td style="width: 60px; text-align: center;">
            同意/否决
        </td>
        <td style="width: 75px; text-align: center;">
            日期
        </td>
        <td style="width: 233px; text-align: center;">
            意见
        </td>
        <td style="width: 30px; text-align: center;">
            删除
        </td>
    </tr>
    <asp:PlaceHolder ID="pnlCountSignList" runat="server"></asp:PlaceHolder>
</table>
