using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VSP.Web.Startup))]
namespace VSP.Web
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
