using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebPhuotTTC.Startup))]
namespace WebPhuotTTC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
