<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Top.aspx.cs" Inherits="FS.ADIM.OA.WebUI.Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <title>�˵纣�ϰ칫�ĵ�һ�廯ϵͳ</title>
    <link href="Css/Common.css" rel="stylesheet" type="text/css" />
    <link href="Css/Top.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">

        function selectNavigation(obj) {
            var illist = document.getElementsByTagName('li');
            for (i = 0; i < illist.length; i++) {
                illist[i].className = illist[i].className.replace(" selected", "");
            }
            obj.parentNode.className = obj.parentNode.className + " selected";
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="table_top">
            <tr>
                <td class="table_top_left">
                </td>
                <td class="td_right">
                </td>
            </tr>
        </table>
        <table class="table_navigationbar">
            <tr>
                <td class="td_left" style="width: 70%">
                    <div class="img_logon">
                        ���ã�<asp:Label ID="lblUserInfo" runat="server" Text=""></asp:Label></div>
                </td>
                <td align="left">
                    <div style="display: none">
                        <asp:TextBox ID="txtURL" runat="server" Text="Container.aspx?TID=������ϵ��&ViewID=1"
                            Width="650px"></asp:TextBox>
                        <input id="btnGo" type="button" value="ת��" onclick="javascript:parent.document.getElementById('main').src=txtURL.value" />
                        <asp:HyperLink ID="HyperLink1" runat="server">�ĵ���</asp:HyperLink></div>
                </td>
                <td class="td_right">
                    <ul>
                        <li class="img_logoff">
                            <asp:LinkButton ID="btnLogOutSys" runat="server" Text="ע��" Visible="false" OnClick="btnLogOutSys_Click"></asp:LinkButton>
                            <asp:LinkButton ID="btnLogOut" runat="server" Text="ע��" Visible="false" OnClick="btnLogOut_Click"></asp:LinkButton>
                            <asp:LinkButton ID="btnLogOutSSO" runat="server" Text="ע��" Visible="false" OnClick="btnLogOutSSO_Click"></asp:LinkButton>
                        </li>
                        <li class="img_help"><a href="���Ϻ˵�OA�û������ֲ�.doc" target="_blank">����</a></li>
                        ||
                        <li class="img_org">
                            <asp:LinkButton ID="btnOU" runat="server" Text="��֯����" OnClick="btnSys_Click"></asp:LinkButton>
                        </li>
                        ||
                        <li class="img_bi">
                            <asp:LinkButton ID="btnSys" runat="server" Text="ϵͳ����" OnClick="btnSys_Click"></asp:LinkButton>
                        </li>
                        ||
                    </ul>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hDisplayID" runat="server" Value="1" />
    </form>
</body>
</html>
