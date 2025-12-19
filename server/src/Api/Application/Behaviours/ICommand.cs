using Api.Application.Abstractions;
using MediatR;

namespace Api.Application.Behaviours
{
    public interface IBaseCommand
    {
    }
    
    public interface ICommand : IRequest<Result>, IBaseCommand
    {
    }
    
    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
    {
    }
}