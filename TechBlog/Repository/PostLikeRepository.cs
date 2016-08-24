using System.Collections.Generic;
using System.Data.Entity;
using TechBlog.Models;

namespace TechBlog.Repository
{
    public class PostLikeRepository : Repository<PostLike>
    {
        public PostLikeRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<PostLike> GetByPostId(int id)
        {
            return Find(e => e.PostId == id);
        }

        public bool Exists(int postId, string ipAddress)
        {
            var result = First(e => e.PostId == postId && e.IPAddress == ipAddress);
            return result != null;
        }

        public int CountByPostId(int id)
        {
            return Count(e => e.PostId == id);
        }
    }
}