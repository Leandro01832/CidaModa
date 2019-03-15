using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Loja_Aline.Startup))]
namespace Loja_Aline
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
