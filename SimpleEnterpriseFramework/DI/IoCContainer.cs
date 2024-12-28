using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEnterpriseFramework.DI
{
    internal class IoCContainer
    {

        private static readonly Dictionary<Type, Func<object>> _registerHandler = new Dictionary<Type, Func<object>>();

        // Register an interface with returner.
        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            //Error throwing
            if (!typeof(TInterface).IsAssignableFrom(typeof(TImplementation)))
            {
                throw new ArgumentException($"Dependency: {typeof(TImplementation)} is incompatible with {typeof(TInterface)}!");
            }

            if (!(typeof(TInterface).IsInterface || typeof(TInterface).IsClass))
            {
                throw new ArgumentException($"{typeof(TInterface)} is not an interface.");
            }

            if (!typeof(TImplementation).IsClass)
            {
                throw new ArgumentException($"{typeof(TImplementation)} is not a class");
            }

            //Get the constructor
            var constructor = typeof(TImplementation).GetConstructors().FirstOrDefault();

            //Create if not null
            if (constructor != null)
            {
                var constParams = constructor.GetParameters();
                var moduleDeps = constParams.Select(param => Resolving(param.ParameterType)).ToArray();
                var module = Activator.CreateInstance(typeof(TImplementation), moduleDeps);

                _registerHandler[typeof(TInterface)] = () => module;
            }
            else
            {
                var module = Activator.CreateInstance(typeof(TImplementation));
                _registerHandler[typeof(TInterface)] = () => module;
            }
        }

        
        // Register an instance
        public void RegisterInstance<TInterface>(TInterface instance)
        {
            _registerHandler[typeof(TInterface)] = () => instance;
        }

        // Resolve service (create)
        public TInterface Resolve<TInterface>()
        {
            return (TInterface)Resolving(typeof(TInterface));
        }
        private static object Resolving(Type interfaceType)
        {
            if (_registerHandler.ContainsKey(interfaceType))
            {
                return _registerHandler[interfaceType];
            }
            throw new Exception($"Dependency: {interfaceType} not registered.");
        }
    }
}
