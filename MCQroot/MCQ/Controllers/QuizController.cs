using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MCQ.Controllers
{
    [Authorize]
    public partial class QuizController : Controller
    {
        // GET: Quiz
        public virtual ActionResult Quiz()
        {
            return View();
        }

        public virtual ActionResult Question()
        {
            return View();
        }
    }
}