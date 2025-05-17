using MediatR;
namespace StudentManagementSystem.Application.Abstractions.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse>
where TResponse : notnull
{ }