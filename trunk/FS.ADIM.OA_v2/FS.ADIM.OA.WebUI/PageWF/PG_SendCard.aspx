<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PG_SendCard.aspx.cs" Inherits="FS.ADIM.OA.WebUI.PageWF.PG_SendCard" %>

<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .SheetBorder
        {
            font-size: 12px;
            width: 100%;
            border: 1px solid #000;
            margin: 0px;
            border-collapse: collapse;
        }
        .SheetBorder td
        {
            font-size: 12px;
            border: 1px solid #000;
            padding: 0px;
            text-indent: 2px;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 680px;">
            <tr>
                <td style="text-align: center; font-size: 14px; font-weight: bold; height: 22px">
                    海南核电有限公司发文卡
                </td>
            </tr>
        </table>
        <asp:Repeater ID="RepeaterForm" runat="server">
            <ItemTemplate>
                <table class="SheetBorder" style="width: 660px;">
                    <tr>
                        <td style="width: 100px">
                            发文号
                        </td>
                        <td style="width: 120px">
                            <%#DataBinder.Eval(Container.DataItem, "DocumentNo").ToString() %>
                        </td>
                        <td style="width: 100px">
                            发文日期
                        </td>
                        <td style="width: 120px">
                            <%#Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "SendDate")).ToString("yyyy-MM-dd")%>
                        </td>
                        <td style="width: 100px">
                            行文号
                        </td>
                        <td style="width: 120px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            文件名称
                        </td>
                        <td colspan="5">
                            <%#DataBinder.Eval(Container.DataItem, "DocumentTitle").ToString() %>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            主题词
                        </td>
                        <td colspan="5">
                            <%#DataBinder.Eval(Container.DataItem, "SubjectWord").ToString()%>
                        </td>
                    </tr>
                    <td style="width: 100px">
                        拟稿人
                    </td>
                    <td style="width: 120px">
                        <%#DataBinder.Eval(Container.DataItem, "Drafter").ToString()%>
                    </td>
                    <td style="width: 100px">
                        校对人
                    </td>
                    <td style="width: 120px">
                        <%#DataBinder.Eval(Container.DataItem, "Checker").ToString()%>
                    </td>
                    <td style="width: 100px">
                        签发人
                    </td>
                    <td style="width: 120px">
                        <%#DataBinder.Eval(Container.DataItem, "SignerName").ToString()%>
                    </td>
                </table>
                <table class="SheetBorder" style="width: 660px; border-top: none;">
                    <tr>
                        <td style="width: 100px; border-top: none;">
                            行文日期
                        </td>
                        <td style="width: 120px; border-top: none;">
                            <%#Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "SendDate")).ToString("yyyy-MM-dd")%>
                        </td>
                        <td style="width: 60px; border-top: none;">
                            正文页数
                        </td>
                        <td style="width: 60px; border-top: none;">
                        </td>
                        <td style="width: 60px; border-top: none;">
                            附件页数
                        </td>
                        <td style="width: 60px; border-top: none;">
                        </td>
                        <td style="width: 60px; border-top: none;">
                            份数
                        </td>
                        <td style="width: 40px; border-top: none;">
                            <%#DataBinder.Eval(Container.DataItem, "ShareCount").ToString()%>
                        </td>
                        <td style="width: 60px; border-top: none;">
                            紧急程度
                        </td>
                        <td style="width: 40px; border-top: none;">
                            <%#DataBinder.Eval(Container.DataItem, "UrgentDegree").ToString()%>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:Repeater>
        <table class="SheetBorder" style="width: 660px; margin-top: 20px">
            <tr>
                <td style="width: 40px">
                    序号
                </td>
                <td style="width: 150px">
                    单位名称
                </td>
                <td style="width: 40px">
                    份数
                </td>
                <td style="width: 100px">
                    签收
                </td>
                <td style="width: 40px">
                    序号
                </td>
                <td style="width: 150px">
                    单位名称
                </td>
                <td style="width: 40px">
                    份数
                </td>
                <td style="width: 100px">
                    签收
                </td>
            </tr>
            <tr>
                <asp:Repeater ID="RepeaterSend" runat="server">
                    <ItemTemplate>
                        <td>
                            <%# Container.ItemIndex + 1 %>
                        </td>
                        <td>
                            <%#DataBinder.Eval(Container.DataItem, "ReceiveUserName").ToString()%>
                        </td>
                        <td>
                            1
                        </td>
                        <td>
                            <%#DataBinder.Eval(Container.DataItem, "SendDateTime").ToString()%>
                        </td>
                        <%# (Container.ItemIndex + 1) % 2 == 0 ? "</tr>" : ""%>
                    </ItemTemplate>
                </asp:Repeater>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
