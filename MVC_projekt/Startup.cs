using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC_projekt.Startup))]
namespace MVC_projekt
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
