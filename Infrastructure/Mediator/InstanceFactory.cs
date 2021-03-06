using System;
using System.Collections.Generic;

namespace EmployeeTempRecorder.Infrastructure.Mediator
{
    public delegate object SingleInstanceFactory(Type serviceType);     // Factory method for creating single instances. Used to build instances of
        
    public delegate IEnumerable<object> MultiInstanceFactory(Type serviceType);     // Factory method for creating multiple instances. Used to build instances of
}
