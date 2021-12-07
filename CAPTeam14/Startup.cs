using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CAPTeam14.Startup))]
namespace CAPTeam14
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
