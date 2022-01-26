using Library.Services.Interfaces;
using System;

namespace Library.Services.Implementation
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetServices<T>() where T : class
        {
            var newservice = _serviceProvider.GetService(typeof(T));
            return (T)newservice;
        }
    }
}
