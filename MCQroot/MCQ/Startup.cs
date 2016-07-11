using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MCQ.Startup))]
namespace MCQ
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
