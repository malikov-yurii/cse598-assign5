<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1 id="aspnetTitle">BrightFuture Invest</h1>
            <p class="lead">This web application is designed to assist investors involved in energy-safe building. 
                The application offers solution to analyze and estimate an attractiveness of the selected location 
                to invest into the eco-friendly and energy independent real estate</p>
        </section>

        <h2>Check Location Characteristics</h2>
        <asp:TextBox ID="txtLatitude" runat="server" Placeholder="Enter Latitude" /><br /><br />
        <asp:TextBox ID="txtLongitude" runat="server" Placeholder="Enter Longitude" /><br /><br />
        <asp:Button ID="btnGetLocationData" runat="server" Text="Get Location Data" OnClick="btnGetLocationData_Click" />
        <asp:Label ID="lblLocationData" runat="server" Text="" /><br />
    </main>
</asp:Content>
