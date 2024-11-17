<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="TryItWebForm.aspx.cs" Inherits="TryItWebApplication.TryItWebForm" ValidateRequest="false" %>
<%@ Register TagPrefix="uc" TagName="LoginControl" Src="~/LoginControl.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Development Investment Evaluation AI Assistant</title>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">

        <!-- Include the Login User Control -->
        <uc:LoginControl ID="LoginControl" runat="server" OnLoginStatusChanged="LoginControl_LoginStatusChanged" />

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
