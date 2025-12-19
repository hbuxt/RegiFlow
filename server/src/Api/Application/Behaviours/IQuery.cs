using Api.Application.Abstractions;
using MediatR;

namespace Api.Application.Behaviours
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}