using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using TechBlog.Controllers;
using TechBlog.Models;

namespace TechBlog.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.Author).OrderByDescending(p => p.Date).Take(3);
            ViewBag.SidebarPosts = db.Posts.OrderByDescending(p => p.Date).Take(5);
            return View(posts.ToList());        
        }

        public ActionResult About()
        {
            ViewBag.Message = "\"The optimist proclaims that we live in the best of all possible worlds,\" observed satirist James Branch Cabell. \"The pessimist fears this is true.\".<br><br>The oldest debate about technology is the most basic: Is it a force for good or bad? There are many arguments about that, but lets ignore optimists and pessimists, we need to be realists and make this clear that the answer is . . . both. In these days technology and human life cannot be separated; society has a cyclical co - dependence on technology. We use technology; depend on technology in our daily life and our needs and demands for technology keep on rising.Humans use technology to travel, to communicate, to learn, to do business and to live in comfort. But how all of this affects our lives?<br><br>Our blog gives information about various science and technology topics. We concentrate on providing news from different sources and do not limit ourselves on some topics, but try to provide information ranging from pure technical disciplines to natural and social sciences.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}