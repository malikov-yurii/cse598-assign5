using System;
using System.Collections.Generic;
using WebApplication.App_Code;


namespace WebApplication
{
    public partial class Staff : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Display All the Members
            MembersRegistry membersRegistry = new MembersRegistry();
            List<User> members = membersRegistry.ReadUsers();

            if (members.Count > 0)
            {
                // Create a table to display the members
                string tableHtml = "<table class=\"table table-dark\">";
                tableHtml += "<thead><tr><th>User ID</th><th>Username</th><th>User Role</th></tr></thead>";
                tableHtml += "<tbody>";

                foreach (User member in members)
                {
                    tableHtml += "<tr>";
                    tableHtml += $"<td>{member.UserId}</td>";
                    tableHtml += $"<td>{member.Username}</td>";
                    tableHtml += $"<td>{member.UserRole}</td>";
                    tableHtml += "</tr>";
                }

                tableHtml += "</tbody></table>";

                // Display the table on the page using a Literal control
                MembersTableLiteral.Text = tableHtml;
            }
        }
    }
}