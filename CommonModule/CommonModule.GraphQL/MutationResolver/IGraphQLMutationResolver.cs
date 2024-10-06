using GraphQL.Types;
using MediatR;

namespace CommonModule.GraphQL.MutationResolver;

public interface IGraphQLMutationResolver
{
    void CreateEntity<TType, TInputType, TCommand, TEntity>(string name)
        where TCommand : IRequest
        where TInputType : InputObjectGraphType
        where TType : ObjectGraphType;

    void UpdateEntity<TType, TInputType, TCommand, TMapped, TEntity, TId>(string name)
        where TCommand : IRequest
        where TInputType : InputObjectGraphType
        where TId : ScalarGraphType
        where TType : ObjectGraphType<TMapped>;

    void DeleteEntity<TCommand, TId>(string name)
        where TCommand : IRequest
        where TId : ScalarGraphType;
}