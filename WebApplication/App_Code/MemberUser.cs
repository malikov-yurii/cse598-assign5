using System;


namespace WebApplication.App_Code
{
    public class MemberUser : User
    {
        public MemberUser(string userId, string username, string password) : base(userId, username, password, "MemberUser")
        {
        }
    }
}