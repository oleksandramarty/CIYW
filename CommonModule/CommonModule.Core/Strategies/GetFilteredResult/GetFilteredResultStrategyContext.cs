using CommonModule.Shared.Responses.Base;
using MediatR;

namespace CommonModule.Core.Strategies.GetFilteredResult;

public class GetFilteredResultStrategyContext<TCommand, TEntityResponse>
    where TCommand: IRequest<ListWithIncludeResponse<TEntityResponse>>
{
    private IGetFilteredResultStrategy<TCommand, TEntityResponse> _strategy;

    public GetFilteredResultStrategyContext(IGetFilteredResultStrategy<TCommand, TEntityResponse> strategy)
    {
        _strategy = strategy;
    }

    public void SetStrategy(IGetFilteredResultStrategy<TCommand, TEntityResponse> strategy)
    {
        _strategy = strategy;
    }

    public async Task<ListWithIncludeResponse<TEntityResponse>> ExecuteStrategyAsync(TCommand command, CancellationToken cancellationToken)
    {
        return await _strategy.GetFilteredResultAsync(command, cancellationToken);
    }
}