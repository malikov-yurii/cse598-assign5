using System;
using System.Web;
using System.Web.Security;
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
                linkLogout.Visible = true; // Show the logout link if user is logged in
                loggedInUser.Visible = true; // Show current user name

                // userId value Taken from the cookies that will be displayed in footer (for testing purpose)
                HttpCookie userCookie = Request.Cookies["userID"];
                footerUserId.InnerText = userCookie != null ? $"User ID: {userCookie.Value}" : "User ID: Not logged in";
            }
            else
            {
                loggedInUser.InnerText = string.Empty; // No user information if not logged in
                linkLogout.Visible = false; // Hide the logout link if user is not logged in
                loggedInUser.Visible = true;

            }
        }
        
        protected void Logout_Click(object sender, EventArgs e)
        {
            // Signing out from Security Forms and removing corresponding value from the Session storage
            FormsAuthentication.SignOut();
            User currentUser = (User)Session["CurrentUser"];

            if (currentUser != null)
            {
                currentUser.SessionLogOut(); // Log's out user by removing the corresponding value in settings
            }

            Response.Redirect("~/Default.aspx");
        }

        protected void btnMemberPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Members/MemberPage.aspx");
        }

        protected void btnStaffPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Staff/StaffPage.aspx");
        }
    }
}