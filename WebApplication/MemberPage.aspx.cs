using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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