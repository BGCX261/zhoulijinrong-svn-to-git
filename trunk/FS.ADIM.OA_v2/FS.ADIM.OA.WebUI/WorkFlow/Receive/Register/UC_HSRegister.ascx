<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_HSRegister.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.Receive.Register.UC_HSRegister" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="../../../PageWF/UC_FileControl.ascx" TagName="FileControl" TagPrefix="uc" %>
<%@ Register Src="../../../PageOU/UC_Company.ascx" TagName="Company" TagPrefix="uc" %>

<script type="text/javascript">
    function DoClampCheck() {
        if ($('<%= txtDocumentNo.ClientID %>').value == "") {
            alert("请先选择一条收文记录");
            event.returnValue = false;
            return false;
        }
        event.returnValue = true;
        return true;
    }
        function DoSecFFConfirm() {
        event.returnValue = window.confirm("请确定已经修改了登记信息,进行二次分发吗？");
    }
    function PackageCheck() {
        DoClampCheck();
        event.returnValue && DoSecFFConfirm();
    }
</script>

<table class="table_add_mid offsetRight" cellpadding="3" cellspacing="2" style="table-layout: fixed">
    <col style="width: 80px" />
    <col style="width: 146px" />
    <col style="width: 73px" />
    <col style="width: 146px" />
    <col style="width: 75px" />
    <col style="width: 146px" />
    <tr>
        <td>
            <span class="label_title_bold">文件标题 ：</span><span class="label_title_red">*</span>
        </td>
        <td colspan="5">
            <cc1:FSTextBox ID="txtDocumentTitle" runat="server" CssClass="txtbox_yellow" Style="width: 583px"
                MaxLength="250" ToolTip="最多250字符"></cc1:FSTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">单位：</span><span class="label_title_red">*</span>
        </td>
        <td colspan="5">
            <cc1:FSTextBox ID="txtReceiveUnit" runat="server" CssClass="txtbox_yellow" Style="width: 560px"
                TabIndex="-1" MaxLength="250"></cc1:FSTextBox>
            <uc:Company ID="ucCompany" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">收文号：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtDocumentNo" runat="server" CssClass="txtbox_blue" Style="width: 135px"
                TabIndex="-1" MaxLength="50" ToolTip="最多50字符"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label_title_bold">文件编码：</span>
        </td>
        <td>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <cc1:FSTextBox ID="txtDocumentEncoding" runat="server" CssClass="txtbox_yellow" Style="width: 135px"
                    MaxLength="50" ToolTip="最多50字符" Onblur="textchanged();return false;"></cc1:FSTextBox>
            </ContentTemplate>
        </asp:UpdatePanel>        
        </td>
        <td>
            <span class="label_title_bold">收文日期：</span><span class="label_title_red">*</span>
        </td>
        <td>
            <cc1:FSCalendar ID="txtReceiveDate" runat="server" Style="width: 135px" CssClass="txtbox_yellow" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">答复文号：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtReplyDocumentNo" runat="server" CssClass="txtbox_yellow" Style="width: 135px"
                MaxLength="50" ToolTip="最多50字符"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label_title_bold">其他编码：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtOtherEncoding" runat="server" CssClass="txtbox_yellow" Style="width: 135px"
                MaxLength="50" ToolTip="最多50字符"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label_title_bold">行文日期：</span>
        </td>
        <td>
            <cc1:FSCalendar ID="txtXingWenDate" runat="server" Style="width: 135px" CssClass="txtbox_yellow" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">函件类型：</span>
        </td>
        <td colspan="3">
            <cc1:FSDropDownList ID="ddlDocumentType" runat="server" CssClass="dropdownlist_yellow"
                Style="width: 360px" AppendDataBoundItems="True">
            </cc1:FSDropDownList>
        </td>
        <td>
            <span class="label_title_bold">形成日期：</span>
        </td>
        <td>
            <cc1:FSCalendar ID="txtFormationDate" runat="server" Style="width: 135px" CssClass="txtbox_yellow" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">紧急程度：</span>
        </td>
        <td>
            <cc1:FSDropDownList ID="ddlUrgentDegree" runat="server" CssClass="dropdownlist_yellow"
                Style="width: 137px">
                <asp:ListItem Text=""></asp:ListItem>
                <asp:ListItem Text="普通"></asp:ListItem>
                <asp:ListItem Text="紧急"></asp:ListItem>
            </cc1:FSDropDownList>
        </td>
        <td>
            <span class="label_title_bold">页数：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtPageCount" runat="server" CssClass="txtbox_yellow" Style="width: 135px"
                MaxLength="6"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label_title_bold">保管期限：</span>
        </td>
        <td>
            <cc1:FSDropDownList ID="ddlKeepTime" runat="server" CssClass="dropdownlist_yellow"
                Style="width: 137px">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem>永久</asp:ListItem>
                <asp:ListItem>长期</asp:ListItem>
                <asp:ListItem>短期</asp:ListItem>
            </cc1:FSDropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">对应合同号：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtContractNumber" runat="server" CssClass="txtbox_yellow" Style="width: 135px"
                MaxLength="50"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label_title_bold">设备代码：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtEquipmentCode" runat="server" CssClass="txtbox_yellow" Style="width: 135px"
                MaxLength="50"></cc1:FSTextBox>
        </td>
        <td>
            <span class="label_title_bold">HN编码：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtHNCode" runat="server" CssClass="txtbox_yellow" Style="width: 135px"
                MaxLength="50"></cc1:FSTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">备注：</span>
        </td>
        <td colspan="5">
            <cc1:FSTextBox ID="txtRemark" runat="server" CssClass="txtbox_yellow" Style="width: 583px"
                MaxLength="50" Width="636px" Height="82px" TextMode="MultiLine" ToolTip="最多1200字符"></cc1:FSTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">附件：</span>
        </td>
        <td colspan="5">
            <uc:FileControl ID="ucAttachment" runat="server" />
        </td>
    </tr>
 </table>
