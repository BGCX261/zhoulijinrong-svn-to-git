<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Comment.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.PageWF.UC_Comment" %>
<a href="#" style="text-decoration: underline; font-weight: bold" onclick="OpenYJPage('<%=UCProcessID %>','<%=UCTemplateName %>','<%=UCStepName %>','<%=UCWorkItemID%>','<%=PopIframeID%>');ShowPopDiv( '<%=PopDivID %>');">
    查看意见</a>
<div id="<%=PopDivID %>" style="display: none; width: 400px; height: 350px;" class="PopDiv">
    <div class="PopDivTitle">
        <div style="float: left">
            <%=UCStepName%>
        </div>
        <a class="clsWinR" title="关闭" onclick="ClosePopDiv('<%=PopDivID %>')"></a>
    </div>
    <iframe id="<%=PopIframeID %>" src="#" scrolling="auto" width="100%" height="98%"
        marginwidth="0" framespacing="0" marginheight="0" frameborder="0"></iframe>
</div>

<script type="text/javascript">
    new Drag("<%=PopDivID %>");
    function OpenYJPage(tbid, ptype, view, workid, pop) {
        $(pop).src = "/PageWF/PG_Comment.aspx?ProcessID=" + tbid + "&TemplateName=" + ptype + "&StepName=" + view + "&WorkItemID=" + workid;
    } 
</script>

