using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Icons;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Requests;

public class IconCategoriesRequest: BaseVersionEntity, IRequest<VersionedListResponse<IconCategoryResponse>>
{
    
}