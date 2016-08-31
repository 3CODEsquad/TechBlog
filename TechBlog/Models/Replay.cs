using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
namespace TechBlog.Models
{
    public class Replay
    {
        public Replay()
        {
            this.Date = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [AllowHtml]
        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }


        [Required]
        public DateTime Date { get; set; }

        public string Author_Id { get; set; }
        [ForeignKey("Author_Id")]
        public ApplicationUser Author { get; set; }




        public int Comment_Id { get; set; }
        [ForeignKey("Comment_Id")]
        public Comment Comment { get; set; }

        public int? ReplayPost_Id { get; set; }

        [ForeignKey("ReplayPost_Id")]
        public Post Post { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}