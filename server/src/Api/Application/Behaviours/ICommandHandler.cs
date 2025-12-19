using Api.Application.Abstractions;
using MediatR;

namespace Api.Application.Behaviours
{
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
        where TCommand : ICommand
    {
    }
    
    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result>
        where TCommand : ICommand<TResponse>
    {
    }
}