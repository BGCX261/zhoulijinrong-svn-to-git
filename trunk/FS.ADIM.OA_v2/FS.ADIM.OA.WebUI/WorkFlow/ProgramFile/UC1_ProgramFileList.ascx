<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC1_ProgramFileList.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.ProgramFile.UC1_ProgramFileList" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="false"
    EnableScriptLocalization="false">
</asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div id="wapper">
            <div id="tab">
                <table>
                    <tr>
                        <td>
                            <div id="tab_create" runat="server">
                                <cc1:FSLinkButton ID="lnkbtnCreateProgram" runat="server" Text="创建程序" OnClick="lnkbtnCreateProgram_Click"></cc1:FSLinkButton>
                            </div>
                        </td>
                        <td>
                            <div id="tab_update" runat="server">
                                <cc1:FSLinkButton ID="lnkbtnUpdateProgram" runat="server" Text="升版/注销程序" OnClick="lnkbtnUpdateProgram_Click"></cc1:FSLinkButton>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="main">
                <asp:MultiView ID="MultiView" runat="server">
                    <asp:View ID="vewCreate" runat="server">
                        <div id="v1">
                            <div class="tab-page" id="tabPage4">
                                <div style="padding-left: 20%">
                                    <table class="table_add_mid">
                                        <tr>
                                            <td style="width: 79px">
                                                <span id="Span76" class="label">程序编码：</span>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtCode" runat="server" CssClass="txtbox_blue" ReadOnly="True"
                                                    Width="140px"></cc1:FSTextBox>
                                            </td>
                                            <td style="width: 69px">
                                                <span id="Span77" class="label">版本：<cc1:FSLabel ID="lblEdition" runat="server" Text="1"
                                                    Visible="false"></cc1:FSLabel></span>
                                            </td>
                                            <td>
                                                <table style="width: 100%; border: 0px">
                                                    <tr>
                                                        <td style="width: 31%;">
                                                            主办部门：
                                                        </td>
                                                        <td>
                                                            <cc1:FSDropDownList ID="ddlDept" runat="server" CssClass="dropdownlist_yellow" Width="180px">
                                                            </cc1:FSDropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <table style="width: 100%" class="table_add_mid">
                                                    <tr>
                                                        <td style="width: 78px">
                                                            <span id="Span11" class="label">程序名称：</span>
                                                        </td>
                                                        <td>
                                                            <cc1:FSTextBox ID="txtName" runat="server" CssClass="txtbox_blue" MaxLength="40"
                                                                ReadOnly="True" Width="355px"></cc1:FSTextBox>
                                                        </td>                                                        
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="Span4" class="label">程序类型：</span>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtType" runat="server" Width="140px" CssClass="txtbox_blue" ReadOnly="True"></cc1:FSTextBox>
                                            </td>
                                            <td style="width: 69px">
                                                <span id="Span5" class="label">程序子类：</span>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtSubType" runat="server" Width="140px" CssClass="txtbox_blue"
                                                    ReadOnly="True"></cc1:FSTextBox>
                                                <cc1:FSButton ID="btnConfirm" runat="server" CssClass="btn" Text="提交" Enabled="false"
                                                    OnClick="btnConfirm_Click" OnClientClick="return FSTextBox_SubmitCheck()" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <table class="table_add_mid" align="center">
                                        <tr>
                                            <td style="width: 69px; padding-left: 10px">
                                                <span id="Span17" class="label">程序分类：</span>
                                            </td>
                                            <td>
                                                <cc1:FSDropDownList ID="ddlSort" runat="server" Width="120px" AutoPostBack="True"
                                                    DataTextField="text" DataValueField="value" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged"
                                                    CssClass="dropdownlist_yellow">
                                                    <asp:ListItem Value="0">--请选择--</asp:ListItem>
                                                    <asp:ListItem Value="1">管理程序</asp:ListItem>
                                                    <asp:ListItem Value="2">部门级管理程序</asp:ListItem>
                                                    <asp:ListItem Value="3">工作程序</asp:ListItem>
                                                </cc1:FSDropDownList>
                                            </td>
                                            <td style="width: 69px; padding-left: 10px">
                                                <span id="Span1" class="label">程序类型：</span>
                                            </td>
                                            <td>
                                                <cc1:FSDropDownList ID="ddlProgramType" runat="server" Width="120px" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlProgramType_SelectedIndexChanged" CssClass="dropdownlist_yellow">
                                                    <asp:ListItem Value="0">请选择分类</asp:ListItem>
                                                </cc1:FSDropDownList>
                                            </td>
                                            <td style="width: 69px; padding-left: 10px">
                                                <span id="Span2" class="label">程序子类：</span>
                                            </td>
                                            <td>
                                                <cc1:FSDropDownList ID="ddlProgramSubType" runat="server" Width="120px" CssClass="dropdownlist_yellow">
                                                    <asp:ListItem Value="0">请选择类型</asp:ListItem>
                                                </cc1:FSDropDownList>
                                            </td>
                                            <td>
                                                <cc1:FSButton ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" CssClass="btn" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="height: 20px">
                                    <hr class="hrDashed" />
                                </div>
                                <div style="width: 100%">
                                    <cc1:FSGridView ID="gvProgramFileList" runat="server" AutoGenerateColumns="False"
                                        AutoGenerateSelectButton="True" DataKeyNames="ID" AllowPaging="True" ShowRadioButton="false"
                                        OnSelectedIndexChanged="gvProgramFileList_SelectedIndexChanged" OnRowDataBound="gvProgramFileList_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="Code" HeaderText="程序编码" HtmlEncode="False" HeaderStyle-Width="100px">
                                                <ItemStyle CssClass="td_j02" />
                                                <HeaderStyle CssClass="th_j02" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FileName" HeaderText="程序名称" HtmlEncode="False">
                                                <ItemStyle CssClass="td_j06" />
                                                <HeaderStyle CssClass="th_j06" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Type" HeaderText="程序分类" HtmlEncode="False" HeaderStyle-Width="150px"
                                                ItemStyle-Width="150px">
                                                <ItemStyle CssClass="td_j02" />
                                                <HeaderStyle CssClass="th_j02" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TypeName" HeaderText="程序类型" HtmlEncode="False">
                                                <ItemStyle CssClass="td_j02" />
                                                <HeaderStyle CssClass="th_j02" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SubTypeName" HeaderText="程序子类" HtmlEncode="False">
                                                <ItemStyle CssClass="td_j02" />
                                                <HeaderStyle CssClass="th_j02" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ProTypId" HeaderText="ProTypId" />
                                            <asp:BoundField DataField="ProTypSubId" HeaderText="ProTypSubId" />
                                            <asp:BoundField DataField="ID" HeaderText="ID" />
                                        </Columns>
                                        <HeaderStyle CssClass="gv_headStyle" />
                                        <SelectedRowStyle BackColor="#B3D0F5" Font-Bold="True" ForeColor="Black" Width="40px" />
                                        <RowStyle HorizontalAlign="Center" CssClass="a" />
                                    </cc1:FSGridView>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vewUpdate" runat="server">
                        <div id="v2">
                            <div class="tab-page" style="display: block">
                                <div style="padding-left: 5%">
                                    <table class="table_add_mid" align="center">
                                        <tr>
                                            <td style="width: 69px">
                                                <span id="Span3" class="label">程序编码：</span>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtCodes" runat="server" CssClass="txtbox_blue" Width="138px"
                                                    ReadOnly="True"></cc1:FSTextBox>
                                            </td>
                                            <td style="width: 69px">
                                                <span id="Span6" class="label">版本：
                                                    <cc1:FSLabel ID="lblEditions" runat="server"></cc1:FSLabel></span>
                                            </td>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 31%;">
                                                            主办部门：
                                                        </td>
                                                        <td>
                                                            <cc1:FSDropDownList ID="ddlDept2" runat="server" CssClass="dropdownlist_yellow" Width="180px">
                                                            </cc1:FSDropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <table style="width: 100%" class="table_add_mid">
                                                    <tr>
                                                        <td style="width: 69px">
                                                            <span id="Span7" class="label">程序名称：</span>
                                                        </td>
                                                        <td>
                                                            <cc1:FSTextBox ID="txtNames" runat="server" CssClass="txtbox_blue" ReadOnly="True"
                                                                Width="281px"></cc1:FSTextBox>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdolstStyle" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rdolstStyle_SelectedIndexChanged"
                                                                RepeatDirection="Horizontal">
                                                                <asp:ListItem Selected="True">升版</asp:ListItem>
                                                                <asp:ListItem>注销</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 69px">
                                                <span id="Span8" class="label">程序类型：</span>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtTypes" runat="server" CssClass="txtbox_blue" Width="140px"
                                                    ReadOnly="True"></cc1:FSTextBox>
                                            </td>
                                            <td style="width: 69px">
                                                <span id="Span9" class="label">程序子类：</span>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtSubTypes" runat="server" CssClass="txtbox_blue" Width="140px"
                                                    ReadOnly="True"></cc1:FSTextBox>
                                                <cc1:FSButton ID="btnConfirms" runat="server" CssClass="btn" Enabled="false" OnClientClick="return FSTextBox_SubmitCheck()"
                                                    Text="提交" OnClick="btnConfirms_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <table class="table_add_mid" align="center">
                                        <tr>
                                            <td style="width: 69px">
                                                <span id="Span10" class="label">程序编码：</span>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtCodess" runat="server" CssClass="txtbox_yellow" Width="140px"></cc1:FSTextBox>
                                            </td>
                                            <td style="width: 69px">
                                                <span id="Span12" class="label">程序名称：</span>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtNamess" runat="server" CssClass="txtbox_yellow" Width="140px"></cc1:FSTextBox>
                                                &nbsp;
                                            </td>
                                            <td style="width: 69px">
                                                <span id="Span13" class="label">程序类型：</span>
                                            </td>
                                            <td>
                                                <cc1:FSDropDownList ID="ddlProgramTypes" runat="server" Width="120px" CssClass="dropdownlist_yellow">
                                                </cc1:FSDropDownList>
                                                <cc1:FSButton ID="btnSearchs" runat="server" CssClass="btn" OnClick="btnSearchs_Click"
                                                    Text="查询" />
                                        </tr>
                                        <tr>
                                            <td style="width: 69px">
                                                <span id="Span14" class="label">编写人：</span>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtWriter" runat="server" CssClass="txtbox_yellow" Width="140px"></cc1:FSTextBox>
                                            </td>
                                            <td style="width: 69px">
                                                <span id="Span15" class="label">批准人：</span>
                                            </td>
                                            <td>
                                                <cc1:FSTextBox ID="txtAuthorized" runat="server" CssClass="txtbox_yellow" Width="140px"></cc1:FSTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="height: 20px">
                                    <hr class="hrDashed" />
                                </div>
                                <div style="width: 100%">
                                    <cc1:FSGridView ID="gvProgramFilesList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                        AutoGenerateSelectButton="True" CheckTemplateHeaderText="选择" CssClass="table_gv"
                                        DataKeyNames="ID" OnRowDataBound="gvProgramFilesList_RowDataBound" OnSelectedIndexChanged="gvProgramFilesList_SelectedIndexChanged"
                                        OnExteriorPaging="gvProgramFilesList_ExteriorPaging">
                                        <Columns>
                                            <asp:BoundField DataField="Code" HeaderText="程序编码" HtmlEncode="False" HeaderStyle-Width="100px">
                                                <ItemStyle CssClass="td_j02" />
                                                <HeaderStyle CssClass="th_j02" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FileName" HeaderText="程序名称" HtmlEncode="False" HeaderStyle-Width="180px"
                                                ItemStyle-Width="180px">
                                                <ItemStyle CssClass="td_j02" />
                                                <HeaderStyle CssClass="th_j02" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TypeName" HeaderText="程序类型" HtmlEncode="False" HeaderStyle-Width="150px"
                                                ItemStyle-Width="150px">
                                                <ItemStyle CssClass="td_j02" />
                                                <HeaderStyle CssClass="th_j02" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SubTypeName" HeaderText="程序子类" HtmlEncode="False" HeaderStyle-Width="80px"
                                                ItemStyle-Width="80px">
                                                <ItemStyle CssClass="td_j02" />
                                                <HeaderStyle CssClass="th_j02" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="WriteName" HeaderText="编写人" HtmlEncode="False" HeaderStyle-Width="50px">
                                                <ItemStyle CssClass="td_j02" />
                                                <HeaderStyle CssClass="th_j02" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Authorized" HeaderText="批准人" HtmlEncode="False" HeaderStyle-Width="50px">
                                                <ItemStyle CssClass="td_j02" />
                                                <HeaderStyle CssClass="th_j02" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Edition" HeaderText="版次" HtmlEncode="False" HeaderStyle-Width="30px">
                                                <ItemStyle CssClass="td_j02" />
                                                <HeaderStyle CssClass="th_j02" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ActivationDate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="生效日期"
                                                HtmlEncode="False">
                                                <ItemStyle CssClass="td_j02" />
                                                <HeaderStyle CssClass="th_j02" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Type" HeaderText="Type">
                                                <ItemStyle CssClass="td_j02" />
                                                <HeaderStyle CssClass="th_j02" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ProTypId" HeaderText="ProTypId">
                                                <ItemStyle CssClass="td_j02" />
                                                <HeaderStyle CssClass="th_j02" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ProTypSubId" HeaderText="ProTypSubId">
                                                <ItemStyle CssClass="td_j02" />
                                                <HeaderStyle CssClass="th_j02" />
                                            </asp:BoundField>
                                        </Columns>
                                        <SelectedRowStyle BackColor="#B3D0F5" Font-Bold="True" ForeColor="Black" />
                                    </cc1:FSGridView>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
                <cc1:FSHiddenField ID="hfSort" runat="server" />
                <cc1:FSHiddenField ID="hfSorts" runat="server" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
