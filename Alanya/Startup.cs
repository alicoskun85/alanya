using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Alanya.Startup))]
namespace Alanya
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
