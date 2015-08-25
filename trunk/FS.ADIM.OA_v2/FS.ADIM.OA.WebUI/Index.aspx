<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="FS.ADIM.OA.WebUI.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312">
    <title>海南核电文件流转系统 V1.09.9</title>
</head><!--<%=FounderSoftware.ADIM.SSO.Utility.SSOUtility.TopUrl%>  -->
<frameset rows="89,*" cols="*" framespacing="0" frameborder="NO" border="1">

<frame src="Top.aspx" name="top" frameborder="no" scrolling="no" noresize marginwidth="0" marginheight="0" id="top">
    
 
    <frameset rows="*" cols="150,*" name="leftframe" framespacing="0" frameborder="NO">
        <frame src="left.aspx" name="left" frameborder="no" scrolling="no" id="left" target="main">
        <frame src="Container.aspx?target=<%=target %>" name="main" frameborder="no" id="main" scrolling="Auto" >
    </frameset>
</frameset>
<noframes>
    <body class="html">
    </body>
</noframes>
</html>
