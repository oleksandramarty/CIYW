using System.Linq.Expressions;
using CommonModule.Interfaces;
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

    public async Task ValidateExistParamAsync<T>(Expression<Func<T, bool>> predicate, string customErrorMessage, CancellationToken cancellationToken) where T : class
    {
        T? entity = await this.dataContext.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);

        if (entity != null)
        {
            throw new Exception(!string.IsNullOrEmpty(customErrorMessage) ? customErrorMessage : ErrorMessages.EntityAlreadyExists);
        }
    }

    public void ValidateExist<T, TId>(T entity, TId? entityId) where T : class
    {
        if (entity == null)
        {
            throw new Exception(string.Format(ErrorMessages.EntityWithIdNotFound, typeof(T).Name, entityId));
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