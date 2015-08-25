<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_CommentList.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.PageWF.UC_CommentList" %>
<table cellpadding="3" cellspacing="1" style="border: 1px solid #000000;margin-bottom:5px;table-layout:fixed">
    <tr style="background-color: #ADD9E6; height: 20px;">
        <td style="width: 30px; text-align: center;">
            序号
        </td>
        <td style="width: 70px; text-align: center;">
            步骤名
        </td>
        <td style="width: 50px; text-align: center;">
            参与人
        </td>
        <td style="width: 120px; text-align: center;">
            完成时间
        </td>
        <td style="word-break: break-all; text-align: center; width: 300px;">
            意见
        </td>
    </tr>
    <asp:Repeater ID="rptCommentList" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <%# Container.ItemIndex + 1 %>
                </td>
                <td>
                    <%# Eval("ViewName")%>
                </td>
                <td>
                    <%# Eval("UserName")%>
                </td>
                <td>
                    <%# Eval("FinishTime")%>
                </td>
                <td style="word-break: break-all;">
                    <span style="color: Red">(<%# Eval("SubmitAction")%>)</span>
                    <%# Eval("Content")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
