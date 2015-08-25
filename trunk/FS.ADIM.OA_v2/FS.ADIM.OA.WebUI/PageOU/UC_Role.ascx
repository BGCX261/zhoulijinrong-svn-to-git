<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Role.ascx.cs" Inherits="FS.ADIM.OA.WebUI.PageOU.UC_Role" %>
<input id="btnSelect" type="button" class="btn_short_person" onclick="OpenRolePage('<%=ID%>','<%=PopIframeID%>','<%=UCRoleName%>','<%=UCUserIDControl %>','<%=UCUserNameControl %>','<%=UCIsSingle %>');
ShowPopDiv('<%=PopDivID %>');" />
<div id="<%=PopDivID %>" style="display: none; width: 400px; height: 400px;" class="PopDiv">
    <div class="PopDivTitle">
        <div style="float: left;">
            选取<%=UCRoleName%>
        </div>
        <a class="clsWinR" title="关闭" onclick="ClosePopDiv('<%=PopDivID %>')"></a>
    </div>
    <iframe id="<%=PopIframeID %>" src="#" scrolling="auto" width="100%" height="100%"
        marginwidth="0" framespacing="0" marginheight="0" frameborder="0"></iframe>
</div>

<script type="text/javascript">
    new Drag("<%=PopDivID %>");
    function OpenRolePage(ID, PopIframeID, UCRoleName, UCUserIDControl, UCUserNameControl, UCIsSingle, UCDeptID) {
        if ($(PopIframeID).src == ("#")) {
             var url= "/PageOU/PG_Role.aspx?UCID=" + ID + "&UCRoleName=" + UCRoleName + "&UCUserIDControl=" + UCUserIDControl + "&UCUserNameControl=" + UCUserNameControl+"&UCIsSingle="+UCIsSingle;
             $(PopIframeID).src = url;
        }
    } 
</script>

