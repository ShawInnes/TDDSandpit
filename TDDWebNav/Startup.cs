using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TDDWebNav.Startup))]
namespace TDDWebNav
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
