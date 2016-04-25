using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Schema_Application.Startup))]
namespace Schema_Application
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
