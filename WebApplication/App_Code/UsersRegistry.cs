using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace WebApplication.App_Code
{
    public class UsersRegistry
    {
        private static string dataFilePath = HttpContext.Current.Server.MapPath("~/App_Data/users.txt");

        public UsersRegistry()
        {
            EnsureFileExists();
        }

        private void EnsureFileExists()
        {
            if (!File.Exists(dataFilePath))
            {
                File.Create(dataFilePath).Close();
            }
        }


        // Write a new User to the file
        public static void WriteUser(User user)
        {
            using (StreamWriter sw = File.AppendText(dataFilePath))
            {
                sw.WriteLine(user.UserId + "," + user.Username + "," + user.Password + "," + user.UserRole);
            }
        }

        // Read all users from the file
        public static List<User> ReadUsers()
        {
            var users = new List<User>();
            string[] lines = File.ReadAllLines(dataFilePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 4)
                {
                    string userId = parts[0];
                    string username = parts[1];
                    string password = parts[2];
                    string userRole = parts[3];

                    if (userRole == "MemberUser")
                    {
                        users.Add(new MemberUser(userId, username, password));
                    }
                    else if (userRole == "StaffUser")
                    {
                        users.Add(new StaffUser(userId, username, password));
                    }
                }
            }
            return users;
        }

        // Check if user exists by username
        public static bool IsUserExists(string username)
        {
            var users = ReadUsers();
            foreach (var user in users)
            {
                if (user.Username == username)
                {
                    return true;
                }
            }
            return false;
        }

        // Validate user credentials
        public static User ValidateUser(string username, string password)
        {
            var users = ReadUsers();
            foreach (var user in users)
            {
                if (user.Username == username && user.Password == password)
                {
                    return user;
                }
            }
            return null;
        }

        // Retrievev user by userId
        public static User GetUserById(string userId) {
            var users = ReadUsers();
            foreach (var user in users)
            {
                if (user.UserId == userId)
                {
                    return user;
                }
            }
            return null;
        }
    }
}