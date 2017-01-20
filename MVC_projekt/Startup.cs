using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin;
using MVC_projekt.Models.Classes;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC_projekt.Startup))]
namespace MVC_projekt
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            JobScheduler.Start();
        }
    }
}
