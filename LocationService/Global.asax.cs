using LocationService.Configuration;
using System;
using System.Web.Http;

namespace LocationService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(LocationServiceAPIConfig.Register);
        }
    }
}