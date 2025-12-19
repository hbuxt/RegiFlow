using Api.Application.Abstractions;
using MediatR;

namespace Api.Application.Behaviours
{
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>
    {
    }
}