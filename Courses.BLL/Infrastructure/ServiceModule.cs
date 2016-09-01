using Courses.DAL.Interfaces;
using Courses.DAL.Repositories;
using Ninject.Modules;

namespace Courses.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private readonly string _connectionString;
        public ServiceModule(string connection)
        {
            _connectionString = connection;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<EfUnitOfWork>().WithConstructorArgument(_connectionString);
        }
    }
}
