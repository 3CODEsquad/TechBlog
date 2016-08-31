using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TechBlog.Controllers
{
    public class TechBlogController : Controller
    {
        // GET: TechBlog
        public ActionResult Index()
        {
            return View("Index", "_StartLayout");
        }
    }
}