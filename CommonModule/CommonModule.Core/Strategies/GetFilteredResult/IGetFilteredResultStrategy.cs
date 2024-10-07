using CommonModule.Shared.Responses.Base;
using MediatR;

namespace CommonModule.Core.Strategies.GetFilteredResult;

public interface IGetFilteredResultStrategy<in TCommand, TEntityResponse>
    where TCommand: IRequest<ListWithIncludeResponse<TEntityResponse>>
{
    Task<ListWithIncludeResponse<TEntityResponse>> GetFilteredResultAsync(TCommand request ,CancellationToken cancellationToken);
}