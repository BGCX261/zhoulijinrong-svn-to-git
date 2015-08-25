<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Step.ascx.cs" Inherits="FS.ADIM.OA.WebUI.WorkFlow.Receive.ReDetail.UC_Step" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register src="../../../PageWF/UC_FileControl.ascx" tagname="FileControlUC" tagprefix="uc" %>
<div class="divSection" style="width: 80%; margin-left: 36px;">
    <div class="divTitle" style="margin: 0px; background: url(../Img/i_left.gif) no-repeat 2px 2px;
        background-color: #C8D0D4; padding-left: 25px;">
        步骤 -
        <%=m_strStepName%>
    </div>
    <div>
        <table class="table_add_mid offsetRight" cellpadding="2">
            <tr>
                <td style="width: 75px">
                    <span class="label">参与人员：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtUser" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 140px"></cc1:FSTextBox>
                </td>
                <td style="width: 75px">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="label">提交动作：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtAction" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 140px"></cc1:FSTextBox>
                </td>
                <td>
                    <span class="label">提交时间：</span>
                </td>
                <td>
                    <cc1:FSTextBox ID="txtDateTime" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 141px"></cc1:FSTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="label">意见：</span>
                </td>
                <td colspan="3">
                    <cc1:FSTextBox ID="txtComment" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 400px" Width="495px"></cc1:FSTextBox>
                </td>
            </tr>
            <tr id="trprompt" runat="server">
                <td>
                    <span class="label">提示信息：</span>
                </td>
                <td colspan="3">
                    <cc1:FSTextBox ID="txtPrompt" runat="server" CssClass="txtPreview" ReadOnly="True"
                        TabIndex="-1" Style="width: 400px" Width="495px"></cc1:FSTextBox>
                </td>
            </tr>
            <tr id="tdAttach" runat="server" visible="false">
                <td>
                    <span class="label">附件：</span>
                </td>
                <td colspan="3" style="width: 600px;">
                    <uc:FileControlUC ID="ucAttachment" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</div>
