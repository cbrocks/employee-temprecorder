using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EmployeeTempRecorder.Infrastructure.DependencyManager
{
    public class StructureMapDependencyScope
    {

        #region Constructor

        public StructureMapDependencyScope(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            Container = container;
        }

        #endregion

        #region Public Properties

        public IContainer Container { get; set; }

        public IContainer CurrentNestedContainer { get; set; }

        #endregion

        #region Public Methods and Operators

        public void CreateNestedContainer()
        {
            if (CurrentNestedContainer != null)
                return;

            CurrentNestedContainer = Container.GetNestedContainer();
        }

        public void Dispose()
        {
            DisposeNestedContainer();
            Container.Dispose();
        }

        public void DisposeNestedContainer()
        {
            if (CurrentNestedContainer != null)
            {
                CurrentNestedContainer.Dispose();
                CurrentNestedContainer = null;
            }
        }

        public IEnumerable<object> GetServices(Type type)
        {
            return DoGetAllInstances(type);
        }

        #endregion

        #region Get Instance Methods

        protected IEnumerable<object> DoGetAllInstances(Type type)
        {
            return (CurrentNestedContainer ?? Container).GetAllInstances(type).Cast<object>();
        }

        protected object DoGetInstance(Type type, string key)
        {
            IContainer container = (CurrentNestedContainer ?? Container);

            if (string.IsNullOrEmpty(key))
                return type.GetTypeInfo().IsAbstract || type.GetTypeInfo().IsInterface
                    ? container.TryGetInstance(type)
                    : container.GetInstance(type);

            return container.GetInstance(type, key);
        }

        #endregion
    }
}
