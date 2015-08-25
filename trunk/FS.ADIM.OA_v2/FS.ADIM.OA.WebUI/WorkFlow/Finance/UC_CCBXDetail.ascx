<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_CCBXDetail.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.Finance.UC_CCBXDetail" %>
<table class="table_border" style="margin-bottom: 5px; text-align: center">
    <tr style="height: 25px" id="trCC1" runat="server">
        <th colspan="4" style="text-align: center">
            起止时间 地址
        </th>
        <th rowspan="2" style="width: 70px; text-align: center">
            机票折扣
        </th>
        <th rowspan="2" style="width: 70px; text-align: center">
            车船票
        </th>
        <th rowspan="2" style="width: 70px; text-align: center">
            市内交通费
        </th>
    </tr>
    <tr style="height: 25px" id="trCC2" runat="server">
        <th style="width: 80px; text-align: center">
            年-月-日
        </th>
        <th style="width: 120px; text-align: center">
            起程
        </th>
        <th style="width: 80px; text-align: center">
            年-月-日
        </th>
        <th style="width: 120px; text-align: center">
            到达
        </th>
    </tr>
    <tr style="height: 25px" id="trPX1" runat="server" visible="false">
        <th colspan="4" style="text-align: center">
            起止时间 地址
        </th>
        <th rowspan="2" style="width: 70px; text-align: center">
            机票折扣
        </th>
        <th rowspan="2" style="width: 70px; text-align: center">
            车船票
        </th>
        <th rowspan="2" style="width: 70px; text-align: center">
            市内交通费
        </th>
    </tr>
    <tr style="height: 25px" id="trPX2" runat="server" visible="false">
        <th style="width: 80px; text-align: center">
            年-月-日
        </th>
        <th style="width: 120px; text-align: center">
            起程
        </th>
        <th style="width: 80px; text-align: center">
            年-月-日
        </th>
        <th style="width: 120px; text-align: center">
            到达
        </th>
    </tr>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
</table>
<asp:Button ID="btnAdd" runat="server" Text="新增" OnClick="btnAdd_Click" CssClass="btn" />
<asp:Button ID="btnRemove" runat="server" Text="删除" OnClick="btnRemove_Click" CssClass="btn" />
