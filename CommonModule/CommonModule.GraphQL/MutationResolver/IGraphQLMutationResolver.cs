using GraphQL.Types;
using MediatR;

namespace CommonModule.GraphQL.MutationResolver;

public interface IGraphQLMutationResolver
{
    void CreateEntity<TInputType, TCommand>(GraphQLEndpoint endpoint)
        where TCommand : IRequest
        where TInputType : InputObjectGraphType;

    void UpdateEntity<TInputType, TCommand, TId>(GraphQLEndpoint endpoint)
        where TCommand : IRequest
        where TInputType : InputObjectGraphType
        where TId : ScalarGraphType;

    void DeleteEntity<TCommand, TId>(GraphQLEndpoint endpoint)
        where TCommand : IRequest<bool>
        where TId : ScalarGraphType;
}