<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PG_DataSave.aspx.cs" Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.FormInfoAdd.PG_DataSave" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
        <link href="../../css/tab.css" rel="stylesheet" type="text/css" />
    <link href="../../css/tab.winclassic[1].css" rel="stylesheet" type="text/css" />
    <link href="../../css/List.css" rel="stylesheet" type="text/css" />
    <link href="../../css/FormPage.css" rel="stylesheet" type="text/css" />
    <link href="../../css/Control.css" rel="stylesheet" type="text/css" />
    <link href="../../css/Style_WFpage.css" rel="stylesheet" type="text/css" />
    <link href="../../css/PopDiv.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="divTitle">
    
    </div>
        <asp:Panel ID="Panel1" runat="server">
        </asp:Panel>
        <div>
            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn" 
                onclick="btnSave_Click" />
        </div>
    </div>
    </form>
</body>
</html>
