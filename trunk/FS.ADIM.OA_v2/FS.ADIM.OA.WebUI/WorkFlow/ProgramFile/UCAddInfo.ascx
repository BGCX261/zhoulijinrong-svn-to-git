<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCAddInfo.ascx.cs" Inherits="FS.ADIM.OA.WebUI.WorkFlow.ProgramFile.UCAddInfo" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>

<script language="javascript" type="text/javascript">
    window.onload = function SetConfirmBtnStauts() {
        var btnConfirm = $("btnConfirm");
        var isHistory = '<%=Request.QueryString["isHistory"] %>';
        if (isHistory == false) {
            btnConfirm.disabled = true;
        }
    }

    function Confirm() {
        if (FSTextBox_SubmitCheck()) {
            var comment = $('<%=txtComment.ClientID%>').value;

            if (comment.trim().length > 300) {
                alert('落实情况不能超过300个字符。');
                return false;
            } else {
                window.returnValue = comment;
                self.close()
            }
        }
    }
</script>

<table cellpadding="0" cellspacing="3" style="width: 600px">
    <tr>
        <td style="padding-left: 20px; width: 40px;">
            内容：
        </td>
        <td>
            <cc1:FSTextBox ID="txtComment" runat="server" Width="300px" RequiredType="NotNull"
                Height="105px" TextMode="MultiLine"></cc1:FSTextBox>
        </td>
    </tr>
    <tr>
        <td class="td_left">
        </td>
        <td>
            <div style="padding-left: 165px">
                <input id="btnConfirm" type="button" onclick="Confirm();" class="btn" value="确定" />
                <input type="button" onclick="self.close();" class="btn" value="关闭" />
            </div>
        </td>
    </tr>
</table>
<cc1:FSHiddenField ID="hfucID" runat="server" />
