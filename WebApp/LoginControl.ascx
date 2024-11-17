<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginControl.ascx.cs" Inherits="TryItWebApplication.LoginControl" %>

<div>
    <h2>Login</h2>
    <label for="txtLoginUserId">User ID (user1 or user2 or user3):</label><br />
    <asp:TextBox ID="txtLoginUserId" runat="server" Width="300px" Text="user1"></asp:TextBox><br />

    <label for="txtPassword">Password (pass1 or pass2 or pass3):</label><br />
    <asp:TextBox ID="txtPassword" runat="server" Width="300px" Text="pass1" TextMode="Password"></asp:TextBox><br />

    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" /><br />

    <!-- Logout Button -->
    <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" /><br />

    <asp:Label ID="lblLoginMessage" runat="server" ForeColor="Red"></asp:Label>
</div>
<hr />
