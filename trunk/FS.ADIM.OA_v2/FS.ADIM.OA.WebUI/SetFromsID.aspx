<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetFromsID.aspx.cs" Inherits="FS.ADIM.OA.WebUI.SetFromsID" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>同步老版OA的表单ID</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        --------------------------formdata处理--------------------------<br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="同步所有流程ID" OnClick="Button1_Click" Width="135px" />
        <asp:Label ID="Label1" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="公司收文老数据处理"
            Width="135px" />
        <asp:Label ID="Label2" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="Button3" runat="server" Text="函件收文老数据处理" Width="135px" OnClick="Button3_Click" />
        <asp:Label ID="Label3" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="Button4" runat="server" Text="公司发文老数据处理" Width="135px" OnClick="Button4_Click" />
        <asp:Label ID="Label4" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="Button5" runat="server" Text="工作联系单老数据处理" Width="135px" 
            onclick="Button5_Click" />
        <asp:Label ID="Label5" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="Button6" runat="server" Text="请示报告老数据处理" Width="135px" 
            onclick="Button6_Click" />
        <asp:Label ID="Label6" runat="server"></asp:Label>
        <br />
        <br />
        <br />
        --------------------------已完成流程数据移植--------------------------<br />
        <br />
        <asp:Button ID="Button7" runat="server" onclick="Button7_Click" Text="公司发文" />
        <asp:Label ID="Label7" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="Button8" runat="server" onclick="Button8_Click" Text="公司收文" />
        <asp:Label ID="Label8" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="Button9" runat="server" onclick="Button9_Click" Text="函件发文" />
        <asp:Label ID="Label9" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="Button10" runat="server" onclick="Button10_Click" Text="函件收文" />
        <asp:Label ID="Label10" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="Button11" runat="server" onclick="Button11_Click" Text="工作联系单" 
            Width="79px" />
        <asp:Label ID="Label11" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="Button12" runat="server" onclick="Button12_Click" Text="请示报告" />
        <asp:Label ID="Label12" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="Button13" runat="server" onclick="Button13_Click" Text="程序文件" />
        <asp:Label ID="Label13" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="DJGTF" runat="server" onclick="DJGTF_Click" Text="党纪工团发文" />
        <asp:Label ID="Label16" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="DJGTS" runat="server" onclick="DJGTS_Click" Text="党纪工团收文" />
        <asp:Label ID="Label17" runat="server"></asp:Label>
        <br />
        <br />
        --------------------------公司收文领导批示时间和办公司批阅时间补偿--------------------------<br />
        <br />
        <asp:Button ID="Button14" runat="server" onclick="Button14_Click" 
            Text="公司收文领导批示时间补偿现行表" Width="206px" />
        <asp:Label ID="Label14" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="Button15" runat="server" onclick="Button15_Click" 
            Text="公司收文领导批示时间补偿备份表" Width="207px" />
        <asp:Label ID="Label15" runat="server"></asp:Label>
    </div>
    </form>
<p>
    &nbsp;</p>
<p>
    &nbsp;</p>
</body>
</html>
