<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_WaitReading.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.Circulate.UC_WaitReading" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="UC_CirculateList.ascx" TagName="UCCirculateList" TagPrefix="uc" %>
<%@ Import Namespace="FS.ADIM.OA.BLL.Common" %>
<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="false"
    EnableScriptLocalization="false">
</asp:ScriptManager>
<div class="divProgress">
    <asp:UpdateProgress ID="UP" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <img alt="" src="Img/loading.gif" style="float: right" />
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table>
            <tr>
                <td style="width: 69px;">
                    <span class="label_title_bold ">流程类型：</span>
                </td>
                <td style="width: 115px;">
                    <cc1:FSDropDownList ID="ddlProcessTemplate" runat="server" AutoPostBack="true" DataTextField="Name"
                        DataValueField="Name" OnSelectedIndexChanged="ddlProcessTemplate_SelectedIndexChanged"
                        CssClass="dropdownlist_yellow" Width="110px">
                    </cc1:FSDropDownList>
                </td>
                <td style="width: 69px;">
                    <span class="label_title_bold">文件标题：</span>
                </td>
                <td style="width: 225px;">
                    <cc1:FSTextBox ID="txtDocumentTitle" runat="server" Width="216" CssClass="txtbox_yellow"
                        MaxLength="200">
                    </cc1:FSTextBox>
                </td>
                <td style="width: 69px;">
                    <span class="label_title_bold ">文号：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtDocumentNo" runat="server" Width="100px" CssClass="txtbox_yellow"
                        MaxLength="50">
                    </cc1:FSTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="label_title_bold ">分发人：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtSponsor" runat="server" Width="106px" CssClass="txtbox_yellow"
                        MaxLength="20">
                    </cc1:FSTextBox>
                </td>
                <td>
                    <span class="label_title_bold ">分发日期：</span>
                </td>
                <td>
                    <cc1:FSCalendar ID="txtStartDate" runat="server" Width="100px" CssClass="txtbox_yellow"
                        MaxLength="10" />~
                    <cc1:FSCalendar ID="txtEndDate" runat="server" Width="100px" CssClass="txtbox_yellow"
                        MaxLength="10" />
                </td>
                <td colspan="2">
                    <cc1:FSButton ID="btnSearch" runat="server" CssClass="btn" OnClick="btnSearch_Click"
                        Text="查询" Style="margin-left: 7px;" />
                </td>
                <td colspan="2">
                    <cc1:FSButton ID="btnMarker" runat="server" CssClass="btn"  OnClick="btnMarker_Click"
                        Text="批量阅知" Style="margin-left: 20px;" />
                </td>
            </tr>
        </table>
        <div style="width: 100%">
            <cc1:FSGridView ID="gvTaskList" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" ShowEmptyHeader="true" ShowRadioButton="false" PageType="ExteriorPage"
                OnRowDataBound="gvTaskList_RowDataBound" OnExteriorPaging="gvTaskList_ExteriorPaging"
                OnExteriorSorting="gvTaskList_ExteriorSorting"  CellSpacing="1" BackColor="White">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-CssClass="td_Center">
                        <HeaderTemplate>
                            <input type="checkbox" id="selectall" onclick="GetAllCheckBox(this);" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div align="center"><input type="checkbox" runat="server" id="cbxContact" /></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText=" " HeaderStyle-Width="12px" ItemStyle-CssClass="td_Center">
                    </asp:BoundField>
                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-CssClass="td_Center">
                        <ItemTemplate>
                            <asp:Label ID="lblLeaderCirculate" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:HyperLinkField DataNavigateUrlFields="DEF_NAME,ProcessID,WorkItemID,TBID,CirculateID,SendDateTime"
                        DataNavigateUrlFormatString="~/Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkFlow.Circulate.PG_Circulate&TemplateName={0}&ProcessID={1}&WorkItemID={2}&TBID={3}&CirculateID={4}&IsRead=False&RDT={5:yyyy/MM/dd hh时mm分ss秒}&IsHistory=1&MS=3&ID={4}"
                        HeaderText="操作" Text="查看" HeaderStyle-Width="30px" ItemStyle-CssClass="td_Center">
                    </asp:HyperLinkField>
                    <asp:TemplateField HeaderText="流程类型" SortExpression="DEF_NAME" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <%#SysString.GetPTDisplayName(Eval("DEF_NAME").ToString()) + GetIsAgaionCirculate(Eval("StepName").ToString())%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DocumentNo" SortExpression="DocumentNo" HeaderText="文号"
                        HeaderStyle-Width="100px"></asp:BoundField>
                    <asp:BoundField DataField="DocumentTitle" SortExpression="DocumentTitle" HeaderText="文件标题">
                    </asp:BoundField>
                    <asp:BoundField DataField="SendUserName" SortExpression="SendUserName" HeaderText="分发人"
                        HeaderStyle-Width="50px"></asp:BoundField>
                    <asp:BoundField DataField="SendDateTime" SortExpression="SendDateTime" HeaderText="分发时间"
                        HeaderStyle-Width="120px"></asp:BoundField>
                    <asp:TemplateField HeaderStyle-Width="42px" ItemStyle-CssClass="td_Center" HeaderText="流程图">
                        <ItemTemplate>
                            <img alt="流程图" onclick="window.open('/AgilePoint/ProcessViewer.aspx?PIID=<%#Eval("ProcessID") %>');"
                                src="../../AgilePoint/resource/en-us/Task.gif" style="cursor: hand" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="快速阅知" HeaderStyle-Width="58px" ItemStyle-CssClass="td_Center" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbnQuickMarker" runat="server" Text="标记阅知" CommandName='<%# DataBinder.Eval(Container.DataItem, "DEF_NAME") %>'
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CirculateID") %>' OnClick="lbnQuickMarker_Click" Visible="false"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="传阅单" HeaderStyle-Width="42px" ItemStyle-CssClass="td_Center">
                        <ItemTemplate>
                            <uc:UCCirculateList ID="ucCirculateList" UCProcessID='<%#Eval("ProcessID") %>' UCTemplateName='<%#Eval("DEF_NAME") %>'
                                runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle CssClass="gv_alternatingrow" />
            </cc1:FSGridView>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    function GetAllCheckBox(parentItem) {
        var items = document.getElementsByTagName("input");
        for (i = 0; i < items.length; i++) {
            if (parentItem.checked) {
                if (items[i].type == "checkbox") {
                    items[i].checked = true;
                }
            }
            else {
                if (items[i].type == "checkbox") {
                    items[i].checked = false;
                }
            }
        }
    }
</script>
