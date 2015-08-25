<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_FormInfoList.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.FormInfoAdd.UC_FormInfoList" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>

<script language="javascript" type="text/javascript">
    function SignCheck(cbox) {
        var obj = document.getElementsByTagName("input");
        for (var i = 0; i < obj.length; i++) {
            if (obj[i].type == "checkbox") {
                obj[i].checked = false;
            }
        }
        var sid = cbox.id;
        var cb = $(sid);
        cb.checked = true;
    }
    function IsSelected() {
        var obj = document.getElementsByTagName("input");
        for (var i = 0; i < obj.length; i++) {
            if (obj[i].type == "checkbox" && obj[i].checked) {
                return confirm("是否真的复制？");
            }
        }
        var objtxt = $('<%=txtID.ClientID%>');
        if (objtxt.value != '') {
            return confirm("是否真的复制？");
        }
        alert("请选择需要复制的记录。");
        return false;
    }
</script>

<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="false"
    EnableScriptLocalization="false">
</asp:ScriptManager>
<div class="divProgress">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <img alt="" src="Img/loading.gif" style="float: right" />
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div>
            <div class="div_gv_tool">
                流程总数:<asp:Label ID="lblCount" runat="server" Text="0"></asp:Label>
                条 <span style="display: none">
                    <cc1:FSTextBox ID="txtID" runat="server"></cc1:FSTextBox></span>
            </div>
            <table>
                <tr>
                    <td>
                        <span class="label_title_bold">流程类型：</span>
                    </td>
                    <td>
                        <cc1:FSDropDownList ID="ddlProcessTemplate" runat="server" DataTextField="Name" DataValueField="Name"
                            Width="100px" CssClass="dropdownlist_yellow">
                        </cc1:FSDropDownList>
                    </td>
                    <td>
                        <span class="label_title_bold">流程名：</span>
                    </td>
                    <td colspan="2">
                        <cc1:FSTextBox ID="txtFlowName" runat="server" Width="189px" CssClass="txtbox_yellow"></cc1:FSTextBox>
                    </td>
                    <td>
                        <cc1:FSCheckBox ID="chkIsNoData" runat="server" Text="无数据" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="label_title_bold">发起人：</span>
                    </td>
                    <td>
                        <cc1:FSTextBox ID="txtSponsor" runat="server" Width="96px" CssClass="txtbox_yellow">
                        </cc1:FSTextBox>
                    </td>
                    <td>
                        <span class="label_title_bold">发起日期：</span>
                    </td>
                    <td>
                        <cc1:FSCalendar ID="txtStartDate" runat="server" Width="88px" CssClass="txtbox_yellow" />
                        <span class="label" cssclass="txtbox_yellow"></span>-<cc1:FSCalendar ID="txtEndDate"
                            runat="server" Width="88px" CssClass="txtbox_yellow" />
                    </td>
                    <td colspan="2">
                        <cc1:FSButton ID="btnSearch" runat="server" CssClass="btn" OnClick="btnSearch_Click"
                            Text="查询" />
                    </td>
                </tr>
            </table>
            <div style="width: 100%">
                <cc1:FSGridView ID="gvTaskList" runat="server" AllowPaging="True" AllowSorting="true"
                    AutoGenerateColumns="False" DataKeyNames="TBID" ShowEmptyHeader="true" ShowRadioButton="false"
                    PageType="ExteriorPage" OnExteriorPaging="gvTaskList_ExteriorPaging" OnExteriorSorting="gvTaskList_ExteriorSorting"
                    OnRowDataBound="gvTaskList_RowDataBound" PageSize="20" OnRowUpdating="gvTaskList_RowUpdating"
                    CellSpacing="1" BackColor="White">
                    <Columns>
                        <asp:HyperLinkField DataNavigateUrlFields="PDEF_NAME,TBID,WARE" DataNavigateUrlFormatString="PG_DataSave.aspx?TID={0}&amp;TBID={1}&amp;WARE={2}"
                            HeaderText="操作" Text="查看" Target="_blank" HeaderStyle-Width="30px" ItemStyle-CssClass="td_Center">
                        </asp:HyperLinkField>
                        <asp:TemplateField HeaderText="流程类型" SortExpression="DEF_NAME" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <cc1:FSLabel ID="lblTemplateName" runat="server" Text='<%#Eval("DEF_NAME")%>'></cc1:FSLabel>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Proc_Inst_Name" HeaderText="流程名" HeaderStyle-Width="100px">
                        </asp:BoundField>
                        <asp:BoundField DataField="StepName" HeaderText="当前步骤" HeaderStyle-Width="80px">
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="处理人" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <%#GetUserSatus(Eval("User_ID").ToString(), Eval("Status").ToString())%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DocumentNo" HeaderText="文号" HeaderStyle-Width="100px">
                        </asp:BoundField>
                        <asp:BoundField DataField="DocumentTitle" HeaderText="文件标题"></asp:BoundField>
                        <asp:BoundField DataField="Drafter" HeaderText="发起人" HeaderStyle-Width="50px"></asp:BoundField>
                        <asp:BoundField DataField="STARTED_DATE" HeaderText="发起日期" HeaderStyle-Width="120px">
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-Width="42px" HeaderText="流程图" ItemStyle-CssClass="td_Center">
                            <ItemTemplate>
                                <img alt="流程图" onclick="window.open('/AgilePoint/ProcessViewer.aspx?PIID=<%#Eval("Proc_Inst_ID") %>');"
                                    src="../../AgilePoint/resource/en-us/Task.gif" style="cursor: hand" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <cc1:FSLabel ID="lblID" runat="server" Text='<%#Eval("TBID") %>' Visible="false"></cc1:FSLabel>
                                <cc1:FSLabel ID="lblWID" runat="server" Text='<%#Eval("WORK_ITEM_ID") %>' Visible="false"></cc1:FSLabel>
                                <cc1:FSLabel ID="lblPID" runat="server" Text='<%#Eval("PROC_INST_ID") %>' Visible="false"></cc1:FSLabel>
                                <cc1:FSLabel ID="lblPoolID" runat="server" Text='<%#Eval("POOL_ID") %>' Visible="false"></cc1:FSLabel>
                                <cc1:FSLabel ID="lblUserID" runat="server" Text='<%#Eval("USER_ID") %>' Visible="false"></cc1:FSLabel>
                                <cc1:FSLabel ID="lblStepName" runat="server" Text='<%#Eval("STEPNAME") %>' Visible="false"></cc1:FSLabel>
                                <cc1:FSCheckBox ID="cbSelect" runat="server" onclick="SignCheck(this)" />
                                <cc1:FSLinkButton ID="lbtnCopy" runat="server" CommandName="Update" Text="复制" OnClientClick="return IsSelected();"
                                    CommandArgument='<%#Eval("TBID").ToString() %>'></cc1:FSLinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle CssClass="gv_alternatingrow" />
                    <SelectedRowStyle BackColor="#B3D0F5" Font-Bold="True" ForeColor="Black" Width="40px" />
                </cc1:FSGridView>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<cc1:FSHiddenField ID="hfID" runat="server" />
