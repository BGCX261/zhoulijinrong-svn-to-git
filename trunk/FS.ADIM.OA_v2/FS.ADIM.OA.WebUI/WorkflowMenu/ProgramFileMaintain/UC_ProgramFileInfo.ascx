<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_ProgramFileInfo.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkflowMenu.ProgramFileMaintain.UC_ProgramFileInfo" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>

<script language="javascript" type="text/javascript">
    function DelConfirm() {
        var id = $("<%=hfSelectedIndex.ClientID%>").value;
        if (id == "") {
            return false;
        } else {
            return confirm('是否真的删除？');
        }
    }
    function OpenDetailDialog(id) {
        if (id == "") {
            alert('数据异常，请联系系统管理员。');
            return false;
        } else {
            window.open("Container.aspx?ClassName=FounderSoftware.ADIM.OA.WebUI.WorkflowAdmin.InformationMaintain.PG_ProgramFileDetail&id=" + id, '', 'height=120px,width=450px');
            return false;
        }
    }
</script>

<style type="text/css">
    .gv_headStyle { background-color: #4977B4; }
    .style1 { width: 91px; }
</style>
<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="false"
    EnableScriptLocalization="false">
</asp:ScriptManager>
<div class="divProgress">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <img alt="" src="../../Img/loading11.gif" />
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <cc1:FSLabel ID="lblPrompt" runat="server" ForeColor="Red"></cc1:FSLabel>
        <div>
            <table style="font-size: 12px;">
                <tr>
                    <td style="width: 50px;">
                        类型：
                    </td>
                    <td style="width: 140px">
                        <cc1:FSDropDownList ID="ddlSort" runat="server" Width="125px" AutoPostBack="True"
                            DataTextField="text" DataValueField="value" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged"
                            CssClass="dropdownlist_yellow">
                            <asp:ListItem Value="0">--请选择--</asp:ListItem>
                            <asp:ListItem Value="1">管理程序</asp:ListItem>
                            <asp:ListItem Value="2">部门级管理程序</asp:ListItem>
                            <asp:ListItem Value="3">工作程序</asp:ListItem>
                        </cc1:FSDropDownList>
                        <span style="color: Red">*</span>
                    </td>
                    <td>
                        <span id="Label1" class="label">一级子类：</span>
                    </td>
                    <td style="width: 150px">
                        <cc1:FSDropDownList ID="ddlProgramType" runat="server" Width="120px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlProgramType_SelectedIndexChanged" CssClass="dropdownlist_yellow">
                            <asp:ListItem Value="0">请选择类型</asp:ListItem>
                        </cc1:FSDropDownList>
                        <span style="color: Red">*</span>
                    </td>
                    <td style="width: 60px">
                        <span id="Span1" class="label">二级子类：</span>
                    </td>
                    <td style="width: 150px">
                        <cc1:FSDropDownList ID="ddlProgramSubType" runat="server" Width="120px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlProgramSubType_SelectedIndexChanged" CssClass="dropdownlist_yellow">
                            <asp:ListItem Value="0">请选择一级子类</asp:ListItem>
                        </cc1:FSDropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        编码：
                    </td>
                    <td style="width: 140px">
                        <cc1:FSTextBox ID="txtCode" runat="server" MaxLength="20" Width="118px" CssClass="txtbox_yellow"></cc1:FSTextBox>
                    </td>
                    <td style="width: 65px;">
                        <span id="Span3" class="label">名称：</span>
                    </td>
                    <td colspan="2">
                        <cc1:FSTextBox ID="txtName" runat="server" MaxLength="50" Width="210px" CssClass="txtbox_yellow"
                            RequiredType="NotNull"></cc1:FSTextBox>
                        <span style="color: Red">*</span>
                    </td>
                    <td style="width: 150px">
                        <cc1:FSButton ID="btnAdd" runat="server" CssClass="btn" Text="添加" OnClick="btnAdd_Click"
                            OnClientClick="return FSTextBox_SubmitCheck()" />
                        <cc1:FSButton ID="btnDel" runat="server" CssClass="btn" Text="删除" OnClick="btnDel_Click"
                            Enabled="false" OnClientClick="return DelConfirm()" />
                    </td>
                </tr>
            </table>
            <hr class="hrDashed" />
            <table style="width: 900px; margin-top: 5px">
                <tr>
                    <td class="style1">
                        类型：
                    </td>
                    <td style="width: 140px">
                        <cc1:FSDropDownList ID="ddlSorts" runat="server" Width="120px" DataTextField="text"
                            DataValueField="value" AutoPostBack="True" OnSelectedIndexChanged="ddlSorts_SelectedIndexChanged"
                            CssClass="dropdownlist_yellow">
                            <asp:ListItem Value="0">--请选择--</asp:ListItem>
                            <asp:ListItem Value="1">管理程序</asp:ListItem>
                            <asp:ListItem Value="2">部门级管理程序</asp:ListItem>
                            <asp:ListItem Value="3">工作程序</asp:ListItem>
                        </cc1:FSDropDownList>
                    </td>
                    <td style="width: 150px">
                        <span id="Span2" class="label">一级子类：</span>
                    </td>
                    <td style="width: 150px">
                        <cc1:FSDropDownList ID="ddlProgramTypes" runat="server" Width="120px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlProgramTypes_SelectedIndexChanged" CssClass="dropdownlist_yellow">
                            <asp:ListItem Value="0">请选择类型</asp:ListItem>
                        </cc1:FSDropDownList>
                    </td>
                    <td style="width: 150px">
                        <span id="Span5" class="label">二级子类：</span>
                    </td>
                    <td style="width: 120px">
                        <cc1:FSDropDownList ID="ddlProgramSubTypes" runat="server" Width="120px" CssClass="dropdownlist_yellow">
                            <asp:ListItem Value="0">请选择一级子类</asp:ListItem>
                        </cc1:FSDropDownList>
                    </td>
                    <td style="width: 150px">
                        <span id="Span8" class="label">归档状态：</span>
                    </td>
                    <td style="width: 120px">
                        <cc1:FSDropDownList ID="ddlArchiveStatus" runat="server" Width="120px" DataTextField="text"
                            DataValueField="value" CssClass="dropdownlist_yellow">
                            <asp:ListItem Value="0">--请选择--</asp:ListItem>
                            <asp:ListItem Value="1">已归档</asp:ListItem>
                            <asp:ListItem Value="2">未完成</asp:ListItem>
                            <asp:ListItem Value="3">已注销</asp:ListItem>
                        </cc1:FSDropDownList>
                    </td>
                    <td style="width: 120px">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        编码：
                    </td>
                    <td style="width: 140px">
                        <cc1:FSTextBox ID="txtCodes" runat="server" MaxLength="20" Width="120px" CssClass="txtbox_yellow"></cc1:FSTextBox>
                    </td>
                    <td style="width: 60px">
                        <span id="Span6" class="label">版次：</span>
                    </td>
                    <td style="width: 150px">
                        <cc1:FSTextBox ID="txtEditions" runat="server" MaxLength="20" Width="120px" CssClass="txtbox_yellow"></cc1:FSTextBox>
                    </td>
                    <td style="width: 60px">
                        <span id="Span7" class="label">名称：</span>
                    </td>
                    <td style="width: 150px">
                        <cc1:FSTextBox ID="txtNames" runat="server" MaxLength="20" CssClass="txtbox_yellow"
                            Width="120px"></cc1:FSTextBox>
                    </td>
                    <td>
                        申请类型：
                    </td>
                    <td>
                        <cc1:FSDropDownList ID="ddlApplyStyle" runat="server" Width="120px" DataTextField="text"
                            DataValueField="value" CssClass="dropdownlist_yellow">
                            <asp:ListItem Value="0">--请选择--</asp:ListItem>
                            <asp:ListItem Value="1">创建程序</asp:ListItem>
                            <asp:ListItem Value="2">升版程序</asp:ListItem>
                            <asp:ListItem Value="3">注销程序</asp:ListItem>
                        </cc1:FSDropDownList>
                    </td>
                    <td>
                        <cc1:FSButton ID="btnSearch" runat="server" CssClass="btn" Text="查询" OnClick="btnSearch_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <hr class="hrDashed" />
        <div style="margin-top: 5px">
            <div style="color: Red">
                <cc1:FSLabel ID="lblRecord" runat="server" Visible="false"></cc1:FSLabel></div>
            <cc1:FSGridView ID="gvProgramFileList" runat="server" AutoGenerateColumns="False"
                Width="990px" AutoGenerateSelectButton="True" DataKeyNames="ID" AllowPaging="True"
                OnSelectedIndexChanged="gvProgramFileList_SelectedIndexChanged" CssClass="table_gv"
                OnRowDataBound="gvProgramFileList_RowDataBound" OnExteriorPaging="gvProgramFileList_ExteriorPaging"
                OnRowUpdating="gvProgramFileList_RowUpdating" OnRowDeleting="gvProgramFileList_RowDeleting">
                <Columns>
                    <asp:BoundField HeaderText="编码" DataField="Code">
                        <HeaderStyle CssClass="th_j03" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="版次" DataField="Edition" HtmlEncode="False">
                        <HeaderStyle CssClass="th_j01" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FileName" HeaderText="名称">
                        <HeaderStyle CssClass="th_j03" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="二级子类" DataField="SubTypeName">
                        <HeaderStyle CssClass="th_j02" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TypeName" HeaderText="一级子类">
                        <HeaderStyle CssClass="th_j02" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Type" HeaderText="类型">
                        <HeaderStyle CssClass="th_j02" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ArchiveState" HeaderText="归档状态">
                        <HeaderStyle CssClass="th_j001" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ApplyStyle" HeaderText="申请类型">
                        <HeaderStyle CssClass="th_j001" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="ID" DataField="ID">
                        <HeaderStyle CssClass="hidden" />
                        <ItemStyle CssClass="hidden" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Files" DataField="Files">
                        <HeaderStyle CssClass="hidden" />
                        <ItemStyle CssClass="hidden" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="ProcessID" DataField="ProcessID">
                        <HeaderStyle CssClass="hidden" />
                        <ItemStyle CssClass="hidden" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="ProTypId" DataField="ProTypId">
                        <HeaderStyle CssClass="hidden" />
                        <ItemStyle CssClass="hidden" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="ProTypSubId" DataField="ProTypSubId">
                        <HeaderStyle CssClass="hidden" />
                        <ItemStyle CssClass="hidden" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="操作" ItemStyle-Width="60px" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <cc1:FSLinkButton ID="lnkbtnRestart" runat="server" CommandName="Update" Visible="false">重新发起</cc1:FSLinkButton>
                            <cc1:FSLinkButton ID="lnkbtnCancel" runat="server" CommandName="Delete" Visible="false"
                                OnClientClick="return confirm('是否真的撤销？');">撤销流程</cc1:FSLinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle HorizontalAlign="Center" />
                <HeaderStyle CssClass="gv_headStyle" />
                <SelectedRowStyle BackColor="#B3D0F5" ForeColor="Black" Width="40px" />
            </cc1:FSGridView>
            <cc1:FSHiddenField ID="hfSelectedIndex" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
