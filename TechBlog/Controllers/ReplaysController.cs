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
            var replays = db.Replays.Include(r => r.Author).Include(r => r.Comment).Include(r => r.Post);
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

                db.Replays.Add(replay);
                db.SaveChanges();
                return RedirectToAction("Details", "Comments", new { id = comment.Id });
            }
            return View(replay);
        }

        // GET: Replays/Edit/5
        public ActionResult Edit(int? id)
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
           
            ViewBag.Comment_Id = new SelectList(db.Comments, "Id", "Body", replay.Comment_Id);
            
            return View(replay);
        }

        // POST: Replays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Body,Date,Author_Id,Post_Id,Comment_Id")] Replay replay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(replay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.Comment_Id = new SelectList(db.Comments, "Id", "Body", replay.Comment_Id);
            
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
