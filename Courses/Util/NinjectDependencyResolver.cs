using Courses.BLL.Interfaces;
using Courses.BLL.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Courses.Util
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<ICourseService>().To<CourseService>();
            kernel.Bind<ISpecializationService>().To<SpecializationService>();
            kernel.Bind<IJournalService>().To<JournalService>();
        }
    }
}