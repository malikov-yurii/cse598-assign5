<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="TryItWebForm.aspx.cs" Inherits="TryItWebApplication.TryItWebForm" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Development Investment Evaluation AI Assistant</title>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">

        <!-- Login Widget -->
        <div>
            <h2>Login</h2>
            <label for="txtLoginUserId">User ID (user1 or user2 or user3):</label><br />
            <asp:TextBox ID="txtLoginUserId" runat="server" Width="300px" Text="user1"></asp:TextBox><br />

            <label for="txtPassword">Password (pass1 or pass2 or pass3):</label><br />
            <asp:TextBox ID="txtPassword" runat="server" Width="300px" Text="testpassword" TextMode="Password"></asp:TextBox><br />

            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" /><br />

            <!-- Logout Button -->
            <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" /><br />

            <asp:Label ID="lblLoginMessage" runat="server" ForeColor="Red"></asp:Label>
        </div>
        <hr />

        <!-- Prompts Left Section -->
        <div>
            <h3>Prompts Left for User Today:</h3>
            <asp:Label ID="lblPromptsLeft" runat="server" Text="Please log in." Font-Bold="true"></asp:Label>
        </div>
        <br />

        <!-- ChatGPT Section -->
        <div>
            <h2>Ask ChatGPT to Evaluate Development Investment Attractiveness</h2>

            <!-- Latitude and Longitude Inputs -->
            <label for="txtLatitude">Enter Latitude:</label><br />
            <asp:TextBox ID="txtLatitude" runat="server" Width="300px" Text="50.4504"></asp:TextBox><br />

            <label for="txtLongitude">Enter Longitude:</label><br />
            <asp:TextBox ID="txtLongitude" runat="server" Width="300px" Text="30.5245"></asp:TextBox><br />

            <!-- Button to ask ChatGPT -->
            <asp:Button ID="btnAskChatGPT" runat="server" Text="Ask ChatGPT" OnClick="btnAskChatGPT_Click" /><br />
        </div>

        <!-- Response Section -->
        <div>
            <h3>ChatGPT Response:</h3>
            <asp:TextBox ID="txtGptResult" runat="server" TextMode="MultiLine" Width="800px" Height="200px" ReadOnly="true"></asp:TextBox>
        </div>

        <!-- Chat History Section -->
        <div>
            <h2>Get Chat History</h2>
            <!-- Button to get Chat History -->
            <asp:Button ID="btnGetChatHistory" runat="server" Text="Get Chat History" OnClick="btnGetChatHistory_Click" /><br />
        </div>

        <!-- Chat History Result Section -->
        <div>
            <h3>Chat History:</h3>
            <asp:TextBox ID="txtChatHistory" runat="server" TextMode="MultiLine" Width="800px" Height="300px" ReadOnly="true"></asp:TextBox>
        </div>

    </form>
</body>
</html>
