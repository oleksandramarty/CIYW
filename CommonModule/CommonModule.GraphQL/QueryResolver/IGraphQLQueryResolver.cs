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

    void GetResultForEmptyCommand<TEntityType, TCommand, TEntityResponse>(GraphQLEndpoint endpoint)
        where TEntityType : ObjectGraphType<TEntityResponse>
        where TCommand : IRequest<TEntityResponse>, new();
}