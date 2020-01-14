using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestSharp;
using RestSharp.Authenticators;

namespace InterserverSSLTest.Controllers
{
    public class SSLController : Controller
    {
        // GET: SSL
        public ActionResult Index()
        {
            return View();
        }
        
        // POST: SSL/Index
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            var apiKey = collection["apikey"];
            var to = collection["to"];
            try
            {
                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator("api", apiKey);
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "sandboxaa07be2124b6407f8b84a25c232b739c.mailgun.org", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", "no-reply@peergroups.be");
                request.AddParameter("to", to);
                request.AddParameter("subject", "Hello");
                request.AddParameter("text", "Testing some Mailgun awesomness!");
                request.Method = Method.POST;
                var result = client.Execute(request);

                var viewmodel = new
                {
                    Content = result.Content,
                    StatusCode = result.StatusCode,
                    Exception = result.ErrorException.GetType().ToString(),
                    ErrorMessage = result.ErrorMessage
                };

                return RedirectToAction("Index", viewmodel);
            }
            catch (Exception e)
            {
                var viewmodel = new
                {
                    Content = "",
                    StatusCode = "",
                    Exception = e.GetType().ToString(),
                    ErrorMessage = e.Message
                };

                return RedirectToAction("Index", viewmodel);
            }
        }
    }
}
