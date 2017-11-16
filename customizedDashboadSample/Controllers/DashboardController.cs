using customizedDashboadSample.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static customizedDashboadSample.MvcApplication;

namespace customizedDashboadSample.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public async Task<ActionResult> Index()
        {
            CDSAPIHelper cdsAPI = new CDSAPIHelper();
            string companyObjString = await cdsAPI.callAPIService("GET", Global._CDSCompanyEndPoint, null);
            dynamic companyObj = JsonConvert.DeserializeObject(companyObjString);

            string factoryListString = await cdsAPI.callAPIService("GET", Global._CDSFcactoryEndPoint, null);
            dynamic factoryList = JsonConvert.DeserializeObject(factoryListString);

            ViewBag.companyObj = companyObj;
            ViewBag.factoryList = factoryList;

            return View();
        }

        //RealTime Index
        public ActionResult Realtime()
        {
            return View();
        }

        //handle Ajax request
        public async Task<ActionResult> ReqAction()
        {
            string jsonString = "", endPoint = "";

            endPoint = Global._CDSAPIServiceBaseURI + "cdstudio/";

            if (Request.QueryString["action"] != null)
            {
                try
                {
                    CDSAPIHelper cdsAPI = new CDSAPIHelper();

                    switch (Request.QueryString["action"].ToString().ToLower())
                    {


                        case "getfactoryinfobyid":
                            {
                                string queryString = Request.Form.ToString();
                                endPoint = endPoint + "factory/" + Request.QueryString["Id"];

                                jsonString = await cdsAPI.callAPIService("GET", endPoint, null);

                                break;
                            }
                        default:



                            break;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToLower() == "invalid session")
                        Response.StatusCode = 401;
                    else
                    {
                    }
                }
            }
            return Content(JsonConvert.SerializeObject(jsonString), "application/json");
        }

    }
}