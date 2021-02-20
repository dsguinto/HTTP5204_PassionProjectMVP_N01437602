using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HTTP5204_PassionProject_N01437602.Startup))]
namespace HTTP5204_PassionProject_N01437602
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
