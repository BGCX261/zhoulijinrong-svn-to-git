<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PG_ProcessRelation.aspx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.Process.PG_ProcessRelation" %>

<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../Circulate/UC_CirculateList.ascx" TagName="UCCirculateList" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>关联函件列表</title>
    <link href="../../css/List.css" rel="stylesheet" type="text/css" />
    <link href="../../css/FormPage.css" rel="stylesheet" type="text/css" />
    <link href="../../css/Control.css" rel="stylesheet" type="text/css" />
    <link href="../../Css/Common.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table>
        <tr>
            <td style="width: 30px;">
                <img src="../../Img/i_info_view.gif" alt="" />
            </td>
            <td>
                <span id="containTitle" class="HeadTitle" runat="server">关联函件列表</span>
            </td>
        </tr>
    </table>
    <br />
    <center>
        <div style="width: 95%;">
            <cc1:FSGridView ID="gvProcessList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                OnRowDataBound="gvProcessList_RowDataBound" ShowEmptyHeader="true" ShowRadioButton="false"
                Width="100%" CellSpacing="1" BackColor="White">
                <Columns>
                    <asp:BoundField DataField="DEF_NAME" HeaderText="流程类型" HeaderStyle-Width="80px">
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="状态" SortExpression="Status" HeaderStyle-Width="42px"
                        ItemStyle-CssClass="td_Center">
                        <ItemTemplate>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DocumentNo" SortExpression="DocumentNo" HeaderText="文号"
                        HeaderStyle-Width="100px"></asp:BoundField>
                    <asp:BoundField DataField="DocumentTitle" SortExpression="DocumentTitle" HeaderText="文件标题">
                    </asp:BoundField>
                    <asp:BoundField DataField="STARTED_DATE" SortExpression="STARTED_DATE" HeaderText="发起日期"
                        HeaderStyle-Width="120px"></asp:BoundField>
                    <asp:TemplateField HeaderStyle-Width="42px" HeaderText="流程图" ItemStyle-CssClass="td_Center">
                        <ItemTemplate>
                            <img alt="流程图" onclick="window.open('/AgilePoint/ProcessViewer.aspx?PIID=<%#Eval("ProcessID") %>');"
                                src="../../AgilePoint/resource/en-us/Task.gif" style="cursor: hand" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:HyperLinkField DataNavigateUrlFields="ProcessID,DEF_NAME,ISHISTORY" DataNavigateUrlFormatString="PG_ProcessStep.aspx?PID={0}&amp;TID={1}&amp;IsHistory={2}"
                        Target="_blank" HeaderText="步骤" Text="查看" HeaderStyle-Width="32px" ItemStyle-CssClass="td_Center">
                    </asp:HyperLinkField>
                    <asp:TemplateField HeaderText="传阅单" HeaderStyle-Width="42px" ItemStyle-CssClass="td_Center">
                        <ItemTemplate>
                            <uc:UCCirculateList ID="ucCirculateList" UCProcessID='<%#Eval("ProcessID") %>' UCTemplateName='<%#Eval("DEF_NAME") %>'
                                runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </cc1:FSGridView>
        </div>
    </center>
    </form>
</body>
</html>
