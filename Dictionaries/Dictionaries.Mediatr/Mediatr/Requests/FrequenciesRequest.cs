using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Expenses;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Requests;

public class FrequenciesRequest: BaseVersionEntity, IRequest<VersionedListResponse<FrequencyResponse>>
{
    
}