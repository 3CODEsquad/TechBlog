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
    public class ReplaysController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Replays
        public ActionResult Index()
        {
            var replays = db.Replays.Include(r => r.Author).Include(r => r.Comment);
            return View(replays.ToList());
        }

        // GET: Replays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Replay replay = db.Replays.Find(id);
            if (replay == null)
            {
                return HttpNotFound();
            }
            return View(replay);
        }

        // GET: Replays/Create
        public ActionResult Create(Replay Replay, int? id)
        {
            var comment = db.Comments.Find(id).Id;
            Replay.Comment_Id = comment;

            var post = db.Comments.Find(id).Post_Id;
            Replay.ReplayPost_Id = post;
            return View(Replay);
        }

        // POST: Replays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Body,Date,Author_Id,Comment_Id")] Replay replay, int id)
        {
            if (ModelState.IsValid)
            {
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ApplicationUser user = UserManager.FindById(this.User.Identity.GetUserId());
                replay.Author = user;

                var commentId = db.Comments.Find(id).Id;
                replay.Comment_Id = commentId;
                Comment comment = db.Comments.Find(id);
                replay.Comment = comment;

                var postId = db.Comments.Find(id).Post_Id;
                replay.ReplayPost_Id = postId;

                db.Replays.Add(replay);
                db.SaveChanges();
                return RedirectToAction("Details", "Posts", new { id = postId });
            }
            return View(replay);
        }

        // GET: Replays/Edit/5
        public ActionResult Edit(Replay Replay,int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Replay replay = db.Replays.Find(id);
            if (replay == null)
            {
                return HttpNotFound();
            }

            Replay postAuthor = db.Replays.Include(b => b.Author).Single(b => b.Id == id);
            //var comment = db.Comments.Find(id).Id;
            //Replay.Comment_Id = comment;

            //var post = db.Comments.Find(id).Post_Id;
            //Replay.ReplayPost_Id = post;

            return View(replay);
        }

        // POST: Replays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Body,Date,Author_Id,Post_Id,Comment_Id")] Replay replay, int id)
        {
            if (ModelState.IsValid)
            {
                db.Entry(replay).State = EntityState.Modified;

                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ApplicationUser user = UserManager.FindById(this.User.Identity.GetUserId());
                replay.Author = user;

                var commentId = db.Comments.Find(id).Id;
                replay.Comment_Id = commentId;
                Comment comment = db.Comments.Find(id);
                replay.Comment = comment;

                var postId = db.Comments.Find(id).Post_Id;
                replay.ReplayPost_Id = postId;

                db.SaveChanges();
                this.AddNotification("Replay Edited.", NotificationType.INFO);

                return RedirectToAction("Details", "Posts", new { id = postId});
            }

            return View(replay);
        }

        // GET: Replays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Replay replay = db.Replays.Find(id);
            if (replay == null)
            {
                return HttpNotFound();
            }
            return View(replay);
        }

        // POST: Replays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Replay replay = db.Replays.Find(id);
            db.Replays.Remove(replay);
            db.SaveChanges();
            return RedirectToAction("Index");
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
