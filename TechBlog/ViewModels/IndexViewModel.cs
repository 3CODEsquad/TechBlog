using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using TechBlog.Models;

namespace TechBlog.ViewModels
{
    public class IndexViewModel
    {
        public Post Post { get; set; }
        public int PostLikes { get; set; }
    }
}