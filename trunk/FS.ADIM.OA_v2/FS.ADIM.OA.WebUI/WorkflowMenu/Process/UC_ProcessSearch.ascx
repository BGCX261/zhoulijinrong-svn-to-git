<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_ProcessSearch.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.Process.UC_ProcessSearch" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../Circulate/UC_CirculateList.ascx" TagName="UCCirculateList" TagPrefix="uc" %>
<asp:ScriptManager ID="SM" runat="server" EnableScriptGlobalization="false" EnableScriptLocalization="false">
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
                <td colspan="3">
                    <cc1:FSTextBox ID="txtDocumentTitle" runat="server" Width="305" CssClass="txtbox_yellow"
                        MaxLength="200">
                    &nbsp;&nbsp;
                    </cc1:FSTextBox>
                </td>
                <td >
                    <span class="label_title_bold" 流程名：></span>
                </td>
                <td >
                    <cc1:FSTextBox ID="txtProcInstName" runat="server" Width="106px" visible="false" CssClass="txtbox_yellow"
                        MaxLength="20" >
                    &nbsp;&nbsp;
                    </cc1:FSTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="label_title_bold">文号：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtDocumentNo" runat="server" Width="106px" CssClass="txtbox_yellow"
                        MaxLength="50">
                    &nbsp;&nbsp;
                    </cc1:FSTextBox>
                </td>
                <td>
                    <span class="label_title_bold">发起人：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtSponsor" runat="server" Width="126px" CssClass="txtbox_yellow"
                        MaxLength="20">
                    &nbsp;&nbsp;
                    </cc1:FSTextBox>
                </td>
                <td>
                    <span class="label_title_bold">发起日期：</span>
                </td>
                <td>
                    <cc1:FSCalendar ID="txtStartDate" runat="server" Width="100px" CssClass="txtbox_yellow"
                        MaxLength="10" />~
                    <cc1:FSCalendar ID="txtEndDate" runat="server" Width="100px" CssClass="txtbox_yellow"
                        MaxLength="10" />
                </td>
            </tr>
            <tr>
                
                <td>
                    <span class="label_title_bold">流程状态：</span>
                </td>
                <td>
                    <cc1:FSDropDownList ID="ddlProcessStatus" runat="server" Width="130px" CssClass="dropdownlist_yellow">
                    </cc1:FSDropDownList>
                </td>
                <td>
                    <span class="label_title_bold">处理人：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtChuLiRen" runat="server" Width="100px" CssClass="txtbox_yellow"
                        MaxLength="20">
                    &nbsp;&nbsp;
                    </cc1:FSTextBox>
                </td>
            </tr>
            <tr id="trGF" runat="server" visible="false">
                <td>
                    <span class="label_title_bold">主送单位：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtGFZhuSongDanWei" runat="server" Width="106px" CssClass="txtbox_yellow"
                        MaxLength="50">

                    &nbsp;&nbsp;

                    </cc1:FSTextBox>
                </td>
                <td>
                    <span class="label_title_bold">编制部门：</span>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="drpGFDept" runat="server" CssClass="dropdownlist_yellow" Width="130px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trGS" runat="server" visible="false">
                <td>
                    <span class="label_title_bold">来文单位：</span>
                </td>
                <td colspan="5">
                    <cc1:FSTextBox ID="txtGSReceiveUnit" runat="server" Width="106px" CssClass="txtbox_yellow"
                        MaxLength="50">

                    &nbsp;&nbsp;

                    </cc1:FSTextBox>
                </td>
            </tr>
            <tr id="trHS" runat="server" visible="false">
                <td>
                    <span class="label_title_bold">来文单位：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtHSReceiveUnit" runat="server" Width="106px" CssClass="txtbox_yellow"
                        MaxLength="50">

                    &nbsp;&nbsp;

                    </cc1:FSTextBox>
                </td>
                <td>
                    <span class="label_title_bold">通讯渠道号：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtHSFileEncoding" runat="server" Width="126px" CssClass="txtbox_yellow"
                        MaxLength="50">
                    &nbsp;&nbsp;
                    </cc1:FSTextBox>
                </td>
                <td>
                    <span class="label_title_bold">函件类型：</span>
                </td>
                <td style="color: #0099CC">
                    <asp:DropDownList ID="ddlHSLetterType" runat="server" CssClass="dropdownlist_yellow">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trHF" runat="server" visible="false">
                <td>
                    <span class="label_title_bold">主送单位：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtHFCompany" runat="server" Width="106px" CssClass="txtbox_yellow"
                        MaxLength="50">
                    &nbsp;&nbsp;
                    </cc1:FSTextBox>
                </td>
                <td>
                    <span class="label_title_bold">发文部门：</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddlHFDept" runat="server" CssClass="dropdownlist_yellow" Width="130px">
                    </asp:DropDownList>
                </td>
                <td>
                    <span class="label_title_bold">函件类型：</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddlHFLetterType" runat="server" CssClass="dropdownlist_yellow"
                        Width="300px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trPF" runat="server" visible="false">
                <td>
                    <span class="label_title_bold">编制部门：</span>
                </td>
                <td colspan="5">
                    <asp:DropDownList ID="ddlPFDept" runat="server" CssClass="dropdownlist_yellow">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trRR" runat="server" visible="false">
                <td>
                    <span class="label_title_bold">主送领导：</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMainSendleader" runat="server" CssClass="dropdownlist_yellow"
                        Width="110px">
                    </asp:DropDownList>
                </td>
                <td>
                    <span class="label_title_bold">承办处室：</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddlUnderTakeDept" runat="server" CssClass="dropdownlist_yellow"
                        Width="130px">
                    </asp:DropDownList>
                </td>
                <td>
                    <span class="label_title_bold">编制部门：</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddlHostDept" runat="server" CssClass="dropdownlist_yellow">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trWR" runat="server" visible="false">
                <td>
                    <span class="label_title_bold">编制部门：</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddlWRHostDept" runat="server" CssClass="dropdownlist_yellow"
                        Width="110px">
                    </asp:DropDownList>
                </td>
                <td>
                    <span class="label_title_bold">主送部门：</span>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlWRMainSendDept" runat="server" CssClass="dropdownlist_yellow"
                        Width="130px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <div>
            <cc1:FSButton ID="btnSearch" runat="server" CssClass="btn" OnClick="btnSearch_Click"
                Text="查询" /><asp:CheckBox ID="chbHistorySearch" runat="server" Text="历史库" />
                <span class="label_title_bold" style="color:Red">请自行添加查询条件后点击查询(一个月前完结的流程请在历史库中查询)</span>
        </div>
        <div style="width: 100%">
            <cc1:FSGridView ID="gvProcessList" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" ShowEmptyHeader="true" ShowRadioButton="false" PageType="ExteriorPage"
                OnRowDataBound="gvProcessList_RowDataBound" OnExteriorPaging="gvProcessList_ExteriorPaging"
                OnExteriorSorting="gvProcessList_ExteriorSorting" CellSpacing="1" BackColor="White">
                <Columns>
                    <asp:BoundField DataField="PDEF_NAME" HeaderText="流程类型" HeaderStyle-Width="80px">
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
                    <asp:BoundField DataField="Drafter" SortExpression="Drafter" HeaderText="发起人" HeaderStyle-Width="50px"
                        Visible="false"></asp:BoundField>
                    <asp:BoundField DataField="STARTED_DATE" SortExpression="STARTED_DATE" HeaderText="发起日期"
                        HeaderStyle-Width="120px"></asp:BoundField>
                    <asp:BoundField DataField="ReceiveUserName" SortExpression="ReceiveUserName" HeaderText="处理人"
                        HeaderStyle-Width="50px" Visible="false"></asp:BoundField>
                    <asp:TemplateField HeaderText="流程图" HeaderStyle-Width="42px" ItemStyle-CssClass="td_Center">
                        <ItemTemplate>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:HyperLinkField DataNavigateUrlFields="Proc_Inst_ID,PDEF_NAME,ISHISTORY" DataNavigateUrlFormatString="PG_ProcessStep.aspx?PID={0}&amp;TID={1}&amp;IsHistory={2}"
                        Target="_blank" HeaderText="步骤" Text="查看" HeaderStyle-Width="32px" ItemStyle-CssClass="td_Center">
                    </asp:HyperLinkField>
                    <asp:TemplateField HeaderText="传阅单" HeaderStyle-Width="42px" ItemStyle-CssClass="td_Center">
                        <ItemTemplate>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="关联" HeaderStyle-Width="30px" ItemStyle-CssClass="td_Center">
                        <ItemTemplate>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle CssClass="gv_alternatingrow" />
            </cc1:FSGridView>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
