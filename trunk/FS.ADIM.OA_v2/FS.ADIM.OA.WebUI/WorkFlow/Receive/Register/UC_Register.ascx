<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Register.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.Receive.Register.UC_Register" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../../../PageWF/UC_FileControl.ascx" TagName="FileControlUC" TagPrefix="uc" %>
<%@ Register Src="../../../PageOU/UC_Company.ascx" TagName="UCCompany" TagPrefix="uc" %>

<script type="text/javascript">
    $("containTitle").innerHTML = "收文登记 - " + "<%= TemplateName %>";
    function DoClampCheck() {
        if ($('<%= txtDocumentNo.ClientID %>').value == "") {
            alert("请先选择一条收文记录");
            event.returnValue = false;
            return;
        }
        event.returnValue = true;
    }
    function DoDeleteConfirm() {
        event.returnValue = window.confirm("确实要删除这笔收文登记单吗？");
    }
    function PackageCheck() {
        DoClampCheck();
        event.returnValue && DoDeleteConfirm();
    }
</script>

<table class="table_add_mid offsetRight" cellpadding="3" cellspacing="2" style="table-layout: fixed">
    <col style="width: 75px" />
    <col style="width: 146px" />
    <col style="width: 75px" />
    <col style="width: 146px" />
    <col style="width: 69px" />
    <col style="width: 146px" />
    <tr runat="server" id="trProcessTemplate" visible="false">
        <td>
            <span class="label_title_bold">流程类型：</span><span class="label_title_red">*</span>
        </td>
        <td colspan="5">
            <cc1:FSDropDownList ID="ddlProcessTemplate" runat="server" CssClass="dropdownlist_yellow"
                AutoPostBack="true" Style="width: 141px" OnSelectedIndexChanged="ddlProcessTemplate_SelectedIndexChanged">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem Value="工会收文">工会收文</asp:ListItem>
                <asp:ListItem Value="团委收文">团委收文</asp:ListItem>
                <asp:ListItem Value="党委纪委收文">党委纪委收文</asp:ListItem>
            </cc1:FSDropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">文件标题：</span><span class="label_title_red">*</span>
        </td>
        <td colspan="5">
            <cc1:FSTextBox ID="txtDocumentTitle" runat="server" CssClass="txtbox_yellow" Style="width: 583px"
                MaxLength="200"></cc1:FSTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">来文单位：</span><span class="label_title_red">*</span>
        </td>
        <td colspan="5">
            <cc1:FSTextBox ID="txtReceiveUnit" runat="server" CssClass="txtbox_yellow" Style="width: 560px"
                TabIndex="-1" MaxLength="250"></cc1:FSTextBox>
            <uc:UCCompany ID="ucSendUnit" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">收文号：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtDocumentNo" runat="server" CssClass="txtbox_blue" Style="width: 135px"
                TabIndex="-1" MaxLength="10"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label_title_bold">收文年份：</span>
        </td>
        <td>
            <cc1:FSDropDownList ID="ddlReceiveYear" runat="server" CssClass="dropdownlist_yellow"
                Style="width: 141px">
                <asp:ListItem></asp:ListItem>
            </cc1:FSDropDownList>
        </td>
        <td>
            <span class="label_title_bold">收文日期：</span>
        </td>
        <td>
            <cc1:FSCalendar ID="txtReceiveDate" runat="server" CssClass="txtbox_yellow" Style="width: 139px" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">行文号：</span><span class="label_title_red">*</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtSendNo" runat="server" CssClass="txtbox_yellow" Style="width: 135px"
                MaxLength="50" Text="〔〕"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label_title_bold">行文日期：</span><span class="label_title_red">*</span>
        </td>
        <td>
            <cc1:FSCalendar ID="txtSendDate" runat="server" CssClass="txtbox_yellow" Style="width: 135px" />
        </td>
        <td>
            <span class="label_title_bold">主题词：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtKeyWord" runat="server" CssClass="txtbox_yellow" Style="width: 139px"
                MaxLength="120"></cc1:FSTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">正文页数：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtPageCount" runat="server" CssClass="txtbox_yellow" Style="width: 135px"
                MaxLength="6"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label_title_bold">份数：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtShareCount" runat="server" CssClass="txtbox_yellow" Style="width: 135px"
                MaxLength="6"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label_title_bold">附件/页数：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtAttchCount" runat="server" CssClass="txtbox_yellow" Style="width: 139px"
                MaxLength="6"></cc1:FSTextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 69px">
            <span class="label_title_bold">保管期限：</span>
        </td>
        <td>
            <cc1:FSDropDownList ID="ddlKeepTime" runat="server" CssClass="dropdownlist_yellow"
                Style="width: 141px">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem>永久</asp:ListItem>
                <asp:ListItem>10年</asp:ListItem>
                <asp:ListItem>30年</asp:ListItem>
                <asp:ListItem>不归档</asp:ListItem>
            </cc1:FSDropDownList>
        </td>
        <td style="width: 69px">
            <span class="label_title_bold">密级：</span>
        </td>
        <td>
            <cc1:FSDropDownList ID="ddlSecretLevel" runat="server" CssClass="dropdownlist_yellow"
                Style="width: 141px">
                <asp:ListItem Text=""></asp:ListItem>
                <asp:ListItem Text="秘密"></asp:ListItem>
                <asp:ListItem Text="机密"></asp:ListItem>
                <asp:ListItem Text="绝密"></asp:ListItem>
            </cc1:FSDropDownList>
        </td>
        <td style="width: 69px">
            <span class="label_title_bold">紧急程度：</span>
        </td>
        <td>
            <cc1:FSDropDownList ID="ddlUrgentDegree" runat="server" CssClass="dropdownlist_yellow"
                Style="width: 145px">
                <asp:ListItem Text=""></asp:ListItem>
                <asp:ListItem Text="一般"></asp:ListItem>
                <asp:ListItem Text="紧急"></asp:ListItem>
            </cc1:FSDropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">预立卷号：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtPreVolumnNo" runat="server" CssClass="txtbox_yellow" Style="width: 135px"
                MaxLength="32"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label_title_bold">归档状态：</span><cc1:FSCheckBox ID="chkIsArchiveDummy"
                runat="server" Style="display: none" />
        </td>
        <td colspan="3">
            <cc1:FSTextBox ID="txtArchiveStatus" runat="server" ReadOnly="true" CssClass="txtbox_blue"
                Width="135px" MaxLength="32"></cc1:FSTextBox>
            <cc1:FSCheckBox ID="chkIsArchive" runat="server" Text="直接归档" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">备注：</span>
        </td>
        <td colspan="5">
            <cc1:FSTextBox ID="txtRemark" runat="server" CssClass="txtbox_yellow" Style="width: 583px"
                MaxLength="300"></cc1:FSTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">附件：</span>
        </td>
        <td colspan="5">
            <uc:FileControlUC ID="ucFileList" runat="server" />
        </td>
    </tr>
