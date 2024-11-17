using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplication.App_Code;


namespace WebApplication
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_AcquireRequestState(object sender, EventArgs e)
        {
            // Get the requested URL path
            string requestPath = HttpContext.Current.Request.Path;

            // Protect MemberPage and StaffPage
            if (requestPath.Contains("MemberPage") || requestPath.Contains("StaffPage"))
            {
                User currentUser = (User)Session["CurrentUser"];
                // Check if the user is authenticated
                // In real application there must be checks whether the
                // user exists and has correct permissions
                if (currentUser == null)
                {
                    // If not authenticated, redirect to the login page
                    HttpContext.Current.Response.Redirect("~/Login.aspx");
                }
            }
        }

        // Checks cookies and set's current user on the Session level
        protected void Session_Start(Object sender, EventArgs e) {
            HttpCookie userCookie = HttpContext.Current.Request.Cookies["userID"];
            
            if (userCookie == null) {
                return;
            }

            string userId = userCookie.Value;
            User user = UsersRegistry.GetUserById(userId);

            if (user != null)
            {
                user.LogIn();
            }
        }
    }
}