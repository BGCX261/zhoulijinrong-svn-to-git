<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_FileControl.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.PageWF.UC_FileControl" %>
<iframe id="<%=PopIframeID %>" src="#" scrolling="no" width="100%" marginwidth="0"
    framespacing="0" marginheight="0" frameborder="0" onload="AutoFitHeight(this)">
</iframe>
<asp:HiddenField ID="txtUCXML" runat="server" />

<script type="text/javascript">
    function OpenAttachment(UCControlID, PopIframeID, UCProcessType, UCProcessID, UCIsEditable, UCWorkItemID, UCTBID, UCIsAgain) {
        if ($(UCControlID) == null) {
            return;
        }        
        $(PopIframeID).src = "/PageWF/PG_FileControl.aspx?UCControlID=" + UCControlID + "&UCProcessType=" + UCProcessType + "&UCProcessID=" + UCProcessID + "&UCIsEditable=" + UCIsEditable + "&UCWorkItemID=" + UCWorkItemID + "&UCTBID=" + UCTBID + "&UCIsAgain=" + UCIsAgain + "&Time=" + new Date();
        
    }
</script>