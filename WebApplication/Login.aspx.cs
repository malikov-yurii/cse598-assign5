using System;
using System.Web;
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
            
            User user = UsersRegistry.ValidateUser(username, password);

            if (user != null)
            {
                // Store the user ID in cookies to maintain login state
                HttpCookie userCookie = new HttpCookie("userID", user.UserId);
                Response.Cookies.Add(userCookie);
                user.LogIn();

                // Redirect to the MemberUser page after successful login
                // TODO redirect user based on the user role
                Response.Redirect("Default.aspx");
            }
            else
            {
                lblLoginResult.Text = "Invalid username or password. Please try again.";
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string regUsername = txtUsername.Text.Trim();
            string regPassword = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(regUsername) || string.IsNullOrEmpty(regPassword))
            {
                lblLoginResult.Text = "Username and password cannot be empty.";
                return;
            }

            if (UsersRegistry.IsUserExists(regUsername))
            {
                lblLoginResult.Text = "Username already exists. Please choose a different one.";
            }
            else
            {
                // Save new user credentials to the data file
                // For now calway create a MemberUser
                string userId = Guid.NewGuid().ToString();
                MemberUser user = new MemberUser(userId, regUsername, regPassword);
                UsersRegistry.WriteUser(user);
                lblLoginResult.Text = "Registration successful. You can now log in.";
            }
        }
    }
}