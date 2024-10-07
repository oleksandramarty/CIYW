using CommonModule.Shared.Requests.Base;
using CommonModule.Shared.Responses.Base;
using GraphQL.Types;
using MediatR;

namespace CommonModule.GraphQL.QueryResolver;

public interface IGraphQLQueryResolver
{
    void GetEntityById<TEntityTypeId, TEntityType, TEntityId, TCommand, TEntityResponse>(GraphQLEndpoint endpoint)
        where TEntityType : ObjectGraphType<TEntityResponse>
        where TEntityTypeId : ScalarGraphType
        where TCommand : IRequest<TEntityResponse>, new();

    void GetResultForNonEmptyCommand<TInputType, TEntityResponseType, TEntityResponse, TCommand, TCommandResponse>(GraphQLEndpoint endpoint)
        where TInputType: InputObjectGraphType
        where TEntityResponseType: ObjectGraphType<TEntityResponse>
        where TCommand: IRequest<TCommandResponse>, new();
    
    void GetFilteredEntities<TEntityType, TEntityResponse, TCommand>(GraphQLEndpoint endpoint)
        where TEntityType : ObjectGraphType<ListWithIncludeResponse<TEntityResponse>>
        where TCommand : IBaseFilterRequest, IRequest<ListWithIncludeResponse<TEntityResponse>>, new();

    void GetResultsForEmptyCommand<TEntityType, TEntityResponse, TCommand, TCommandResponse>(GraphQLEndpoint endpoint)
        where TEntityType : ObjectGraphType<TEntityResponse>
        where TCommand : IRequest<TCommandResponse>, new();
}