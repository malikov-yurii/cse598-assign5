using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.ChatGPTServiceReference;

namespace TryItWebApplication
{
    public partial class LoginControl : System.Web.UI.UserControl
    {
        public event EventHandler LoginStatusChanged;

        private Dictionary<string, string> registeredUsers;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Always load registered users
            registeredUsers = Application["registeredUsers"] as Dictionary<string, string>;

            if (!IsPostBack)
            {
                if (Session["userId"] != null)
                {
                    lblLoginMessage.Text = $"Welcome, {Session["userId"]}!";
                    lblLoginMessage.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblLoginMessage.Text = "Please log in.";
                    lblLoginMessage.ForeColor = System.Drawing.Color.Red;
                }

                UpdateLoginLogoutVisibility();
            }
        }

        private void UpdateLoginLogoutVisibility()
        {
            if (Session["userId"] != null)
            {
                // User is logged in
                btnLogin.Visible = false;
                txtLoginUserId.Visible = false;
                txtPassword.Visible = false;
                btnLogout.Visible = true;
            }
            else
            {
                // User is not logged in
                btnLogin.Visible = true;
                txtLoginUserId.Visible = true;
                txtPassword.Visible = true;
                btnLogout.Visible = false;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string userId = txtLoginUserId.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (registeredUsers != null && registeredUsers.ContainsKey(userId))
            {
                if (registeredUsers[userId] == password)
                {
                    // Login successful
                    Session["userId"] = userId;
                    lblLoginMessage.Text = $"Welcome, {userId}!";
                    lblLoginMessage.ForeColor = System.Drawing.Color.Green;
                    OnLoginStatusChanged();
                }
                else
                {
                    lblLoginMessage.Text = "Incorrect password.";
                    lblLoginMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                lblLoginMessage.Text = "Login failed. You are not registered.";
                lblLoginMessage.ForeColor = System.Drawing.Color.Red;
            }

            UpdateLoginLogoutVisibility();
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear the session
            Session["userId"] = null;
            lblLoginMessage.Text = "You have been logged out.";
            lblLoginMessage.ForeColor = System.Drawing.Color.Red;
            OnLoginStatusChanged();
            UpdateLoginLogoutVisibility();
        }

        protected void OnLoginStatusChanged()
        {
            if (LoginStatusChanged != null)
            {
                LoginStatusChanged(this, EventArgs.Empty);
            }
        }
    }
}