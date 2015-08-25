<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PG_Comment.aspx.cs" Inherits="FS.ADIM.OA.WebUI.PageWF.PG_Comment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/Common.css" rel="stylesheet" type="text/css" />
    <link href="../css/PopPage.css" rel="stylesheet" type="text/css" />
    <link href="../css/List.css" rel="stylesheet" type="text/css" />
    <link href="../Css/FormPage.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-left: 10px; margin-top: 10px; color: Blue">
        <asp:Label ID="MsgLabel" runat="server"></asp:Label></div>
    <div style="margin: 10px; width: 360">
        <asp:Repeater runat="server" ID="rptComments">
            <ItemTemplate>
                <div class="lblcontent">
                    <ul class="rank_list">
                        <li style="clear: left; float: left;margin:auto 2px;"><em>
                            <%#Container.ItemIndex + 1%>
                        </em></li>
                    </ul>
                    <%#DataBinder.Eval(Container.DataItem, "UserName")%>：[<%#DataBinder.Eval(Container.DataItem, "FinishTime")%>]
                    <%#DataBinder.Eval(Container.DataItem, "Content")%>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>
