using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechBlog.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

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