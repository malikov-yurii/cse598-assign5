using System;


namespace WebApplication.App_Code
{
    public class StaffUser : User
    {
        public StaffUser(string userId, string username, string password) : base(userId, username, password, "StaffUser")
        {
        }
    }
}