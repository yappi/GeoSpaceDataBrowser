<%@ Control ClassName="DataViewControl" Language="C#" AutoEventWireup="true" CodeBehind="DataViewControl.ascx.cs"
  Inherits="GeospaceDataBrowser.Web.Controls.DataViewControl" %>
<asp:UpdatePanel runat="server" ID="DataViewPanel">
  <ContentTemplate>
    <asp:Panel ID="DataPlotPanel" runat="server" CssClass="dataPlotPanel">
      <div class="controlButtons" style="float: left;">
        <ul>
          <li>
            <asp:Button ID="NextButton" Text="<%$ Resources:LocalizedText, NextButton%>" runat="server"
              CssClass="applyButton ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
              OnClick="NextButton_Click"></asp:Button>
          </li>
          <li>
            <asp:Button ID="PreviousButton" Text="<%$ Resources:LocalizedText, PreviousButton%>"
              runat="server" CssClass="applyButton ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
              OnClick="PreviousButton_Click"></asp:Button>
          </li>
        </ul>
      </div>
      <div class="controlButtons" style="float: right;">
        <ul>
          <li>
            <asp:Button ID="AddButton" Text="<%$ Resources:LocalizedText, AddButton%>" runat="server"
              CssClass="applyButton ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
              OnClick="AddButton_Click"></asp:Button>
          </li>
          <li>
            <asp:Button ID="RemoveButton" Text="<%$ Resources:LocalizedText, RemoveButton%>"
              runat="server" CssClass="applyButton ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
              OnClick="RemoveButton_Click"></asp:Button>
          </li>
        </ul>
      </div>
      <h1 class="plotTitle">
        <asp:Literal ID="DataTitleLiteral" runat="server"></asp:Literal>
      </h1>
      <asp:Image ID="DataPlot" runat="server" />
    </asp:Panel>
    <asp:UpdatePanel runat="server" ID="ControlUpdatePanel">
      <ContentTemplate>
        <asp:Panel ID="PlaceHolder" runat="server" CssClass="controlPanel">
          <div class="ui_accordion">
            <h3>
              Data properties
            </h3>
            <table>
              <tr>
                <td class="controlColumn">
                  <table>
                    <tr>
                      <td class="controlLabel">
                        <asp:Label ID="ObservatoryLabel" runat="server" Text="<%$ Resources:LocalizedText, ObservatoryLabel%>"></asp:Label>
                      </td>
                      <td>
                        <asp:DropDownList ID="ObservatoryDropDown" AutoPostBack="true" OnSelectedIndexChanged="ObservatoryDropDown_SelectedIndexChanged"
                          runat="server">
                        </asp:DropDownList>
                      </td>
                    </tr>
                    <tr>
                      <td class="controlLabel">
                        <asp:Label ID="InstrumentLabel" runat="server" Text="<%$ Resources:LocalizedText, InstrumentLabel%>"></asp:Label>
                      </td>
                      <td>
                        <asp:DropDownList ID="InstrumentDropDown" AutoPostBack="true" OnSelectedIndexChanged="InstrumentDropDown_SelectedIndexChanged"
                          runat="server">
                        </asp:DropDownList>
                      </td>
                    </tr>
                    <tr>
                      <td class="controlLabel">
                        <asp:Label ID="DataTypeLabel" runat="server" Text="<%$ Resources:LocalizedText, DataTypeLabel%>"></asp:Label>
                      </td>
                      <td>
                        <asp:DropDownList ID="DataTypeDropDown" AutoPostBack="true" runat="server">
                        </asp:DropDownList>
                      </td>
                    </tr>
                  </table>
                </td>
                <td class="controlColumn">
                  <table>
                    <tr>
                      <td class="controlLabel">
                        <asp:Label ID="DateLabel" runat="server" Text="<%$ Resources:LocalizedText, DateLabel%>"></asp:Label>
                        <asp:Label ID="FromDateLabel" runat="server" Text="<%$ Resources:LocalizedText, FromDateLabel%>"></asp:Label>
                      </td>
                      <td>
                        <asp:TextBox ID="DateTimeText" runat="server" CssClass="ui_datetime"></asp:TextBox>
                        <asp:TextBox ID="FromDateText" runat="server" CssClass="ui_datetime"></asp:TextBox>
                        <asp:HiddenField ID="JsDateTimeFormat" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="NetDateTimeFormat" runat="server"></asp:HiddenField>
                      </td>
                    </tr>
                    <tr>
                      <td class="controlLabel">
                        <asp:Label ID="ToDateLabel" runat="server" Text="<%$ Resources:LocalizedText, ToDateLabel%>"></asp:Label>
                      </td>
                      <td>
                        <asp:TextBox ID="ToDateText" runat="server" CssClass="ui_datetime"></asp:TextBox>
                      </td>
                    </tr>
                  </table>
                </td>
                <td class="controlColumn">
                  <table>
                    <tr>
                      <td class="controlLabel">
                        <asp:Label ID="YRangeFromLabel" runat="server" Text="<%$ Resources:LocalizedText, YRangeFromLabel%>"></asp:Label>
                      </td>
                      <td>
                        <asp:TextBox ID="YRangeFromText" runat="server" CssClass="ui-widget-content ui-corner-all"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td class="controlLabel">
                        <asp:Label ID="YRangeToLabel" runat="server" Text="<%$ Resources:LocalizedText, YRangeToLabel%>"></asp:Label>
                      </td>
                      <td>
                        <asp:TextBox ID="YRangeToText" runat="server" CssClass="ui-widget-content ui-corner-all"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td class="commandControls" colspan="2">
                        <asp:Button ID="ApplyButton" Text="<%$ Resources:LocalizedText, ApplyButton%>" runat="server"
                          CssClass="applyButton ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
                          OnClick="ApplyButton_Click"></asp:Button>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
            <asp:HiddenField ID="AccordionState" runat="server" Value="0" />
          </div>
        </asp:Panel>
        <script type="text/javascript">
            function InitControls(parentControlId) {
              $('.ui_accordion').each(function (index) {

                // Read accordion state from hidden control.
                var accordionState = parseInt($(this).children('input:hidden').val());
                $(this).accordion({
                  collapsible: true,
                  active: accordionState,
                  heightStyle: 'content',
                  activate: function (event, ui) {

                    // Save accordion state to hidden control.
                    var index = $(this).children('h3').index(ui.newHeader);
                    $(this).children('input:hidden').val(index);
                  }
                })
              });

              $('select').selectmenu({
                style: 'dropdown'
              });

              $('.ui_datetime').each(function () {
                  var dateTimeControl = $('#' + this.id);
                  var formatString = dateTimeControl.next('input[type=hidden]').val();
                  dateTimeControl.addClass('ui-corner-all').addClass('ui-widget-content').AnyTime_picker({
                      format: formatString
                  });

                  var dateTimeConverter = new AnyTime.Converter({
                    format: formatString,
                  });

                  var dateTime;
                  try {
                  dateTime = new AnyTime.Converter({
                      format: '<%= Resources.LocalizedText.JsStringDateTimeFormat %>',
                      utcFormatOffsetImposed: 0
                  }).parse(this.value);
                  }
                  catch (e) {
                  try {
                      dateTime = new AnyTime.Converter({
                      format: '<%= Resources.LocalizedText.JsStringDateFormat %>',
                      utcFormatOffsetImposed: 0
                      }).parse(this.value);
                  }
                  catch (e) {
                      dateTime = Date.now();
                  }
                  }

                  this.value = dateTimeConverter.format(dateTime);
              });
            }

            function UninitControls(parentControlId) {
              $('.ui_datetime').AnyTime_noPicker();
            }
        </script>
      </ContentTemplate>
    </asp:UpdatePanel>
  </ContentTemplate>
</asp:UpdatePanel>