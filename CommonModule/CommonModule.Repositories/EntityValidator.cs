using System.Linq.Expressions;
using System.Net;
using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Constants;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Repositories;

public class EntityValidator<TDataContext> : IEntityValidator<TDataContext> where TDataContext : DbContext
{
    private readonly TDataContext dataContext;

    public EntityValidator(TDataContext dataContext)
    {
        this.dataContext = dataContext;
    }
    
    public void IsEntityExist<T>(T entity)
    {
        if (entity == null)
        {
            throw new EntityNotFoundException();
        }
    }

    public void IsEntityActive<T>(T entity) where T : IActivatable?
    {
        if (entity.IsActive == false)
        {
            throw new BusinessException(ErrorMessages.EntityBlocked, (int)HttpStatusCode.Conflict);
        }
    }

    public void IsEntityLocked<T>(T entity) where T : IPessimisticOfflineLockEntity?
    {
        if (entity.IsLocked)
        {
            throw new BusinessException(string.Format(ErrorMessages.EntityPessimisticLocked, entity.LockedBy, entity.LockedAt), (int)HttpStatusCode.Conflict);
        }
    }

    public void IsEntityHasWrongVersion<T>(T entity, string version) where T : IBaseVersionEntity?
    {
        if (entity.Version != version)
        {
            throw new BusinessException(ErrorMessages.VersionNotSpecified, (int)HttpStatusCode.Conflict);
        }
    }

    public async Task ValidateExistParamAsync<T>(Expression<Func<T, bool>> predicate, string customErrorMessage, CancellationToken cancellationToken) where T : class
    {
        T? entity = await this.dataContext.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);

        if (entity != null)
        {
            throw new Exception(!string.IsNullOrEmpty(customErrorMessage) ? customErrorMessage : ErrorMessages.EntityAlreadyExists);
        }
    }

    public void ValidateRequest<TCommand, TResult>(TCommand command, Func<IValidator<TCommand>> validatorFactory) where TCommand : IRequest<TResult>
    {
        this.FluentValidation(validatorFactory.Invoke(), command);
    }
    
    public void ValidateVoidRequest<TCommand>(TCommand command, Func<IValidator<TCommand>> validatorFactory) where TCommand : IRequest
    {
        this.FluentValidation(validatorFactory.Invoke(), command);
    }

    private void FluentValidation<TCommand>(IValidator<TCommand> validator, TCommand command)
    {
        ValidationResult validationResult = validator.Validate(command);

        if (validationResult.IsValid)
        {
            return;
        }

        throw new Exception(ErrorMessages.ValidationError);
    }
}