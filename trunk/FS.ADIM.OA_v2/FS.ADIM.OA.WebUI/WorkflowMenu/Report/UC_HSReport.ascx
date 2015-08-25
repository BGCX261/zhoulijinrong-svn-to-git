<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_HSReport.ascx.cs" Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.Report.UC_HSReport" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<div>
    <table>
        <tr>
            <td style="width: 69px;">
                <span class="label_title_bold ">函件类型：</span>
            </td>
            <td style="width: 115px;">
                <cc1:FSDropDownList ID="ddlProcessTemplate" runat="server" AutoPostBack="true" DataTextField="Name"
                    DataValueField="Name" CssClass="dropdownlist_yellow" Width="110px">
                </cc1:FSDropDownList>
            </td>
            <td>
                <cc1:FSButton ID="btnSearch" runat="server" CssClass="btn" Text="查询" 
                    onclick="btnSearch_Click" />
            </td>
        </tr>
    </table>
    <div style="width: 100%">
            <cc1:FSGridView ID="gvProcessList" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" ShowEmptyHeader="true" ShowRadioButton="false" PageType="ExteriorPage"
                CellSpacing="1" BackColor="White"  OnExteriorPaging="gvProcessList_ExteriorPaging"
                >
                <Columns>
                    <asp:BoundField DataField="type" HeaderText="来文类型" HeaderStyle-Width="80px">
                    </asp:BoundField>                    
                    <asp:BoundField DataField="number"  HeaderText="序号"
                        HeaderStyle-Width="100px"></asp:BoundField>
                    <asp:BoundField DataField="receivecompany"  HeaderText="收文单位">
                    </asp:BoundField>                    
                </Columns>
                <AlternatingRowStyle CssClass="gv_alternatingrow" />
            </cc1:FSGridView>
        </div>    
</div>
