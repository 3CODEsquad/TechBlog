using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace TechBlog.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public static Expression<Func<Post, PostViewModel>> ViewModel
        {
            get
            {
                return e => new PostViewModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    Author = e.Author.FullName
                };
            }
        }
    }
}