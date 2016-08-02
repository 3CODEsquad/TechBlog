using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TechBlog.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [AllowHtml]
        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }


        [Required]
        public DateTime Date { get; set; }


        public ApplicationUser Author { get; set; }
    }
}