using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication.App_Code;

namespace WebApplication
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            User currentUser = (User)Session["CurrentUser"];

            if (currentUser != null)
            {
                currentUser.LogOut();
            }

            Response.Redirect("~/Login.aspx");
        }
    }
}