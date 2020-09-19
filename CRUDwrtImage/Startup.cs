using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CRUDwrtImage.Startup))]
namespace CRUDwrtImage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
