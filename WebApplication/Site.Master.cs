using System;
using System.Web;
using System.Web.UI;
using WebApplication.App_Code;

namespace WebApplication
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User currentUser = (User)Session["CurrentUser"];
            if (currentUser != null)
            {
                loggedInUser.InnerText = $"Welcome, {currentUser.Username}";
                linkLogin.Visible = false; // Hide the login link if user is logged in
                linkLogout.Visible = true; // Show the logout link if user is logged in
                loggedInUser.Visible = true; // Show current user name
            } 
            else
            {
                loggedInUser.InnerText = string.Empty; // No user information if not logged in
                linkLogin.Visible = true;  // Show the login link if user is not logged in
                linkLogout.Visible = false; // Hide the logout link if user is not logged in
                loggedInUser.Visible = true;
            }

            // Sets userId value that will be displayed in footer
            HttpCookie userCookie = Request.Cookies["userID"];
            footerUserId.InnerText = userCookie != null ? $"User ID: {userCookie.Value}" : "User ID: Not logged in";

        }

        
        protected void Logout_Click(object sender, EventArgs e)
        {
            User currentUser = (User)Session["CurrentUser"];

            if (currentUser != null)
            {
                currentUser.LogOut(); // Log's out user by removing the corresponding value in settings
            }

            Response.Redirect("~/Login.aspx");
        }
    }
}