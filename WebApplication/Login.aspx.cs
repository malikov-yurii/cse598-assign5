using System;
using System.Web;
using System.Web.Security;
using WebApplication.App_Code;

namespace WebApplication
{
    public partial class Login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string returnUrl = Request.QueryString["ReturnUrl"];
            UsersRegistry userRegistry;

            // Identify whethere Member or Staff User is trying to login
            if (returnUrl.Contains("/Staff/"))
            {
                userRegistry = new StaffRegistry();
            }
            else {
                userRegistry = new MembersRegistry();
            }

            User user = userRegistry.ValidateUser(username, password); 

            if (user != null) {
                // Store the user ID in cookies
                HttpCookie userCookie = new HttpCookie("userID", user.UserId);
                Response.Cookies.Add(userCookie);

                // Store User details into the Session
                user.SessionLogIn();
                // Pass Forms Security
                FormsAuthentication.RedirectFromLoginPage(username, false);
            } else {
                lblLoginResult.Text = "Invalid login";
            }
        }

        // Only Member Users can be created
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string regUsername = txtUsername.Text.Trim();
            string regPassword = txtPassword.Text.Trim();
            UsersRegistry userRegistry = new MembersRegistry();

            if (string.IsNullOrEmpty(regUsername) || string.IsNullOrEmpty(regPassword))
            {
                lblLoginResult.Text = "Username and password cannot be empty.";
                return;
            }

            if (userRegistry.IsUserExists(regUsername))
            {
                lblLoginResult.Text = "Username already exists. Please choose a different one.";
            }
            else
            {
                // Save new user credentials to the Members.xml
                string userId = Guid.NewGuid().ToString();
                MemberUser user = new MemberUser(userId, regUsername, regPassword);
                userRegistry.WriteUser(user);
                lblLoginResult.Text = "Registration successful. You can now log in.";
            }
        }
    }
}