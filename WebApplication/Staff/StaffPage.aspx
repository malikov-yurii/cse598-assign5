<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="StaffPage.aspx.cs" Inherits="WebApplication.Staff" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <h1>Staff Page</h1>
        <h2>List of Members</h2>
        <asp:Literal ID="MembersTableLiteral" runat="server"></asp:Literal>

    </main>
</asp:Content>