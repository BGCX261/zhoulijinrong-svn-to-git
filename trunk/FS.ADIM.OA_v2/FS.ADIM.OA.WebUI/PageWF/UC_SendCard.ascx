<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_SendCard.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.PageWF.UC_SendCard" %>
<link href="../../css/Style_WFpage.css" rel="stylesheet" type="text/css" />
<input type="button" class="btn" value="发文卡" onclick="OpenSCPage('<%=UCProcessID.ToString() %>','<%=UCTemplateName %>','<%=UCStepName %>','<%=UCWorkItemID %>','<%=PopIframeID%>');" />

<script type="text/javascript">
    function OpenSCPage(tbid, ptype, view, itemID, pop) {
        window.open("/PageWF/PG_SendCard.aspx?ProcessID=" + tbid + "&ProcessType=" + ptype + "&WorkItemID=" + itemID + "&ViewName=" + view, "", "width=700,height=500,toolbar=no,scrollbars=yes,menubar=no,resizable=yes");
    }
</script>