<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Login.aspx.cs" Inherits="WebApplication.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <main>
        <h1>Login Page</h1>
        <div class="form-group row">
            <div class="col-3">
                <label for="txtUsername">Username:</label>
                <asp:TextBox class="form-control" ID="txtUsername" runat="server" />
            </div>
        </div>
        <div class="form-group row">
            <div class="col-3">
                <label for="txtPassword">Password:</label>
                <asp:TextBox class="form-control" ID="txtPassword" runat="server" TextMode="Password" />
             </div>
        </div>

        <div class="mt-3">
            <%-- Actions to Log in or to register  --%>
            <asp:Button class="btn btn-secondary" ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />

            <%-- Register  --%>
            <asp:Button class="btn btn-light" ID="btnRegister" runat="server" Text="Register as a Member" OnClick="btnRegister_Click" />
            <p><asp:Label ID="lblLoginResult" runat="server" Text="" /></p>
        </div>
        <br />
    </main>
</asp:Content>
