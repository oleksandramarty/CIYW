using System.Linq.Expressions;
using CommonModule.Shared.Common.BaseInterfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Interfaces;

public interface IEntityValidator<TDataContext> where TDataContext : DbContext
{
    void IsEntityExist<T>(T entity);
    void IsEntityActive<T>(T entity)
        where T : IActivatable?;
    void IsEntityLocked<T>(T entity)
        where T : IPessimisticOfflineLockEntity?;
    void IsEntityHasWrongVersion<T>(T entity, string version)
        where T : IBaseVersionEntity?;

    Task ValidateExistParamAsync<T>(Expression<Func<T, bool>> predicate, string customErrorMessage,
        CancellationToken cancellationToken) where T : class;
    void ValidateRequest<TCommand, TResult>(TCommand command, Func<IValidator<TCommand>> validatorFactory) where TCommand : IRequest<TResult>;
    void ValidateVoidRequest<TCommand>(TCommand command, Func<IValidator<TCommand>> validatorFactory) where TCommand : IRequest;
}