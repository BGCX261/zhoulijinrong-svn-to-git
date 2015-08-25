<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_CommentInfo.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.ProgramFile.UC_CommentInfo" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>

<script type="text/javascript">
    function OpenSignCommentDetail(stepName, userID, deptID) {
        var processID = "<%=UCProcessID %>";
        var workItemID ="<%=UCWorkItemID %>";
        if (userID == "") {
            $("iFrameComment").src = "WorkFlow/ProgramFile/PG_CommentInfo.aspx?processID=" + processID + "&stepName=" + stepName + "&workItemID=" + workItemID;
        } //编、校、审、批
        else {
            $("iFrameComment").src = "WorkFlow/ProgramFile/PG_SignCommentInfo.aspx?processID=" + processID + "&stepName=" + stepName + "&userID=" + userID + "&deptID=" + deptID + "&workItemID=" + workItemID;
        } //会签、质保
    }
     
</script>

<div id="popDiv" style="display: none; width: 520px; height: 295px;" class="PopDiv">
    <div class="PopDivTitle">
        <div style="float: left">
            意见内容
        </div>
        <a class="clsWinR" title="关闭" onclick="ClosePopDiv('popDiv')"></a>
    </div>
    <iframe id="iFrameComment" src="#" scrolling="auto" width="100%" height="265px" marginwidth="0"
        framespacing="0" marginheight="0" frameborder="0"></iframe>
</div>
