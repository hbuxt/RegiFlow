using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Abstractions
{
    public class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}