<hr class="hrDashed" />
<div id="divSubmit" style="margin-left: 32px;">
    <cc1:FSButton ID="btnAddNew" runat="server" CssClass="btn" Text="新增" OnClick="btnAddNew_Click"
        OnClientClick="return FSTextBox_SubmitCheck()" />
    <cc1:FSButton ID="btnModify" runat="server" CssClass="btn" Text="修改" OnClick="btnModify_Click"
        OnClientClick="if(DoClampCheck()){ FSTextBox_SubmitCheck()}" />
    <cc1:FSButton ID="btnLaunch" runat="server" CssClass="btn" Text="启动" OnClick="btnLaunch_Click"
        OnClientClick="DoClampCheck()" />
    <cc1:FSButton ID="btnDetail" runat="server" CssClass="btn" Text="详细信息" OnClick="btnDetail_Click"
        OnClientClick="DoClampCheck()" />
            <cc1:FSButton ID="btnSecFF" runat="server" CssClass="btn" Text="二次分发"
        OnClientClick="PackageCheck()" onclick="btnSecFF_Click" />
    <cc1:FSButton ID="btnQuery" runat="server" CssClass="btn" Text="查询" OnClick="btnQuery_Click"
        Style="margin-left: 250px;" />
</div>
<hr class="hrDashed" />
<table class="table_add_mid offsetRight" cellpadding="3" cellspacing="2" style="table-layout: fixed">
    <tr>
        <td style="width: 80px">
            <span class="label_title_bold">收文号：</span>
        </td>
        <td>
            <cc1:FSTextBox ID="txtQueryDocNoFrom" runat="server" CssClass="txtbox_yellow" Style="width: 130px"
                MaxLength="10"></cc1:FSTextBox>~<cc1:FSTextBox ID="txtQueryDocNoTo" runat="server"
                    CssClass="txtbox_yellow" Style="width: 130px" MaxLength="10"></cc1:FSTextBox>
        </td>
        <td style="width: 73px">
            <span class="label_title_bold">收文日期：</span>
        </td>
        <td>
            <cc1:FSCalendar ID="txtQueryRecDateFrom" runat="server" CssClass="txtbox_yellow"
                Style="width: 89px" />~<cc1:FSCalendar ID="txtQueryRecDateTo" runat="server" CssClass="txtbox_yellow"
                    Style="width: 89px" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">文件标题：</span>
        </td>
        <td colspan="3">
            <cc1:FSTextBox ID="txtQueryDocTitle" runat="server" CssClass="txtbox_yellow" Style="width: 583px"
                MaxLength="250" ToolTip="最多250字符"></cc1:FSTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label_title_bold">单位：</span>
        </td>
        <td colspan="3">
            <cc1:FSTextBox ID="txtQueryRecUnit" runat="server" CssClass="txtbox_yellow" Style="width: 560px"
                MaxLength="250"></cc1:FSTextBox>
            <uc:Company ID="ucQueryCompany" runat="server" />
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
                    <a href="#" onclick="CallServer(this);event.returnValue = false;" cid='<%# DataBinder.Eval(Container.DataItem, "ID") %>'>
                        选择 </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ProcessName" HeaderText="流程类型" HeaderStyle-Width="60px"
                Visible="false"></asp:BoundField>
            <asp:BoundField DataField="UrgentDegree" HeaderText="紧急程度" HeaderStyle-Width="60px"
                ItemStyle-CssClass="td_Center"></asp:BoundField>
            <asp:BoundField DataField="DocumentNo" HeaderText="文号" HeaderStyle-Width="68px">
            </asp:BoundField>
            <asp:BoundField DataField="DocumentTitle" HeaderText="文件标题"></asp:BoundField>
            <asp:BoundField DataField="ReceiptDate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="收文日期"
                HeaderStyle-Width="70px"></asp:BoundField>
            <asp:BoundField DataField="XingWenDate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="行文日期"
                HeaderStyle-Width="70px"></asp:BoundField>
            <asp:TemplateField HeaderText="状态" HeaderStyle-Width="50px" ItemStyle-CssClass="td_Center">
                <ItemTemplate>
                    <%# GetStatus(Eval("ProcessID"))%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:FSGridView>
