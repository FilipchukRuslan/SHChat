using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Ninject;

namespace WebApplication5.IoC
{
    internal class NinjectSignalRDependencyResolver : DefaultDependencyResolver
    {
        private readonly IKernel kernel;
        public NinjectSignalRDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public override object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType) ?? base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType).Concat(base.GetServices(serviceType));
        }

        //This class overrides the GetService and GetServices methods of DefaultDependencyResolver.
        //SignalR calls these methods to create various objects at runtime, including hub instances, as well as various 
        //services used internally by SignalR.

        //The GetService method creates a single instance of a type.Override this method to call the Ninject kernel's 
        //TryGet method. If that method returns null, fall back to the default resolver.
        //The GetServices method creates a collection of objects of a specified type. Override this method to 
        //concatenate the results from Ninject with the results from the default resolver.
    }
}