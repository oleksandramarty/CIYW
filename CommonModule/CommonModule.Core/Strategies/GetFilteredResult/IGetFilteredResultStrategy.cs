using CommonModule.Shared.Responses.Base;
using MediatR;

namespace CommonModule.Core.Strategies.GetFilteredResult;

public interface IGetFilteredResultStrategy<in TCommand, TEntityResponse>
    where TCommand: IRequest<FilteredListResponse<TEntityResponse>>
{
    Task<FilteredListResponse<TEntityResponse>> GetFilteredResultAsync(TCommand request ,CancellationToken cancellationToken);
}