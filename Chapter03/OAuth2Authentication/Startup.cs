using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OAuth2Authentication.Startup))]
namespace OAuth2Authentication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureWebApi(app);
        }
    }
}
