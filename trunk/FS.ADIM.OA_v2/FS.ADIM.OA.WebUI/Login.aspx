<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FounderSoftware.ADIM.OA.WebUI._Login"
    UICulture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>登录页面</title>

</head>= 
<body bgcolor="#CDCDCD">
    <form id="form1" runat="server">
    <div align="center">
        <table border="0" width="800" height="500" background="Img/login.jpg">
            <tr height="215">
                <td>
                    <asp:TextBox ID="TextBoxFile" runat="server" Style="width: 150px; height: 15px" Text="" Visible="false" ></asp:TextBox><br/>
                    <asp:TextBox ID="TextBoxName" runat="server" Style="width: 150px; height: 15px" Text="" Visible="false"></asp:TextBox><br/>
                    <asp:Button ID="Button1" runat="server"  OnClick="imgbtnLogin_Click" Visible="false"/>
                    
                </td>
            </tr>
            <tr height="140">
                <td>
                    <table border="0" width="100%">
                        <tr height="30">
                            <td width="300">
                                &nbsp;
                            </td>
                            <td width="100" align="right">
                                <font size="2.5em">用户名：</font>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtUserName" runat="server" Style="width: 150px; height: 15px" Text=""></asp:TextBox>
                            </td>
                        </tr>
                        <tr height="30">
                            <td width="300">
                            </td>
                            <td width="100" align="right">
                                <font size="2.5em">密&nbsp;&nbsp;码：</font>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Style="width: 150px;
                                    height: 15px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr height="30">
                            <td width="300">
                            </td>
                            <td width="100" align="right">
                                <font size="2.5em">系&nbsp;&nbsp;统：</font>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlSubSys" runat="server" Width="154px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" style="color: Red;">
                    &nbsp;
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr height="135">
                <td>
                    <table border="0" width="100%">
                        <tr>
                            <td align="center" width="350">
                            </td>
                            <td align="left">
                                <asp:ImageButton ID="imgbtnLogin" runat="server" CausesValidation="true" ImageUrl="Img/bt_login.gif"
                                    OnClick="imgbtnLogin_Click" ValidationGroup="Lg" />&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="imgbtnLoginOut" runat="server" CausesValidation="false" ImageUrl="Img/bt_cancel.gif" />
                            </td>
                        </tr>
                        <tr height="50">
                            <td align="center">
                            </td>
                            <td align="center">
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <font size="2.5em"><b></b></font>
                            </td>
                            <td align="center">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
