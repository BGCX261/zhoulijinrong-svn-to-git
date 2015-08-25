<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PG_SignCommentInfo.aspx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.WorkRelation.PG_SignCommentInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../css/Common.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table style="border: solid 1px #EAEAEA; width: 100%">
        <tr style="color: #333; height: 25px; font-weight: bold; background: #deeef4">
            <td style="width: 30px; text-align: center">
                序号
            </td>
            <td style="width: 50px; text-align: center">
                参与人
            </td>
            <td style="width: 70px; text-align: center">
                提交日期
            </td>
            <td style="text-align: center">
                意见
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
                    <td style="background-color: #F8F6EB; word-break: break-all; word-wrap: break-word;">
                        <%# Eval("Content")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    </form>
</body>
</html>