</table>
<hr class="hrDashed" />
<div id="divSubmit" style="margin-left: 32px;">
    <cc1:FSButton ID="btnAddNew" runat="server" CssClass="btn" Text="新增" OnClick="btnAddNew_Click"
        OnClientClick="return FSTextBox_SubmitCheck()" />
    <cc1:FSButton ID="btnModify" runat="server" CssClass="btn" Text="修改" OnClick="btnModify_Click"
        OnClientClick="DoClampCheck(); FSTextBox_SubmitCheck()" />
    <cc1:FSButton ID="btnDelete" runat="server" CssClass="btn" Text="删除" OnClick="btnDelete_Click"
        OnClientClick="PackageCheck()" />
    <cc1:FSButton ID="btnLaunch" runat="server" CssClass="btn" Text="启动" OnClick="btnLaunch_Click"
        OnClientClick="DoClampCheck()" />
    <cc1:FSButton ID="btnDetail" runat="server" CssClass="btn" Text="详细信息" OnClick="btnDetail_Click"
        OnClientClick="DoClampCheck()" />
    <cc1:FSButton ID="btnPrint" runat="server" CssClass="btn" Text="清单打印" OnClick="btnPrint_Click" />
    <cc1:FSButton ID="btnQuery" runat="server" CssClass="btn" Text="查询" OnClick="btnQuery_Click"
        Style="margin-left: 210px;" />
