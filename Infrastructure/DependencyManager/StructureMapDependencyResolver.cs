using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTempRecorder.Infrastructure.DependencyManager
{
    public class StructureMapDependencyResolver : StructureMapDependencyScope, IDependencyResolver
    {
        #region Private Fields

        private readonly IContainer _container;

        #endregion

        #region Constructor

        public StructureMapDependencyResolver(IContainer container) : base(container)
        {
            _container = container;
        }

        #endregion

        #region GetInstance Methods

        public object GetInstance(Type type)
        {
            return _container.GetInstance(type);
            //return StructuremapMvc.ParentScope.CurrentNestedContainer.GetInstance(type);
        }

        public IEnumerable<T> GetInstances<T>()
        {
            return _container.GetAllInstances<T>();
        }

        #endregion
    }
}
