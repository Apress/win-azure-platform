<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ACSWebApp._Default" ValidateRequest="false" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Identity provider: <asp:Label ID="lblIdP" runat="server" Text=""></asp:Label>
    </h2>
    <h2>
        Claims:
    </h2>
    <p>
        <asp:Label ID="lblClaims" runat="server" Text=""></asp:Label>
    </p>
    <p>
        <asp:HyperLink ID="lnkAdmin" runat="server" NavigateUrl="~/Admin.aspx" Visible="False">Go to Admin Page</asp:HyperLink>
    </p>
    

</asp:Content>


