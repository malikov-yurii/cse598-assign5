using System;
using System.Web;


namespace WebApplication.App_Code
{
    public class User
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }

        public User(string userId, string username, string password, string userRole)
        {
            UserId = userId;
            Username = username;
            Password = password;
            UserRole = userRole;
        }

        public void LogIn() {
            HttpContext.Current.Session["CurrentUser"] = this;
        }

        public void LogOut()
        {
            HttpContext.Current.Session["CurrentUser"] = null;
        }
    }
}