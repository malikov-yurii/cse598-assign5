using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WebApplication.App_Code
{
    public abstract class UsersRegistry
    {
        protected string dataFilePath;
        protected string userRegistryType;

        public List<User> ReadUsers()
        {
            List<User> users = new List<User>();

            if (File.Exists(dataFilePath))
            {
                XDocument doc = XDocument.Load(dataFilePath);
                var userElements = doc.Descendants(userRegistryType);

                foreach (var userElement in userElements)
                {
                    string userId = userElement.Element("UserId").Value;
                    string username = userElement.Element("Username").Value;
                    string password = userElement.Element("Password").Value;

                    if (userRegistryType == "Member")
                    {
                        users.Add(new MemberUser(userId, username, password));
                    }

                    if (userRegistryType == "Staff")
                    {
                        users.Add(new StaffUser(userId, username, password));
                    }

                }
            }

            return users;
        }

        public bool IsUserExists(string username)
        {
            var users = ReadUsers();
            return users.Any(user => user.Username == username);
        }

        public User ValidateUser(string username, string password)
        {
            var users = ReadUsers();
            return users.FirstOrDefault(user => user.Username == username && user.Password == password);
        }

        public User GetUserById(string userId)
        {
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

        public abstract void WriteUser(User user);
    }

    public class MembersRegistry : UsersRegistry
    {
        public MembersRegistry()
        {
            dataFilePath = HttpContext.Current.Server.MapPath("~/App_Data/Members.xml");
            userRegistryType = "Member";
        }


        public override void WriteUser(User user)
        {
            XDocument doc = XDocument.Load(dataFilePath);
            XElement root = doc.Element("Users");

            User existingUser = GetUserById(user.UserId);

            if (existingUser != null)
            {
                return;
            }
            // Create new Member entry
            XElement newMember = new XElement("Member",
                new XElement("UserId", user.UserId),
                new XElement("Username", user.Username),
                new XElement("Password", user.Password),
                new XElement("UserRole", user.UserRole)
            );

            root.Add(newMember);
            doc.Save(dataFilePath);
        }
    }

    public class StaffRegistry : UsersRegistry
    {
        public StaffRegistry()
        {
            dataFilePath = HttpContext.Current.Server.MapPath("~/App_Data/Staff.xml");
            userRegistryType = "Staff";
        }

        public override void WriteUser(User user)
        {
            return; // It is not allowed to register Staff users from UI
        }
    }
}