</div>
<hr class="hrDashed" />
<table class="table_add_mid offsetRight" cellpadding="3" cellspacing="2" style="table-layout: fixed">
    <tr>
        <td style="width: 75px">
            <span class="label_title_bold">收文号：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtQueryDocNoFrom" runat="server" CssClass="txtbox_yellow" Style="width: 143px"
                MaxLength="10"></cc1:FSTextBox><cc1:FSTextBox ID="txtQueryDocNoTo" runat="server"
                    CssClass="txtbox_yellow" Style="width: 143px" MaxLength="10"></cc1:FSTextBox>
        </td>
        <td style="width: 69px;">
            <span class="label_title_bold">文件标题：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtQueryDocTitle" runat="server" CssClass="txtbox_yellow" Style="width: 143px"
                MaxLength="200"></cc1:FSTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">收文日期：</span>
        </td>
        <td>
            <cc1:FSCalendar ID="txtQueryRecDateFrom" runat="server" CssClass="txtbox_yellow"
                Style="width: 143px" /><cc1:FSCalendar ID="txtQueryRecDateTo" runat="server" CssClass="txtbox_yellow"
                    Style="width: 143px" />
        </td>
        <td>
            <span class="label_title_bold">收文年份：</span>
        </td>
        <td>
            <cc1:FSDropDownList ID="ddlQueryRecYear" runat="server" CssClass="txtbox_yellow"
                Width="148px">
                <asp:ListItem></asp:ListItem>
            </cc1:FSDropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">来文单位：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtQueryRecUnit" runat="server" CssClass="txtbox_yellow" Style="width: 255px;"
                MaxLength="250"></cc1:FSTextBox>
            <uc:UCCompany ID="CompanyUC2" runat="server" />
        </td>
        <td>
            <span class="label_title_bold">状态：</span>
        </td>
        <td>
            <cc1:FSDropDownList ID="ddlQueryStatus" Width="148px" runat="server" CssClass="dropdownlist_yellow">
                <asp:ListItem Text=""></asp:ListItem>
                <asp:ListItem Text="未完成"></asp:ListItem>
                <asp:ListItem Text="已归档"></asp:ListItem>
            </cc1:FSDropDownList>
        </td>
    </tr>
</table>
<hr class="hrDashed" />
<div style="width: 100%" style="margin-top: 10px;">
    <cc1:FSGridView ID="gvRegisterList" runat="server" AllowPaging="True" AllowSorting="true"
        AutoGenerateColumns="False" ShowEmptyHeader="true" ShowRadioButton="false" PageType="ExteriorPage"
        OnExteriorPaging="gvRegisterList_ExteriorPaging" OnExteriorSorting="gvRegisterList_ExteriorSorting"
        CellSpacing="1" BackColor="White">
        <Columns>
            <asp:TemplateField HeaderText="选择" HeaderStyle-Width="30px" ItemStyle-CssClass="td_Center">
                <ItemTemplate>
                    <a href="#" onclick="CallServer(this);" cid='<%# DataBinder.Eval(Container.DataItem, "ID") %>'>
                        选择 </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="收文号" DataField="ReceiveNo" HeaderStyle-Width="68px" />
            <asp:BoundField HeaderText="行文号" DataField="SendLetterNo" HeaderStyle-Width="120px" />
            <asp:BoundField HeaderText="收文日期" DataField="ReceiveDate" DataFormatString="{0:yyyy-MM-dd}"
                HeaderStyle-Width="70px" />
            <asp:BoundField HeaderText="文件标题" DataField="DocumentTitle" />
            <asp:BoundField HeaderText="来文单位" DataField="ReceiveUnit" HeaderStyle-Width="180px" />
            <asp:BoundField HeaderText="密级" DataField="SecretLevel" ItemStyle-CssClass="td_Center"
                HeaderStyle-Width="30px" />
            <asp:TemplateField HeaderText="状态" HeaderStyle-Width="50px" ItemStyle-CssClass="td_Center">
                <ItemTemplate>
                    <%# GetStatus(Eval("ProcessID"), Eval("ArchiveStatus"))%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <AlternatingRowStyle CssClass="gv_alternatingrow" />
    </cc1:FSGridView>
