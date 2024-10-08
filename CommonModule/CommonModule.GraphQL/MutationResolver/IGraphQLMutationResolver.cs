using CommonModule.Shared.Common.BaseInterfaces;
using GraphQL.Types;
using MediatR;

namespace CommonModule.GraphQL.MutationResolver;

public interface IGraphQLMutationResolver
{
    void CreateEntity<TEntityInputType, TCommand>(GraphQLEndpoint endpoint)
        where TEntityInputType : InputObjectGraphType
        where TCommand : IRequest, new();

    void UpdateEntity<TEntityInputType, TEntityTypeId, TEntityId, TCommand>(GraphQLEndpoint endpoint)
        where TEntityInputType : InputObjectGraphType
        where TEntityTypeId : ScalarGraphType
        where TCommand : IBaseIdEntity<TEntityId>, IRequest;

    void DeleteEntity<TCommand, TEntityTypeId, TEntityId>(GraphQLEndpoint endpoint)
        where TEntityTypeId : ScalarGraphType
        where TCommand : IBaseIdEntity<TEntityId>, IRequest<bool>, new();

}