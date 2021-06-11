using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PersonalVideoGameLibrary.Startup))]
namespace PersonalVideoGameLibrary
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
