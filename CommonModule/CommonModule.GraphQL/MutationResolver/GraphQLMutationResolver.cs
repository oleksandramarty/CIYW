using CommonModule.Core.JsonConverter;
using GraphQL;
using GraphQL.Types;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CommonModule.GraphQL.MutationResolver;

public class GraphQLMutationResolver: ObjectGraphType, IGraphQLMutationResolver
{
    public void CreateEntity<TType, TInputType, TCommand, TEntity>(string name) 
        where TCommand: IRequest
        where TInputType: InputObjectGraphType
        where TType: ObjectGraphType
    {
        Field<TType>(name)
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<TInputType>> { Name = "input" }
            ))
            .ResolveAsync(async context =>
            {
                var cancellationToken = context.CancellationToken;
                await this.ModifyEntityWithoutResultAsync<TCommand>(context, cancellationToken);
                return null;
            });
    }
    
    public void UpdateEntity<TType, TInputType, TCommand, TMapped, TEntity, TId>(string name) 
        where TCommand: IRequest
        where TInputType: InputObjectGraphType
        where TId: ScalarGraphType
        where TType: ObjectGraphType<TMapped>
    {
        Field<TType>(name)
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<TId>> { Name = "id" },
                new QueryArgument<NonNullGraphType<TInputType>> { Name = "input" }
            ))
            .ResolveAsync(async context =>
            {
                var cancellationToken = context.CancellationToken;
                await this.ModifyEntityWithoutResultAsync<TCommand>(context, cancellationToken);
                return null;
            });
    }
    
    public void DeleteEntity<TCommand, TId>(string name) 
        where TCommand: IRequest
        where TId: ScalarGraphType
    {
        Field<BooleanGraphType>(name)
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<TId>> { Name = "id" }
            ))
            .ResolveAsync(async context =>
            {
                var cancellationToken = context.CancellationToken;
                await this.ModifyEntityWithoutResultAsync<TCommand>(context, cancellationToken);
                return true;
            });
    }

    private async Task<TResult> ModifyEntityWithResultAsync<TCommand, TResult>(IResolveFieldContext<object?> context, CancellationToken cancellationToken, bool isUpdate = false) where TCommand: IRequest<TResult>
    {
        TCommand command = context.GetArgument<TCommand>("input");
        if (isUpdate)
        {
            ReflectionUtils.SetValue<TCommand, Guid>(command, "Id", context.GetArgument<Guid>("id"));
        }
        var mediator = context.RequestServices.GetRequiredService<IMediator>();
        TResult result = await mediator.Send(command, cancellationToken);
        return result;
    }
    
    private async Task ModifyEntityWithoutResultAsync<TCommand>(IResolveFieldContext<object?> context, CancellationToken cancellationToken) where TCommand: IRequest
    {
        TCommand command = context.GetArgument<TCommand>("input");
        var mediator = context.RequestServices.GetRequiredService<IMediator>();
        await mediator.Send(command, cancellationToken);
    }
}