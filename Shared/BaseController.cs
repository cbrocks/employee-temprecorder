using EmployeeTempRecorder.Infrastructure.CQRS;
using EmployeeTempRecorder.Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTempRecorder.Shared
{

    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        #region Constructor

        protected BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion

        protected ActionResult SendCommand(ICommand command)
        {
            return SendCommand(command);
        }

        //protected ActionResult SendCommand(ICommand command, Func<ActionResult> redirect)
        //{
        //    var result = _mediator.Send(command);
        //    if (result.HasException())
        //    {
        //        throw result.Exception;
        //    }
        //    if (redirect != null)
        //    {
        //        return redirect.Invoke();
        //    }

        //    return RedirectToAction("Index");
        //}
    }
}
