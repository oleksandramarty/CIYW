using CommonModule.GraphQL.Types.Common;
using CommonModule.Shared;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Requests.Base;
using CommonModule.Shared.Responses.Base;
using GraphQL.Types;
using MediatR;

namespace CommonModule.GraphQL.QueryResolver;

public interface IGraphQLQueryResolver
{
    void GetEntityById<TEntityTypeId, TEntityType, TEntityId, TEntityResponse, TCommand, TCommandResponse>(GraphQLEndpoint endpoint)
        where TEntityType : ObjectGraphType<TEntityResponse>
        where TEntityTypeId : ScalarGraphType
        where TCommand : IBaseIdEntity<TEntityId>, IRequest<TCommandResponse>, new();

    void GetResultForNonEmptyCommand<TEntityInputType, TEntityResponseType, TEntityResponse, TCommand, TCommandResponse>(GraphQLEndpoint endpoint)
        where TEntityInputType : InputObjectGraphType
        where TEntityResponseType : ObjectGraphType<TEntityResponse>
        where TCommand : IRequest<TCommandResponse>, new();

    void GetResultForEmptyCommand<TEntityType, TEntityResponse, TCommand, TCommandResponse>(GraphQLEndpoint endpoint)
        where TEntityType : ObjectGraphType<TEntityResponse>
        where TCommand : IRequest<TCommandResponse>, new();

    void GetFilteredEntities<TEntityType, TEntityResponse, TCommand>(GraphQLEndpoint endpoint)
        where TEntityType : ObjectGraphType<FilteredListResponse<TEntityResponse>>
        where TCommand : IBaseFilterRequest, IRequest<FilteredListResponse<TEntityResponse>>, new();

    void GetVersionedList<TEntityType, TEntityResponse, TCommand>(GraphQLEndpoint endpoint)
        where TEntityType : ObjectGraphType<VersionedListResponse<TEntityResponse>>
        where TCommand : IBaseVersionEntity, IRequest<VersionedListResponse<TEntityResponse>>, new();

    void GetVersionedTreeList<TEntityType, TEntityResponse, TCommand>(GraphQLEndpoint endpoint)
        where TEntityType : ObjectGraphType<VersionedListResponse<TreeNodeResponse<TEntityResponse>>>
        where TCommand : IBaseVersionEntity, IRequest<VersionedListResponse<TreeNodeResponse<TEntityResponse>>>, new();
    
    void ExecuteForEmptyCommand<TCommand>(GraphQLEndpoint endpoint)
        where TCommand : IRequest, new();

    void GetLocalizations(GraphQLEndpoint endpoint);
}