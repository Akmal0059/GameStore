using GameStore.Domain.Abstract;
using GameStore.Domain.Concrete;
using GameStore.Domain.Entities;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernrlParam)
        {
            kernel = kernrlParam;
            AddBinding();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBinding()
        {
            kernel.Bind<IGameRepository>().To<EFGameRepository>();
        }
    }
}