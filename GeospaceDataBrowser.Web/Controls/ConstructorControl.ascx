<%@ Control ClassName="ConstructorControl" Language="C#" AutoEventWireup="true" CodeBehind="ConstructorControl.ascx.cs"
  Inherits="GeospaceDataBrowser.Web.Controls.ConstructorControl" %>
<%@ Reference Control="DataViewControl.ascx" %>
<asp:ScriptManager ID="ScriptManager" runat="server">
</asp:ScriptManager>
<asp:UpdatePanel runat="server" ID="ConstructorUpdatePanel">
  <ContentTemplate>
    <asp:PlaceHolder ID="PlaceHolder" runat="server"></asp:PlaceHolder>
  </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress ID="UpdateProgress" DisplayAfter="0" runat="server">
  <ProgressTemplate>
    <div id="blur">
      <div id="progress">
        <div id="spin" />
      </div>
    </div>
  </ProgressTemplate>
</asp:UpdateProgress>
<script type="text/javascript">
  var opts = {
    lines: 9, // The number of lines to draw
    length: 18, // The length of each line
    width: 8, // The line thickness
    radius: 18, // The radius of the inner circle
    corners: 1, // Corner roundness (0..1)
    rotate: 0, // The rotation offset
    color: '#000', // #rgb or #rrggbb
    speed: 1, // Rounds per second
    trail: 60, // Afterglow percentage
    shadow: false, // Whether to render a shadow
    hwaccel: false, // Whether to use hardware acceleration
    className: 'spinner', // The CSS class to assign to the spinner
    zIndex: 2e9, // The z-index (defaults to 2000000000)
    top: 'auto', // Top position relative to parent in px
    left: 'auto' // Left position relative to parent in px
  };
  var target = document.getElementById('spin');
  var spinner = new Spinner(opts).spin(target);
</script>