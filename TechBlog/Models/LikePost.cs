using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TechBlog.Models
{
    [DataContract]
    public class LikePost
    {
        [DataMember]
        public int LikeCount { get; set; }
    }
}