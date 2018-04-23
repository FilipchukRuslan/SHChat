using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication5.Interfaces;
using WebApplication5.Models;
using WebApplication5.Repositories;
using Ninject.Modules;

namespace WebApplication5.IoC
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>()
                .To<UnitOfWork>()
                .InSingletonScope();
            Bind(typeof(IBaseRepository<>))
                .To(typeof(BaseRepository<>));
            Bind<ApplicationDbContext>()
                .To<ApplicationDbContext>();
        }
    }
}