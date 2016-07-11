using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MCQ.Controllers
{
    public partial class HomeController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Main()
        {
            return View();
        }

        public virtual ActionResult Login()
        {
            return View();
        }

        public virtual ActionResult Register()
        {
            return View();
        }
        //[Authorize]
        public virtual ActionResult TopicSummary()
        {
            return View();
        }
       // [Authorize]
        public virtual ActionResult Dashboard()
        {
            return View();
        }

        [Authorize]
        public virtual ActionResult Payment()
        {
            return View();
        }

        [Authorize]
        public virtual ActionResult AccountSettings()
        {
            return View();
        }

        public virtual ActionResult ResetPassword()
        {
            return View();
        }

    }
}