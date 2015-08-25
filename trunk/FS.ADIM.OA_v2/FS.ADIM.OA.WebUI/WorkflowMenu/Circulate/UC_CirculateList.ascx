<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_CirculateList.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.Circulate.UC_CirculateList" %>
<a href="#" id='linkCirculateList' onclick="OpenCirculatePage('<%=UCProcessID %>','<%=UCTemplateName %>')">
    传阅单</a><input type='button' value='传阅单' onclick="OpenCirculatePage('<%=UCProcessID %>','<%=UCTemplateName %>')"
        class='btn' id='btnCirculateList' style="display: none;">

<script type="text/javascript">
    function OpenCirculatePage(tbid, ptype) {
        window.open("/WorkflowMenu/Circulate/ChuanYueDan.aspx?ProcessID=" + tbid + "&TemplateName=" + ptype, "", "width=600,height=500,toolbar=no,scrollbars=yes,menubar=no,resizable=yes");
        event.returnValue = false;
    }
</script>