</div>
<div style="display: none">
    隐藏区域,记录流程数据<br />
    <cc1:FSTextBox ID="txtRegisterID" runat="server"></cc1:FSTextBox>
    <cc1:FSTextBox ID="txtProcessID" runat="server"></cc1:FSTextBox></div>
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
var DATETIME_MAX = "100-01-01";
function ReceiveServerData(rValue) {
    eval("var a = " + rValue);
    $('<%= txtRegisterID.ClientID %>').value = a.ID;
    $('<%= txtProcessID.ClientID %>').value = a.ProcessID;
    $('<%= txtReceiveUnit.ClientID %>').value = a.CommunicationUnit;
    $('<%= txtDocumentNo.ClientID %>').value = a.DocumentNo;
    $('<%= txtDocumentEncoding.ClientID %>').value = a.FileEncoding;
    $('<%= txtReceiveDate.ClientID %>').value = a.ReceiptDate.toCommonCase();
    $('<%= txtReplyDocumentNo.ClientID %>').value = a.ReplyDocumentNo;
    $('<%= txtOtherEncoding.ClientID %>').value = a.OtherEncoding;
    $('<%= txtXingWenDate.ClientID %>').value = a.XingWenDate.toCommonCase()!=DATETIME_MAX?a.XingWenDate.toCommonCase():"";
    $('<%= txtDocumentTitle.ClientID %>').value = a.DocumentTitle;
    $('<%= ddlDocumentType.ClientID %>').value = a.LetterType;
    var FormationDate = a.FormationDate.toCommonCase();
    $('<%= txtFormationDate.ClientID %>').value = FormationDate==DATETIME_MAX?"":FormationDate;
    $('<%= ddlUrgentDegree.ClientID %>').value = a.UrgentDegree;
    $('<%= txtPageCount.ClientID %>').value = a.Pages==INT_MINVALUE?"":a.Pages;
    $('<%= ddlKeepTime.ClientID %>').value = a.KeepTime;
    $('<%= txtContractNumber.ClientID %>').value = a.ContractNumber;
    $('<%= txtEquipmentCode.ClientID %>').value = a.EquipmentCode;
    $('<%= txtHNCode.ClientID %>').value = a.HNCode;
    $('<%= btnLaunch.ClientID %>').disabled = !(a.ProcessID == "");
    $('<%= btnDetail.ClientID %>').disabled = a.ProcessID == "";
    $('<%= btnSecFF.ClientID %>').disabled = a.ProcessID == "";
    $('<%= ucAttachment.HiddenClientID %>').value =  a.FileData;
    $('<%= txtRemark.ClientID %>').value = a.Remarks;
    var location = document.frames('<%= ucAttachment.PopIframeID %>').location;
    location.href = location.href;

}
</script>
<script type="text/javascript">
    function textchanged(arg){
        var oTb = document.getElementById('<%=txtDocumentEncoding.ClientID %>');
        // arg中是传给服务器的变量
        arg = oTb.value;
        <%=this.Page.ClientScript.GetCallbackEventReference(this, "arg", "receiveServerResult", null, true)%>
    }   
    function receiveServerResult(result){
        // 在这里添加处理服务器返回结果的逻辑，result变量是服务器返回的结果
        if(result!="0"){
        alert("通讯渠道号错误，请确认!");
        }
    }
</script>


