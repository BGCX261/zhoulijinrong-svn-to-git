﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Company.ascx.cs" Inherits="FS.ADIM.OA.WebUI.PageOU.UC_Company" %>
<input id="btnSelect" type="button" class="btn_short_person" onclick="OpenCompany('<%=ID%>','<%=PopIframeID%>','<%=UCNoControl %>','<%=UCIDControl %>','<%=UCNameControl %>','<%=UCIsSingle %>');
ShowPopDiv( '<%=PopDivID %>');" />
<div id="<%=PopDivID %>" style="display: none; width: 750px; height: 400px;" class="PopDiv">
    <div class="PopDivTitle">
        <div style="float: left;">
            选取外部单位
        </div>
        <a class="clsWinR" title="关闭" onclick="ClosePopDiv('<%=PopDivID %>')"></a>
    </div>
    <iframe id="<%=PopIframeID %>" src="#" scrolling="auto" width="100%" height="100%"
        marginwidth="0" framespacing="0" marginheight="0" frameborder="0"></iframe>
</div>

<script type="text/javascript">
    new Drag("<%=PopDivID %>");
    function OpenCompany(ucid, PopIframeID, id, no, name, isSingle){
        if ($(PopIframeID).src == ("#")) {
           $(PopIframeID).src ="/PageOU/PG_Company.aspx?UCID=" + ucid + "&UCNoControl=" + id + "&UCIDControl=" + no + "&UCNameControl=" + name+"&UCIsSingle="+isSingle;
       }       
    }
</script>