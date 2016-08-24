using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBlog.Models;
using TechBlog.Repository;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace TechBlog.Hubs
{
    [HubName("postHub")]
    public class PostHub : Hub
    {
        public Task Like(string postId)
        {
            var likePost = SaveLike(postId);
            return Clients.All.updateLikeCount(likePost);
        }
        private LikePost SaveLike(string id)
        {
            var postId = int.Parse(id);
            var baseContext = Context.Request.GetHttpContext();
            var postRepository = new PostRepository();
            var item = postRepository.GetById(postId);
            var liked = new PostLike
            {
                IPAddress = baseContext.Request.UserHostAddress,
                PostId = item.Id,
                UserAgent = baseContext.Request.UserAgent,
                UserLike = true
            };
            var dupe = item.PostLikes.FirstOrDefault(e => e.IPAddress == liked.IPAddress);
            if (dupe == null)
            {
                item.PostLikes.Add(liked);
            }
            else
            {
                dupe.UserLike = !dupe.UserLike;
            }
            postRepository.SaveChanges();
            var post = postRepository.GetById(postId);

            return new LikePost
            {
                LikeCount = post.PostLikes.Count(e => e.UserLike)
            };
        }

    }
}