</div>
<div style="display: none">
    隐藏区域,记录流程数据<br />
    <cc1:FSTextBox ID="txtRegisterID" runat="server"></cc1:FSTextBox></div>

<script type="text/javascript">
var HighlightRow = null;
function CallServer(e) {
    var rid = e.cid;
    <%= this.Page.ClientScript.GetCallbackEventReference(this, "rid", "ReceiveServerData",null)%>;
    if(HighlightRow != null) {
        for(i = 0;i<HighlightRow.cells.length;i++) {
            HighlightRow.cells[i].style.backgroundColor="";
        }
    }
    HighlightRow = e.offsetParent.parentNode;
    for(i = 0;i<HighlightRow.cells.length;i++) {
        HighlightRow.cells[i].style.backgroundColor="#88A8C1";
    }
    event.returnValue = false;
}
Date.prototype.toCommonCase=function(){
　　var Year=this.getYear();
　　var Month=this.getMonth()+1;
　　if(Month<10){
　　　　Month="0"+Month;
　　}
　　var Day=this.getDate();
　　if(Day<10){
　　　　Day="0"+Day;
　　}
　　return Year+"-"+Month+"-"+Day;
}
var INT_MINVALUE = "-2147483648";
function ReceiveServerData(rValue) {
    eval("var a = " + rValue);
    $('<%= txtRegisterID.ClientID %>').value = a.ID;
    $('<%= txtReceiveUnit.ClientID %>').value = a.ReceiveUnit;
    $('<%= ddlReceiveYear.ClientID %>').value = a.ReceiveYear;
    $('<%= txtDocumentNo.ClientID %>').value = a.ReceiveNo;
    $('<%= txtReceiveDate.ClientID %>').value = a.ReceiveDate.toCommonCase();
    $('<%= txtSendNo.ClientID %>').value = a.SendLetterNo;
    $('<%= txtSendDate.ClientID %>').value = a.SendLetterDate.toCommonCase();
    $('<%= txtDocumentTitle.ClientID %>').value = a.DocumentTitle;
    $('<%= txtKeyWord.ClientID %>').value = a.SubjectWord;
    $('<%= txtPageCount.ClientID %>').value = a.PageCount==INT_MINVALUE?"":a.PageCount;
    $('<%= txtShareCount.ClientID %>').value = a.ShareCount==INT_MINVALUE?"":a.ShareCount;
    $('<%= txtAttchCount.ClientID %>').value = a.AttachmentCount==INT_MINVALUE?"":a.AttachmentCount;
    $('<%= ddlKeepTime.ClientID %>').value = a.KeepTime;
    $('<%= ddlSecretLevel.ClientID %>').value = a.SecretLevel;
    $('<%= ddlUrgentDegree.ClientID %>').value = a.UrgentDegree;
    $('<%= txtPreVolumnNo.ClientID %>').value = a.PreVolumeNo;
    $('<%= txtRemark.ClientID %>').value = a.Remarks;
    $('<%= txtArchiveStatus.ClientID %>').value = a.ArchiveStatus;
    $('<%= btnLaunch.ClientID %>').disabled = a.ProcessID == ""? false:true;
    $('<%= btnDetail.ClientID %>').disabled = a.ProcessID != ""? false:true;
    var Is_Archive = $('<%= chkIsArchive.ClientID %>');
    Is_Archive.checked = a.Is_Archive == "1" ? true:false;
    Is_Archive.disabled = a.Is_Archive == "1" ? "disabled":"";
    Is_Archive.nextSibling.disabled = a.Is_Archive == "1" ? "disabled":"";
    if(Is_Archive.parentNode != null) {
        Is_Archive.parentNode.disabled = a.Is_Archive == "1" ? "disabled":"";
    }
    $('<%= ucFileList.HiddenClientID %>').value =  a.FileData;
    var location = document.frames('<%= ucFileList.PopIframeID %>').location;
    location.href = location.href;
}
</script>

