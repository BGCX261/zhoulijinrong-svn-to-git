<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Print.ascx.cs" Inherits="FS.ADIM.OA.WebUI.PageWF.UC_Print" %>
<link href="../css/Style_WFpage.css" rel="stylesheet" type="text/css" />
<input id="btnP" type="button" class="btn" value="打印" onclick="ShowPopDiv( '<%=PopDivID %>');" />
<div id="<%=PopDivID %>" style="display: none; width: 300px; height: 200px;" class="PopDiv">
    <div class="PopDivTitle">
        <div style="float: left;">
            选择模板
        </div>
        <a class="clsWinR" title="关闭" onclick="ClosePopDiv('<%=PopDivID %>')"></a>
    </div>
    <table class="printTable">
        <tr>
            <td style="width: 80px">
                <%=UCTemplateName%>
            </td>
            <td style="width: 140px">
                <asp:DropDownList ID="drpMoBan" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                打印到
            </td>
            <td>
                <asp:RadioButton ID="rdoWord" runat="server" GroupName="Method" Text="Word" Checked="true" />
                <asp:RadioButton ID="rdoPDF" runat="server" GroupName="Method" Text="PDF" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnPrint" runat="server" OnClientClick="" OnClick="btnPrint_Click"
                    CssClass="btn" Text="打印" />
            </td>
        </tr>
    </table>
</div>

<script type="text/javascript">
    new Drag("<%=PopDivID %>");
</script>

