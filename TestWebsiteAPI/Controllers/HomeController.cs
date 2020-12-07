using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Facebook;
using System.Web.Script.Serialization;

namespace TestWebsiteAPI.Controllers
{
    public class HomeController : Controller
    {
        
        private Uri RediredtUri

        {

            get

            {

                var uriBuilder = new UriBuilder(Request.Url);

                uriBuilder.Query = null;

                uriBuilder.Fragment = null;

                uriBuilder.Path = Url.Action("FacebookCallback");

                return uriBuilder.Uri;

            }

        }

        [AllowAnonymous]

        public ActionResult Facebook()

        {

            var fb = new FacebookClient();

            var loginUrl = fb.GetLoginUrl(new

            {

                client_id = "1075138046266613",

                client_secret = "24bd2a2a11a7d73cd5b1e3dbf8130671",

                redirect_uri = RediredtUri.AbsoluteUri,

                resporesnse_type = "code",

                scope = "email"

            });

            return Redirect(loginUrl.AbsoluteUri);

        }
        public ActionResult FacebookCallback(string code)

        {

            var fb = new FacebookClient();

            dynamic result = fb.Post("oauth/access_token", new

            {

                client_id = "1075138046266613",

                client_secret = "24bd2a2a11a7d73cd5b1e3dbf8130671",

                redirect_uri = RediredtUri.AbsoluteUri,

                code = code



            });

            var accessToken = result.access_token;

            fb.AccessToken = accessToken;

            dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");

            string email = me.email;
            string id = me.id;

            TempData["email"] = me.email;

            TempData["first_name"] = me.first_name;

            TempData["lastname"] = me.last_name;

            TempData["picture"] = me.picture.data.url;

            FormsAuthentication.SetAuthCookie(email, false);

            return RedirectToAction("About");

        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
    }
}