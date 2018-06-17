using ShortenUrl.Data;
using shortid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ShrtenUrl.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var repo = new UserRepository(Properties.Settings.Default.ConStr);
            var user = repo.Login(email, password);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            FormsAuthentication.SetAuthCookie(email, false);
            return RedirectToAction("Index");
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user, string password)
        {
            var repo = new UserRepository(Properties.Settings.Default.ConStr);
            repo.RegisterUser(user, password);
            return RedirectToAction("Login");
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Home/Login");
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult ShortenUrl()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ShortenUrl(string Realurl)
        {
            var urlrepo = new UrlRepository(Properties.Settings.Default.ConStr);
            var userrepo = new UserRepository(Properties.Settings.Default.ConStr);
            var url = urlrepo.GetUrl(User.Identity.Name, Realurl);
            if (url == null)
            {
                var user = userrepo.GetByEmail(User.Identity.Name);
                var shortenedUrl = ShortId.Generate(true, false);
                url = new URL
                {
                    RealURL = Realurl,
                    ShortenedURL = shortenedUrl,
                    UserId = user.Id
                };
                urlrepo.AddUrl(url);
            }
            return Json($"{ Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "")}/{url.ShortenedURL}");
        }
        public ActionResult ViewHistory()
        {
            var urlrepo = new UrlRepository(Properties.Settings.Default.ConStr);
            return View(urlrepo.GetUrlByEmail(User.Identity.Name));
        }

        [Route("{shortenedurl}")]
        public ActionResult ViewShortenedUrl(string shortenedurl)
        {
            var urlRepo = new UrlRepository(Properties.Settings.Default.ConStr);
            var url = urlRepo.Get(shortenedurl);
            if (url == null)
            {
                return View("/");
            }
            urlRepo.IncrementViews(url.id);
            return Redirect(url.RealURL);
        }
    }
}
