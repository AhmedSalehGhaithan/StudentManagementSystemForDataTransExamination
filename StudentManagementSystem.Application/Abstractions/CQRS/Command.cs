using MediatR;

namespace StudentManagementSystem.Application.Abstractions.CQRS;

public interface ICommand<out TResponse> : IRequest<TResponse> { }

public interface ICommand : IRequest<Unit> { }