using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechBlog.Models;

namespace TechBlog.Controllers
{
    public class AdminPanelController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: AdminPanel
        public ActionResult Index()
        {
            ViewBag.TotalUsers = db.Users.Count();
            ViewBag.TotalPosts = db.Posts.Count();
            ViewBag.UsersList = db.Users.OrderBy(u => u.FullName);
            ViewBag.TotalComments = db.Comments.Count();
            //ViewBag.CommentsCount = db.CommentsCount.Count();
            return View();
        }
    }
}