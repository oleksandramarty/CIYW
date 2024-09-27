using System.Linq.Expressions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Interfaces;

public interface IEntityValidator<TDataContext> where TDataContext : DbContext
{
    Task ValidateExistParamAsync<T>(Expression<Func<T, bool>> predicate, string customErrorMessage, CancellationToken cancellationToken) where T : class;
    void ValidateExist<T, TId>(T entity, TId? entityId) where T : class?;
    void ValidateExist<T>(T? entity, string? message = null);
    void ValidateRequest<TCommand, TResult>(TCommand command, Func<IValidator<TCommand>> validatorFactory) where TCommand : IRequest<TResult>;
    void ValidateVoidRequest<TCommand>(TCommand command, Func<IValidator<TCommand>> validatorFactory) where TCommand : IRequest;
}