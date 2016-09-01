using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TechBlog.Extensions;
using TechBlog.Models;

namespace TechBlog.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            return View(db.Comments.Include(p => p.Author).OrderByDescending(b => b.Date).ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Include(b => b.Author).Single(b => b.Id == id);
            var replays = db.Replays.Where(p => p.Comment_Id == comment.Id).Include(b => b.Author).ToList();
            comment.Replays = replays;

            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        [HttpGet]
        public ActionResult Create(Comment Comment, int? id)
        {
            var post = db.Posts.Find(id).Id;
            Comment.Post_Id = post;
            return View(Comment);
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Body,Author_Id,Post_Id")] Comment Comment, int id)
        {
            if (ModelState.IsValid)
            {
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ApplicationUser user = UserManager.FindById(this.User.Identity.GetUserId());
                Comment.Author = user;

                var postId = db.Posts.Find(id).Id;
                Comment.Post_Id = postId;
                Post post = db.Posts.Find(id);
                Comment.Post = post;
                post.CommentsCount = post.CommentsCount + 1;

                db.Comments.Add(Comment);
                db.SaveChanges();
                return RedirectToAction("Details", "Posts", new { id = post.Id });
            }

            return View();
        }

        // GET: Comments/Edit/5
        [HttpGet]
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            Comment postAuthor = db.Comments.Include(b => b.Author).Single(b => b.Id == id);

            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Body,Date,Post_Id,Author_Id")] Comment comment, int id)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;

                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ApplicationUser user = UserManager.FindById(this.User.Identity.GetUserId());
                comment.Author = user;

                var postId = db.Posts.Find(id).Id;
                comment.Post_Id = postId;
                Post post = db.Posts.Find(id);  
                comment.Post = post;

                db.SaveChanges();
                this.AddNotification("Comment Edited.", NotificationType.INFO);

                return RedirectToAction("Details", "Posts", new { id = post.Id });
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            Post post = db.Posts.Find(id);
            db.Comments.Remove(comment);
            post.CommentsCount = post.CommentsCount - 1;
            db.SaveChanges();
            return RedirectToAction("Index", "Posts", null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
