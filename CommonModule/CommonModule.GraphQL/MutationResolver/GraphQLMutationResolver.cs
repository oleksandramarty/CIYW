using CommonModule.Core.JsonConverter;
using CommonModule.Shared.Common.BaseInterfaces;
using GraphQL;
using GraphQL.Types;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CommonModule.GraphQL.MutationResolver;

public class GraphQLMutationResolver: ObjectGraphType, IGraphQLMutationResolver
{
    public void CreateEntity<TEntityInputType, TCommand>(GraphQLEndpoint endpoint)
        where TEntityInputType : InputObjectGraphType
        where TCommand : IRequest, new()
    {
        Field<BooleanGraphType>(endpoint.Name)
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<TEntityInputType>> { Name = "input" }
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
    
    public void UpdateEntity<TEntityInputType, TEntityTypeId, TEntityId, TCommand>(GraphQLEndpoint endpoint)
        where TEntityInputType : InputObjectGraphType
        where TEntityTypeId : ScalarGraphType
        where TCommand : IBaseIdEntity<TEntityId>, IRequest
    {
        Field<BooleanGraphType>(endpoint.Name)
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<TEntityTypeId>> { Name = "id" },
                new QueryArgument<NonNullGraphType<TEntityInputType>> { Name = "input" }
            ))
            .ResolveAsync(async context =>
            {
                context.IsAuthenticated(endpoint.IsAuthenticated);
                var cancellationToken = context.CancellationToken;
                TCommand command = context.GetArgument<TCommand>("input");
                command.Id = context.GetArgument<TEntityId>("id");
                var mediator = context.RequestServices.GetRequiredService<IMediator>();
                await mediator.Send(command, cancellationToken);
                return true;
            });
    }
    
    public void DeleteEntity<TCommand, TEntityTypeId, TEntityId>(GraphQLEndpoint endpoint)
        where TEntityTypeId : ScalarGraphType
        where TCommand : IBaseIdEntity<TEntityId>, IRequest<bool>, new()
    {
        Field<BooleanGraphType>(endpoint.Name)
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<TEntityTypeId>> { Name = "id" }
            ))
            .ResolveAsync(async context =>
            {
                context.IsAuthenticated(endpoint.IsAuthenticated);
                var cancellationToken = context.CancellationToken;
                TCommand command = new TCommand();
                command.Id = context.GetArgument<TEntityId>("id");
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