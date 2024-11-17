using CommonModule.Shared;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using GraphQL.Types;
using MediatR;

namespace CommonModule.GraphQL.MutationResolver;

public interface IGraphQlMutationResolver
{
    void CreateEntity<TEntityInputType, TCommand>(GraphQlEndpoint endpoint)
        where TEntityInputType : InputObjectGraphType
        where TCommand : IRequest;

    void UpdateEntity<TEntityInputType, TEntityTypeId, TEntityId, TCommand>(GraphQlEndpoint endpoint)
        where TEntityInputType : InputObjectGraphType
        where TEntityTypeId : ScalarGraphType
        where TCommand : IBaseIdEntity<TEntityId>, IRequest;

    void DeleteEntity<TCommand, TEntityTypeId, TEntityId>(GraphQlEndpoint endpoint)
        where TEntityTypeId : ScalarGraphType
        where TCommand : IBaseIdEntity<TEntityId>, IRequest<BaseBoolResponse>, new();

}