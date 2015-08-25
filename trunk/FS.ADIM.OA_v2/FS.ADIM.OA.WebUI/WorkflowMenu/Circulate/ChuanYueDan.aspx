<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChuanYueDan.aspx.cs" Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.Circulate.ChuanYueDan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../CSS/Common.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .SheetBorder { text-align: left; width: 510px border: 1px solid #000; margin: 0px; border-collapse: collapse; }
        .SheetBorder td { border: 1px solid #000; text-align: center; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 100%; margin-top: 20px; text-align: center;">
        <table class="SheetBorder">
            <tr>
                <td colspan="3" style="font-size: 14px; font-weight: bold; height: 30px">
                    传阅信息
                </td>
            </tr>
            <tr style="height: 30px;">
                <td style="width: 110px; background: blue; color: White; font-weight: bold;">
                    步骤
                </td>
                <td style="width: 200px; background: blue; color: White; font-weight: bold;">
                    分发人/分发时间
                </td>
                <td style="width: 200px; background: blue; color: White; font-weight: bold;">
                    接收人/阅知时间
                </td>
            </tr>
            <asp:Repeater ID="RepeaterSend" runat="server">
                <ItemTemplate>
                    <tr style="height: 30px">
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "StepName")%>
                        </td>
                        <td>
                            <%#GetDistributeDate(DataBinder.Eval(Container.DataItem, "LevelCode").ToString(), DataBinder.Eval(Container.DataItem, "SendUserName").ToString(), Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "SendDateTime")).ToString("yyyy-MM-dd HH:mm:ss"))%>
                        </td>
                        <td>
                            <%#GetUserName(DataBinder.Eval(Container.DataItem, "ReceiveUserID").ToString(), DataBinder.Eval(Container.DataItem, "ReceiveUserName").ToString())%>
                            /
                            <%#GetReadDate(DataBinder.Eval(Container.DataItem, "Is_Read").ToString(), DataBinder.Eval(Container.DataItem, "EditDate").ToString())%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <br />
    <div style="text-align: center">
        <a href="javascript:window.close();">【关闭】</a>
    </div>
    </form>
</body>
</html>
