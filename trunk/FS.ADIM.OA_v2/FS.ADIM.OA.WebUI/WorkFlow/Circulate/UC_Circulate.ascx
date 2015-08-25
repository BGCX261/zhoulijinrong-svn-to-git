<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Circulate.ascx.cs"
    Inherits="FS.ADIM.OA.WebUI.WorkFlow.Circulate.UC_Circulate" %>
<%@ Register Assembly="FounderSoftware.Framework.UI.WebCtrls" Namespace="FounderSoftware.Framework.UI.WebCtrls"
    TagPrefix="cc1" %>
<%@ Register Src="UC_GoOnCirculate.ascx" TagName="UC_GoOnCirculate" TagPrefix="uc" %>
<%@ Register Src="../../WorkflowMenu/Circulate/UC_CirculateList.ascx" TagName="UCCirculateList"
    TagPrefix="uc" %>
<asp:PlaceHolder ID="phlContent" runat="server"></asp:PlaceHolder>
<div class="divCenter">
    <div id="divCirculate" style="border: 1px solid black; width: 720px;padding:5px 0px;">
        <uc:UC_GoOnCirculate ID="ucGoOnCirculate" runat="server" />
        <cc1:FSButton ID="btnRead" Text="阅知" runat="server" class="btn" OnClick="btnRead_Click" />
        <cc1:FSButton ID="btnGoOnCirculate" Text="继续传阅" runat="server" class="btn" Visible="false"
            OnClick="btnGoOnCirculate_Click" />
        <uc:UCCirculateList ID="ucCirculateList" runat="server" />
    </div>
</div>

<script type="text/javascript">
    var Circulate = $("divCirculate");
    var Print = $('btnP');
    if (Circulate != null && Print != null) {
        Circulate.insertAdjacentElement("beforeEnd", Print);
    }
</script>

