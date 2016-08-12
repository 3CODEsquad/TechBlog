using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace TechBlog.Models
{
    public class PostDetailsViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string AuthorId { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static Expression<Func<Post, PostDetailsViewModel>> ViewModel
        {
            get
            {
                return e => new PostDetailsViewModel()
                {
                    Id = e.Id,
                    Comments = e.Comments.AsQueryable().Select(CommentViewModel.ViewModel),
                    AuthorId = e.Author.Id
                };
            }
        }
}