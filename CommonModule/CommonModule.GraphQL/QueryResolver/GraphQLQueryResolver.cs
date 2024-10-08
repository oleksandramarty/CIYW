using CommonModule.Shared.Requests.Base;
using CommonModule.Shared.Responses.Base;
using GraphQL;
using GraphQL.Types;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CommonModule.GraphQL.QueryResolver;

public class GraphQLQueryResolver : ObjectGraphType, IGraphQLQueryResolver
{
    public void GetEntityById<TEntityTypeId, TEntityType, TEntityId, TCommand, TEntityResponse>(GraphQLEndpoint endpoint)
        where TEntityType : ObjectGraphType<TEntityResponse>
        where TEntityTypeId : ScalarGraphType
        where TCommand : IRequest<TEntityResponse>, new()
    {
        Field<TEntityType>(endpoint.Name)
            .Arguments(new QueryArguments(new QueryArgument<TEntityTypeId> { Name = "id" }))
            .ResolveAsync(async context =>
            {
                context.IsAuthenticated(endpoint.IsAuthenticated);
                var cancellationToken = context.CancellationToken;
                TEntityId entityId = context.GetArgument<TEntityId>("id", default(TEntityId));
                TCommand command = (TCommand)Activator.CreateInstance(typeof(TCommand), entityId);
                var mediator = context.RequestServices.GetRequiredService<IMediator>();
                return await mediator.Send(command, cancellationToken);
            });
    }

    public void GetResultForNonEmptyCommand<TInputType, TEntityResponseType, TEntityResponse, TCommand, TCommandResponse>(GraphQLEndpoint endpoint)
        where TInputType: InputObjectGraphType
        where TEntityResponseType: ObjectGraphType<TEntityResponse>
        where TCommand: IRequest<TCommandResponse>, new()
    {
        Field<TEntityResponseType>(endpoint.Name)
            .Arguments(
                new QueryArgument<NonNullGraphType<TInputType>> { Name = "input" })
            .ResolveAsync(async context =>
            {
                context.IsAuthenticated(endpoint.IsAuthenticated);
                var cancellationToken = context.CancellationToken;
                TCommand command = context.GetArgument<TCommand>("input");
                var mediator = context.RequestServices.GetRequiredService<IMediator>();
                return await mediator.Send(command, cancellationToken);
            });
    }

    public void GetResultForEmptyCommand<TEntityType, TEntityResponse, TCommand, TCommandResponse>(GraphQLEndpoint endpoint)
        where TEntityType : ObjectGraphType<TEntityResponse>
        where TCommand : IRequest<TCommandResponse>, new()
    {
        Field<TEntityType>(endpoint.Name)
            .ResolveAsync(async context =>
            {
                context.IsAuthenticated(endpoint.IsAuthenticated);
                var cancellationToken = context.CancellationToken;
                var mediator = context.RequestServices.GetRequiredService<IMediator>();
                return await mediator.Send(new TCommand(), cancellationToken);
            });
    }
    
    public void GetResultsForEmptyCommand<TEntityType, TEntityResponse, TCommand, TCommandResponse>(GraphQLEndpoint endpoint)
        where TEntityType : ObjectGraphType<TEntityResponse>
        where TCommand : IRequest<TCommandResponse>, new()
    {
        Field<ListGraphType<TEntityType>>(endpoint.Name)
            .ResolveAsync(async context =>
            {
                context.IsAuthenticated(endpoint.IsAuthenticated);
                var cancellationToken = context.CancellationToken;
                var mediator = context.RequestServices.GetRequiredService<IMediator>();
                return await mediator.Send(new TCommand(), cancellationToken);
            });
    }
    
    public void GetFilteredEntities<TEntityType, TEntityResponse, TCommand>(GraphQLEndpoint endpoint)
        where TEntityType : ObjectGraphType<ListWithIncludeResponse<TEntityResponse>>
        where TCommand : IBaseFilterRequest, IRequest<ListWithIncludeResponse<TEntityResponse>>, new()
    {
        Field<TEntityType>(endpoint.Name)
            .Arguments(GraphQLExtension.GetPageableQueryArguments())
            .ResolveAsync(async context =>
            {
                context.IsAuthenticated(endpoint.IsAuthenticated);
                var cancellationToken = context.CancellationToken;
                TCommand command = context.GetFilterQuery<TCommand>();
                var mediator = context.RequestServices.GetRequiredService<IMediator>();
                return await mediator.Send(command, cancellationToken);
            });
    } 
}