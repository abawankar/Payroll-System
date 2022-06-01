using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebPayroll.Startup))]
namespace WebPayroll
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
