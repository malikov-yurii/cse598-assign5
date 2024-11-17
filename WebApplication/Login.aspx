<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Login.aspx.cs" Inherits="WebApplication.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/Login.css" rel="stylesheet" type="text/css" />
    <main>
        <h1>Login Page</h1>
        <div class="form-group">
            <label for="username">Username:</label>
            <asp:TextBox ID="txtUsername" runat="server" />
        </div>
        <div class="form-group">
            <label for="password">Password:</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
        </div>
        <%-- Actions to Log in or to register  --%>
        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
        <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" />
        <p><asp:Label ID="lblLoginResult" runat="server" Text="" /></p>
        <br />
    </main>
</asp:Content>
