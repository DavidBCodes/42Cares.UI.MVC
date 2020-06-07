using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_42Cares.Startup))]
namespace _42Cares
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
