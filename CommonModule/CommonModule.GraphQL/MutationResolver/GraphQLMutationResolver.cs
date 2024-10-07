using CommonModule.Core.JsonConverter;
using GraphQL;
using GraphQL.Types;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CommonModule.GraphQL.MutationResolver;

public class GraphQLMutationResolver: ObjectGraphType, IGraphQLMutationResolver
{
    public void CreateEntity<TInputType, TCommand>(GraphQLEndpoint endpoint) 
        where TCommand: IRequest
        where TInputType: InputObjectGraphType
    {
        Field<BooleanGraphType>(endpoint.Name)
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<TInputType>> { Name = "input" }
            ))
            .ResolveAsync(async context =>
            {
                context.IsAuthenticated(endpoint.IsAuthenticated);
                var cancellationToken = context.CancellationToken;
                TCommand command = context.GetArgument<TCommand>("input");
                var mediator = context.RequestServices.GetRequiredService<IMediator>();
                await mediator.Send(command, cancellationToken);
                return true;
            });
    }
    
    public void UpdateEntity<TInputType, TCommand, TId>(GraphQLEndpoint endpoint) 
        where TCommand: IRequest
        where TInputType: InputObjectGraphType
        where TId: ScalarGraphType
    {
        Field<BooleanGraphType>(endpoint.Name)
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<TId>> { Name = "id" },
                new QueryArgument<NonNullGraphType<TInputType>> { Name = "input" }
            ))
            .ResolveAsync(async context =>
            {
                context.IsAuthenticated(endpoint.IsAuthenticated);
                var cancellationToken = context.CancellationToken;
                TCommand command = context.GetArgument<TCommand>("input");
                ReflectionUtils.SetValue<TCommand, Guid>(command, "Id", context.GetArgument<Guid>("id"));
                var mediator = context.RequestServices.GetRequiredService<IMediator>();
                await mediator.Send(command, cancellationToken);
                return true;
            });
    }
    
    public void DeleteEntity<TCommand, TId>(GraphQLEndpoint endpoint) 
        where TCommand: IRequest<bool>
        where TId: ScalarGraphType
    {
        Field<BooleanGraphType>(endpoint.Name)
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<TId>> { Name = "id" }
            ))
            .ResolveAsync(async context =>
            {
                context.IsAuthenticated(endpoint.IsAuthenticated);
                var cancellationToken = context.CancellationToken;
                TCommand command = Activator.CreateInstance<TCommand>();
                ReflectionUtils.SetValue<TCommand, Guid>(command, "Id", context.GetArgument<Guid>("id"));
                
                var mediator = context.RequestServices.GetRequiredService<IMediator>();
                await mediator.Send(command, cancellationToken);
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
}