<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_OASelect.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.PageOU.UC_OASelect" %>
<input id="btnSelect" type="button" class="btn_short_person" onclick="OpenDeptLeader('<%=ID%>','<%=PopIframeID%>',
'<%=UCDeptNameControl %>','<%=UCDeptIDControl %>',
'<%=UCDeptUserIDControl %>','<%=UCDeptUserNameControl %>',
'<%=UCRoleUserIDControl %>','<%=UCRoleUserNameControl %>',
'<%=UCSelectType %>','<%=UCIsSingle %>',
'<%=UCShowDeptID %>','<%=UCLevel %>',
'<%=UCRole %>','<%=UCDeptShowType %>','<%=UCDeptAndUserControl %>',
'<%=UCDeptTreeUserIDControl %>','<%=UCDeptTreeUserNameControl %>',
'<%=UCAllSelect %>','<%=UCALLChecked %>','<%=UCTemplateName %>','<%=UCFormName %>'
);ShowPopDiv( '<%=PopDivID %>');" />
<div id="<%=PopDivID %>" style="display: none; width: <%=DivWidth %>px; height: <%=DivHeight%>px;"
    class="PopDiv">
    <div class="PopDivTitle">
        <div style="float: left;">
            <%=SHead %>
        </div>
        <a class="clsWinR" title="关闭" onclick="ClosePopDiv('<%=PopDivID %>')"></a>
    </div>
    <iframe id="<%=PopIframeID %>" src="#" scrolling="no" width="100%" height="100%"
        marginwidth="0" framespacing="0" marginheight="0" frameborder="0"></iframe>
</div>

<script type="text/javascript">
    new Drag("<%=PopDivID %>");
    function OpenDeptLeader(ucid, PopIframeID, UCDeptNameControl, UCDeptIDControl, UCDeptUserIDControl, UCDeptUserNameControl, UCRoleUserIDControl, UCRoleUserNameControl, UCSelectType, UCIsSingle, UCShowDeptID, UCLevel, UCRole, UCDeptShowType, UCDeptAndUserControl, UCDeptTreeUserIDControl, UCDeptTreeUserNameControl,UCAllSelect,UCALLChecked,UCTemplateName,UCFormName){
        if ($(PopIframeID).src == ("#")) {
            $(PopIframeID).src = "/PageOU/PG_OASelect.aspx?UCID=" + ucid + "&UCDeptNameControl=" + UCDeptNameControl + "&UCDeptIDControl="
         + UCDeptIDControl + "&UCDeptUserIDControl=" + UCDeptUserIDControl + "&UCDeptUserNameControl="
         + UCDeptUserNameControl + "&UCRoleUserIDControl=" + UCRoleUserIDControl + "&UCRoleUserNameControl="
         + UCRoleUserNameControl + "&UCSelectType=" + UCSelectType + "&UCIsSingle="
         + UCIsSingle + "&UCShowDeptID=" + UCShowDeptID + "&UCLevel=" + UCLevel + "&UCRole="
         + UCRole + "&UCDeptShowType=" + UCDeptShowType + "&UCDeptAndUserControl="
         + UCDeptAndUserControl + "&UCDeptTreeUserIDControl=" + UCDeptTreeUserIDControl + "&UCDeptTreeUserNameControl=" + UCDeptTreeUserNameControl
         +"&UCAllSelect="+UCAllSelect+"&UCALLChecked="+UCALLChecked+"&UCTemplateName="+UCTemplateName+"&UCFormName="+UCFormName;
    }
} 
</script>

