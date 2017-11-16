using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static customizedDashboadSample.MvcApplication;

namespace customizedDashboadSample.Models
{
    public class CDSAPIHelper
    {
        public CDSAPIHelper()
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });
        }

        public async Task<string> callAPIService(string method, string endPointURI, string postData)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endPointURI);
            request.Method = method;
            HttpWebResponse response = null;

            try
            {
                request.ContentType = "application/x-www-form-urlencoded";
                if (HttpContext.Current.Session["CDSAPIToken"] != null)
                    request.Headers.Add("Authorization", "Bearer " + HttpContext.Current.Session["CDSAPIToken"].ToString());

                switch (method.ToLower())
                {
                    case "get":
                    case "delete":
                        response = request.GetResponse() as HttpWebResponse;
                        break;
                    case "post":
                    case "put":
                        using (Stream requestStream = request.GetRequestStream())
                        using (StreamWriter writer = new StreamWriter(requestStream, Encoding.ASCII))
                        {
                            writer.Write(postData);
                        }
                        response = (HttpWebResponse)request.GetResponse();
                        break;
                    default:
                        throw new Exception("Method:" + method + " Not Support");
                }
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                var httpResponse = (HttpWebResponse)ex.Response;

                if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (await getAPIToken())
                        return await callAPIService(method, endPointURI, postData);
                }
                else
                    throw new Exception(response.StatusCode.ToString());
            }
            catch (Exception ex)
            {
                throw;
            }

            return null;
        }

        private async Task<bool> getAPIToken()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;


            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "grant_type", "password" },
                { "email", "" },
                { "password", Global._CDSAuthenticationKey },
                { "role", Global._CDSServiceTokenRole }
            });

            string uri = Global._CDSAPIServiceTokenEndPoint;
            response = await client.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                dynamic access_result = JObject.Parse(result);
                string access_token = access_result.access_token;
                if (!string.IsNullOrEmpty(access_token))
                {
                    HttpContext.Current.Session["CDSAPIToken"] = access_token;
                    return true;
                }
                return false;
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new Exception("Authentication Fail");
                else
                    throw new Exception();
            }
        }
    }

}