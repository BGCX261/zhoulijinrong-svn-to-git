<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Report1.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.Report.UC_Report1" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<link href="../../CSS/FormPage.css" rel="stylesheet" type="text/css" />
<link href="../../CSS/Control.css" rel="stylesheet" type="text/css" />
<div>
    <table>
        <tr>
            <td>
                <span class="label">开始日期：</span>
            </td>
            <td>
                <cc1:FSCalendar ID="cldStartDate" CssClass="txtbox_yellow" runat="server" Width="100px" />
            </td>
            <td>
                <span class="label">结束日期：</span>
            </td>
            <td>
                <cc1:FSCalendar ID="cldEndDate" CssClass="txtbox_yellow" runat="server" Width="100px" />
            </td>
            <td>
                <cc1:FSDropDownList ID="ddlFlowType" runat="server" DataTextField="Name"
                    DataValueField="Name" CssClass="dropdownlist_yellow" Width="100px">
                </cc1:FSDropDownList>
            </td>
            <td>
                <cc1:FSButton ID="btnSearch" runat="server" CssClass="btn" Text="查询" 
                    onclick="btnSearch_Click" />
            </td>
        </tr>
    </table>
    <div>
        <asp:Label ID="lblInfo" runat="server" Text=""></asp:Label>
    </div>
</div>
