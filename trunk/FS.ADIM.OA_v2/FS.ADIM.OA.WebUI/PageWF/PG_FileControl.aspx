<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PG_FileControl.aspx.cs"
    Inherits="FS.ADIM.OA.WebUI.PageWF.PG_FileControl" %>

<%@ Register TagPrefix="Upload" Namespace="Brettle.Web.NeatUpload" Assembly="Brettle.Web.NeatUpload" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Css/Common.css" rel="stylesheet" type="text/css" />
    <link href="../Css/Attach.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
    //office在线编辑
        function OpenWebDoc(url){
            window.open(url);
        }
        function Init(ctlThis, ctlParent){
            if (parent.document.getElementById('"+ctlParent+"') != null) {
                document.getElementById('"+ctlThis+"').value = parent.document.getElementById('"+ctlParent+"').value;
            }
        }
        function AttachmentWork(fjCtl, serviceCtl){
            try {
                newObj = new ActiveXObject("EDITORONLINE.EditorOnlineCtrl.1");
                if (newObj == undefined || newObj == null) {
                    alert("请下载在线编辑控件并添加进可信任站点！");
                }
                else {
                    xml = parent.document.getElementById(fjCtl).value;
                    servicePath = document.getElementById(serviceCtl).value;
                    document.getElementById("IEPlugin").SetDocLibLst(xml);
                    document.getElementById("IEPlugin").SetConfigInfo(servicePath);
                    document.getElementById("IEPlugin").ShowAxWnd();
                    document.getElementById("txtFJXML").value = document.getElementById("IEPlugin").GetDocLibLst();
                }
            } catch (e) {
                alert(e);
            }
        }
        function AttachmentOpen(fjCtl, serviceCtl, title) {
            try {
                newObj = new ActiveXObject("EDITORONLINE.EditorOnlineCtrl.1");
                if (newObj == undefined || newObj == null) {
                    alert("请下载在线编辑控件并添加进可信任站点！");
                }
                else {
                    xml = parent.document.getElementById(fjCtl).value;
                    servicePath = document.getElementById(serviceCtl).value;
                    document.getElementById("IEPlugin").SetDocLibLst(xml);
                    document.getElementById("IEPlugin").SetConfigInfo(servicePath);
                    document.getElementById("IEPlugin").AutoShowAxWnd(title);
                }
            } catch (e) {
                alert(e);
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: left; margin: 0px">
        <table style="width: 590px;" runat="server" id="tbUpload">
            <tr>
                <td style="width: 50px;">
                    <Upload:MultiFile ID="multiFile" runat="server" UseFlashIfAvailable="true">
                    </Upload:MultiFile>
                </td>
                <td style="width: 60px;">
                    <asp:LinkButton ID="btnUpload" runat="server" Style="color: #0099ff" Text="开始上传"
                        OnClick="btnUpload_Click" />
                </td>
                <td style="width: 60px;">
                    <asp:LinkButton ID="btnOL" runat="server" Text="在线编辑" OnClick="btnOL_Click" Style="font-size: 12px;
                        color: #0099ff" />
                </td>
                <td style="width: 120px;">
                    <a href="Office/Editor.zip" target="_blank" style="color: #0099ff">控件下载v2.6.8.0</a>
                </td>
                <td style="width: 300px; text-align: right;">
                    <asp:LinkButton ID="LinkButton1" runat="server" Style="color: #0099ff" ToolTip="清除未上传的文件">全部清除</asp:LinkButton>
                </td>
            </tr>
        </table>
        <div class="div_top">
            <table class="tb_top" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 15px; background-color: #ADD9E6">
                        &nbsp;
                    </td>
                    <td style="width: 280px; background-color: #ADD9E6">
                        文件名称
                    </td>
                    <td style="width: 80px; background-color: #ADD9E6">
                        大小
                    </td>
                    <td style="width: 110px; background-color: #ADD9E6">
                        查看
                    </td>
                    <td style="width: 60px; background-color: #ADD9E6">
                        删除
                    </td>
                    <td style="width: 60px; background-color: #ADD9E6">
                        状态
                    </td>
                    <td style="width: 60px; background-color: #ADD9E6">
                        正文
                    </td>
                </tr>
            </table>
        </div>
        <div class="div_bottom">
            <table id="Files_Table" class="tb_bottom" cellpadding="0" cellspacing="0">
                <tr style="height: 0px;">
                    <td style="width: 15px;">
                    </td>
                    <td style="width: 280px;">
                    </td>
                    <td style="width: 80px;">
                    </td>
                    <td style="width: 110px;">
                    </td>
                    <td style="width: 60px;">
                    </td>
                    <td style="width: 60px;">
                    </td>
                    <td style="width: 60px;">
                    </td>
                </tr>
                <asp:Repeater ID="RepeaterFiles" runat="server" OnItemDataBound="RepeaterFiles_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td style="width: 15px; text-align: center;">
                                <%# Container.ItemIndex + 1 %>
                            </td>
                            <td style="width: 280px;" title="<%#DataBinder.Eval(Container.DataItem, "Alias")%>">
                                <img src="../../NeatUpload/images/ic<%#"doc,docx,xls,xlsx,ppt,pptx,pdf,txt,jpg,pdf".Contains(DataBinder.Eval(Container.DataItem, "Type").ToString())?DataBinder.Eval(Container.DataItem, "Type").ToString():"gen"%>.gif"
                                    alt="" style="border: 0px" />
                                <%#GetTitleName(DataBinder.Eval(Container.DataItem, "Alias").ToString(), DataBinder.Eval(Container.DataItem, "Type").ToString())%>
                            </td>
                            <td style="width: 80px;">
                                <%# DataBinder.Eval(Container.DataItem, "Size").ToString() == "" ? "&nbsp;" : DataBinder.Eval(Container.DataItem, "Size").ToString()%>
                            </td>
                            <td style="width: 110px;">
                                <asp:LinkButton ID="btnDownload" runat="server" Text="查看" CommandName='<%#DataBinder.Eval(Container.DataItem, "URL").ToString() %>'
                                    Visible="false" />
                                <a href="FileDownLoad.aspx?TemplateName=<%#UCProcessType%>&Alias=<%# DataBinder.Eval(Container.DataItem, "Alias")%>&URL=<%#DataBinder.Eval(Container.DataItem, "URL") %>"
                                    target="_blank">查看</a>
                                <asp:LinkButton ID="btnEdit" runat="server" OnClick="btnEdit_Click" Text="编辑" ToolTip="需要安装在线编辑控件V2.6.2.0以上版本"
                                    CommandName='<%#DataBinder.Eval(Container.DataItem, "Title") %>' />
                            </td>
                            <td style="width: 60px;">
                                <asp:LinkButton ID="btnDel" runat="server" CommandName='<%# Eval("URL") %>' ForeColor="Blue"
                                    OnClick="btnDel_Click"><%#CheckChuanYue2(DataBinder.Eval(Container.DataItem, "Type").ToString())%>
                                </asp:LinkButton>
                            </td>
                            <td style="width: 60px;">
                                已上传
                            </td>
                            <td style="width: 60px;">
                                <asp:CheckBox ID="chkStatus" runat="server" AutoPostBack="true" OnCheckedChanged="chkStatus_CheckedChanged" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <div style="width: 585px;" runat="server" id="divUpload">
            <Upload:ProgressBar CssClass="ProgressBar" ID="inlineProgressBar" runat="server"
                Inline="true" Triggers="submitButton linkButton commandButton htmlInputButtonButton htmlInputButtonSubmit" />
        </div>
        <asp:HiddenField ID="txtFJXML" runat="server" />
        <div style="display: none">
            <asp:HiddenField ID="txtServicePath" runat="server" />
            <asp:Panel ID="Panel1" runat="server">
                <object id="IEPlugin" classid="clsid:B810BF98-8736-4824-B674-521A7E7369B1">
                    <param name="_Vertion" value="65536" />
                    <param name="_ExtentX" value="2646" />
                    <param name="_ExtentY" value="1323" />
                    <param name="_StockProps" value="0" />
                </object>
            </asp:Panel>
        </div>
    </div>
    </form>
</body>
</html>
