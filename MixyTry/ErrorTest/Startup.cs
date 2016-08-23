using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ErrorTest.Startup))]
namespace ErrorTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
