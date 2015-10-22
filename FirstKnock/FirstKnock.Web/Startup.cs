using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FirstKnock.Web.Startup))]
namespace FirstKnock.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
