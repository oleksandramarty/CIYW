using CommonModule.GraphQL.Types.EnumType;
using CommonModule.GraphQL.Types.Responses.Localizations;
using CommonModule.Shared;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Requests.Base;
using CommonModule.Shared.Responses.Base;
using GraphQL;
using GraphQL.Types;
using Localizations.Mediatr.Mediatr.Locations.Requests;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CommonModule.GraphQL.QueryResolver;

public class GraphQLQueryResolver : ObjectGraphType, IGraphQLQueryResolver
{
    public void GetEntityById<TEntityTypeId, TEntityType, TEntityId, TEntityResponse, TCommand, TCommandResponse>(GraphQLEndpoint endpoint)
        where TEntityType : ObjectGraphType<TEntityResponse>
        where TEntityTypeId : ScalarGraphType
        where TCommand : IBaseIdEntity<TEntityId>, IRequest<TCommandResponse>, new()
    {
        Field<TEntityType>(endpoint.Name)
            .Arguments(new QueryArguments(new QueryArgument<TEntityTypeId> { Name = "id" }))
            .ResolveAsync(async context =>
            {
                context.IsAuthenticated(endpoint.IsAuthenticated);
                var cancellationToken = context.CancellationToken;
                TCommand command = new TCommand();
                command.Id = context.GetArgument<TEntityId>("id");
                var mediator = context.RequestServices.GetRequiredService<IMediator>();
                return await mediator.Send(command, cancellationToken);
            });
    }

    public void GetResultForNonEmptyCommand<TEntityInputType, TEntityResponseType, TEntityResponse, TCommand, TCommandResponse>(GraphQLEndpoint endpoint)
        where TEntityInputType : InputObjectGraphType
        where TEntityResponseType : ObjectGraphType<TEntityResponse>
        where TCommand : IRequest<TCommandResponse>, new()
    {
        Field<TEntityResponseType>(endpoint.Name)
            .Arguments(
                new QueryArgument<NonNullGraphType<TEntityInputType>> { Name = "input" })
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
    
    public void GetFilteredEntities<TEntityType, TEntityResponse, TCommand>(GraphQLEndpoint endpoint)
        where TEntityType : ObjectGraphType<FilteredListResponse<TEntityResponse>>
        where TCommand : IBaseFilterRequest, IRequest<FilteredListResponse<TEntityResponse>>, new()
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
    
    public void GetVersionedList<TEntityType, TEntityResponse, TCommand>(GraphQLEndpoint endpoint)
        where TEntityType : ObjectGraphType<VersionedListResponse<TEntityResponse>>
        where TCommand : IBaseVersionEntity, IRequest<VersionedListResponse<TEntityResponse>>, new()
    {
        Field<TEntityType>(endpoint.Name)
            .Arguments(new QueryArguments(new QueryArgument<StringGraphType> { Name = "version" }
            ))
            .ResolveAsync(async context =>
            {
                context.IsAuthenticated(endpoint.IsAuthenticated);
                var cancellationToken = context.CancellationToken;
                TCommand command = new TCommand();
                command.Version = context.GetArgument<string>("version");
                var mediator = context.RequestServices.GetRequiredService<IMediator>();
                return await mediator.Send(command, cancellationToken);
            });
    }

    public void ExecuteForEmptyCommand<TCommand>(GraphQLEndpoint endpoint)
        where TCommand : IRequest, new()
    {
        Field<BooleanGraphType>(endpoint.Name)
            .ResolveAsync(async context =>
            {
                context.IsAuthenticated(endpoint.IsAuthenticated);
                var cancellationToken = context.CancellationToken;
                var mediator = context.RequestServices.GetRequiredService<IMediator>();
                await mediator.Send(new TCommand(), cancellationToken);
                return true;
            });
    }
    
    public void GetLocalizations(GraphQLEndpoint endpoint)
    {
        Field<LocalizationsResponseType>(endpoint.Name)
            .Arguments(new QueryArguments(new QueryArgument<StringGraphType> { Name = "version" }
            ))
            .ResolveAsync(async context =>
            {
                context.IsAuthenticated(endpoint.IsAuthenticated);
                var cancellationToken = context.CancellationToken;
                GetLocalizationsRequest command = new GetLocalizationsRequest();
                command.IsPublic = !endpoint.IsAuthenticated;
                command.Version = context.GetArgument<string>("version");
                var mediator = context.RequestServices.GetRequiredService<IMediator>();
                return await mediator.Send(command, cancellationToken);
            });
    }
}