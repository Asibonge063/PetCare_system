using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PetCare_system.Startup))]
namespace PetCare_system
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
