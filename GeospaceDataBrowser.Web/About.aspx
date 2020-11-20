<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
  CodeBehind="About.aspx.cs" Inherits="GeospaceDataBrowser.Web.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
  <h2>
    <asp:Literal runat="server" Text="<%$ Resources:LocalizedText, AboutTitle%>"></asp:Literal>
  </h2>
  <p>
    <asp:Literal runat="server" ID="AboutMessage"></asp:Literal>
  </p>
</asp:Content>