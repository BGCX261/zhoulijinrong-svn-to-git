<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PG_ProcessStep.aspx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.Process.PG_ProcessStep" %>

<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../css/List.css" rel="stylesheet" type="text/css" />
    <link href="../../css/FormPage.css" rel="stylesheet" type="text/css" />
    <link href="../../css/Control.css" rel="stylesheet" type="text/css" />
    <link href="../../Css/Common.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td style="width: 30px;">
                        <img src="../../Img/i_info_view.gif" alt="" />
                    </td>
                    <td>
                        <span id="containTitle" class="HeadTitle" runat="server">流程步骤列表</span>
                    </td>
                </tr>
            </table>
            <br />
            <center>
                <div style="width: 95%;">
                    <cc1:FSGridView ID="gvProcessStep" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        DataKeyNames="Work_Item_ID" ShowEmptyHeader="true" ShowRadioButton="false" Width="100%"
                        OnExteriorPaging="gvProcessStep_ExteriorPaging" OnRowDataBound="gvProcessStep_RowDataBound"
                        CellSpacing="1" BackColor="White">
                        <Columns>
                            <asp:BoundField HeaderText="操作" HeaderStyle-Width="50px"></asp:BoundField>
                            <asp:BoundField DataField="Proc_Inst_Name" HeaderText="流程名" HeaderStyle-Width="100px">
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="步骤(动作)" ItemStyle-Font-Bold="true" HeaderStyle-Width="180px">
                                <ItemTemplate>
                                    <%#GetStepAndAction(Eval("StepName").ToString(), Eval("SubmitAction").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="处理人 状态" HeaderStyle-Width="120px">
                                <ItemTemplate>
                                    <%#GetUserSatus(Eval("User_ID").ToString(), Eval("Status").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Completed_Date" HeaderText="处理时间" HeaderStyle-Width="150px">
                            </asp:BoundField>
                            <asp:BoundField DataField="DocumentNo" HeaderText="文号" HeaderStyle-Width="150px">
                            </asp:BoundField>
                            <asp:BoundField DataField="DocumentTitle" HeaderText="文件标题"></asp:BoundField>
                            <asp:BoundField DataField="Drafter" HeaderText="发起人" HeaderStyle-Width="60px"></asp:BoundField>
                            <%--                        <asp:BoundField DataField="STARTED_DATE" HeaderText="发起日期">
                            <ItemStyle CssClass="td_j04" />
                            <HeaderStyle CssClass="th_j04" />
                        </asp:BoundField>--%>
                            <%--<asp:TemplateField HeaderText="" HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <%#CheckFormData(Eval("TBID").ToString())%>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        </Columns>
                        <%--                    <HeaderStyle CssClass="gv_headStyle" />
                    <SelectedRowStyle BackColor="#B3D0F5" Font-Bold="True" ForeColor="Black" Width="40px" />--%>
                    </cc1:FSGridView>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
