<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PG_SignCommentInfo.aspx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.ProgramFile.PG_SignCommentInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="font-size: 13px">
        <table style="border-top: none; border: solid 1px #EAEAEA; width: 100%">
            <tr style="color: #333; height: 25px; font-size: 12px; font-weight: bold; background: #deeef4">
                <td style="width: 30px">
                    序号
                </td>
                <td style="width: 50px">
                    参与人
                </td>
                <td style="width: 70px">
                    提交日期
                </td>
                <td>
                    意见
                </td>
                <td>
                    落实情况
                </td>
            </tr>
            <asp:Repeater ID="rptComment" runat="server">
                <ItemTemplate>
                    <tr style="line-height: 1.5em">
                        <td style="text-align: center; background-color: #F7F7F7">
                            <%# Container.ItemIndex + 1 %>
                        </td>
                        <td style="background-color: #F8F6EB; width: 50px">
                            <%# Eval("UserName")%>
                        </td>
                        <td style="background-color: #F7F7F7; width: 70px">
                            <%# Eval("FinishTime")%>
                        </td>
                        <td style="background-color: #F8F6EB; word-break: break-all; word-wrap: break-word; width:170px">
                            <%# Eval("Content")%>
                        </td>
                        <td style="background-color: #F7F7F7">
                            <%#Eval("DealCondition")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    </form>
</body>
</html>
