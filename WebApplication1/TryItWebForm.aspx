<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="TryItWebForm.aspx.cs" Inherits="TryItWebApplication.TryItWebForm" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Word Count and Conversions</title>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">

        <!-- ChatGPT Section -->
        <div>
            <h2>Ask ChatGPT a Question. You can add a website urls for question context</h2>
            <label for="txtPrompt">Enter Prompt:</label><br />
            <asp:TextBox ID="txtPrompt" runat="server" Width="300px"></asp:TextBox><br />

            <!-- URL Inputs -->
            <label for="txtUrl1">Enter URL 1:</label><br />
            <asp:TextBox ID="txtUrl1" runat="server" Width="300px"></asp:TextBox><br />
    
            <label for="txtUrl2">Enter URL 2:</label><br />
            <asp:TextBox ID="txtUrl2" runat="server" Width="300px"></asp:TextBox><br />

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
            <label for="txtChatId">Enter Chat ID (default: "test-chat-id"):</label><br />
            <asp:TextBox ID="txtChatId" runat="server" Width="300px" Text="test-chat-id"></asp:TextBox><br />

            <!-- Button to get Chat History -->
            <asp:Button ID="btnGetChatHistory" runat="server" Text="Get Chat History" OnClick="btnGetChatHistory_Click" /><br />
        </div>

        <!-- Chat History Result Section -->
        <div>
            <h3>Chat History:</h3>
            <asp:TextBox ID="txtChatHistory" runat="server" TextMode="MultiLine" Width="800px" Height="300px" ReadOnly="true"></asp:TextBox>
        </div>

        <!-- Word Count Section -->
        <div>
            <h2>Upload Text File for Word Count</h2>
            <asp:FileUpload ID="fileUpload" runat="server" accept=".txt" />
            <asp:Button ID="btnUpload" runat="server" Text="Upload and Count Words" OnClick="btnUpload_Click" />
        </div>
        <div>
            <h3>Word Count Result (JSON):</h3>
            <asp:TextBox ID="txtCountWordResult" runat="server" TextMode="MultiLine" 
                Width="800px" Height="50px" ReadOnly="true"></asp:TextBox>
        </div>

        <!-- Web Download Section -->
        <div>
            <h2>Test Web Download Service</h2>
            <label for="txtUrl">Enter URL:</label><br />
            <asp:TextBox ID="txtUrl" runat="server" Width="300px"></asp:TextBox><br />
            <asp:Button ID="btnDownload" runat="server" Text="Download" OnClick="btnDownload_Click" /><br />
        </div>

        <div>
            <h3>Web Download Result:</h3>
            <!-- <asp:Label ID="lblWebDownloadResult" runat="server" Text=""></asp:Label> -->
            <asp:TextBox ID="txtWebDownloadResult" runat="server" TextMode="MultiLine" 
                Width="800px" Height="200px" ReadOnly="true"></asp:TextBox>
        </div>

    </form>
</body>
</html>
