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

        //Set Current User in Session
        public void SessionLogIn()
        {
            HttpContext.Current.Session["CurrentUser"] = this;
        }

        //Remove Current User from Session
        public void SessionLogOut()
        {
            HttpContext.Current.Session["CurrentUser"] = null;
        }
    }

    public class StaffUser : User
    {
        public StaffUser(string userId, string username, string password) : base(userId, username, password, "StaffUser") { }
    }

    public class MemberUser : User
    {
        public MemberUser(string userId, string username, string password) : base(userId, username, password, "MemberUser") { }
    }
}