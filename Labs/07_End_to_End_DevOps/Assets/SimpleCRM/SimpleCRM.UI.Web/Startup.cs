using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SimpleCRM.UI.Startup))]
namespace SimpleCRM.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
