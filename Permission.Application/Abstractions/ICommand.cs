using MediatR;

namespace Permission.Application.Abstractions
{
    public interface ICommand : IRequest
    {
    }
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
