using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace customizedDashboadSample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public class Global
        {
            public static string _CDSAuthenticationKey = ConfigurationManager.AppSettings["CDSAuthenticationKey"];
            public static string _CDSServiceTokenRole = ConfigurationManager.AppSettings["CDSServiceTokenRole"];
            public static string _CDSAPIServiceBaseURI = ConfigurationManager.AppSettings["CDSAPIServiceBaseURI"];

            public static string _CDSAPIServiceTokenEndPoint = _CDSAPIServiceBaseURI + "token";
            public static string _CDSCompanyEndPoint = _CDSAPIServiceBaseURI + "cdstudio/company";
            public static string _CDSFcactoryEndPoint = _CDSAPIServiceBaseURI + "cdstudio/factory";
        }
    }
}
