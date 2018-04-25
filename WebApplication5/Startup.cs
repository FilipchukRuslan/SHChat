using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Ninject;
using Owin;
using WebApplication5.Hubs;
using WebApplication5.IoC;
using WebApplication5.Models;

[assembly: OwinStartupAttribute(typeof(WebApplication5.Startup))]
namespace WebApplication5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.Register(
                typeof(ChatHub),
                () => new ChatHub(new UnitOfWork(new ApplicationDbContext())));
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
