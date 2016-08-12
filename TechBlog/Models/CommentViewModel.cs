namespace TechBlog.Models
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class CommentViewModel
    {
        public string Text { get; set; }

        public string Author { get; set; }

        public static Expression<Func<Comment, CommentViewModel>> ViewModel
        {
            get
            {
                return c => new CommentViewModel()
                {
                    Text = c.Body,
                    Author = c.Author.FullName
                };
            }
        }
    }
}
