using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace TechBlog.Models
{
    

    public partial class BlogEntities : DbContext
    {
        public BlogEntities()
            : base("name=BlogEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostLike> PostLikes { get; set; }
    }
}