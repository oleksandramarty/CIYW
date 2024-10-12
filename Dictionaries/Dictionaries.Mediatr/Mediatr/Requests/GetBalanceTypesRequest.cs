using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Balances;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Requests;

public class GetBalanceTypesRequest: BaseVersionEntity, IRequest<VersionedListResponse<BalanceTypeResponse>>
{
    
}