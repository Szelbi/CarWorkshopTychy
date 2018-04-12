using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarWorkshop.Startup))]
namespace CarWorkshop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
