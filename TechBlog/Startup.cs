using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TechBlog.Startup))]
namespace TechBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
