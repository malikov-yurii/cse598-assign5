using System;
using WebApplication.App_Code;

namespace WebApplication
{
    public partial class Member : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User currentUser = (User)Session["CurrentUser"];

            if (currentUser != null)
            {
                usernameDisplay.InnerText = "Glad to see you back, " + currentUser.Username + "!";
            }
        }
    }
}