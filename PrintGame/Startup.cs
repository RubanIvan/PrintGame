using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PrintGame.Startup))]
namespace PrintGame
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
