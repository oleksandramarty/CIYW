using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Requests;

public class GetCategoriesRequest: BaseVersionEntity, IRequest<VersionedListResponse<TreeNodeResponse<CategoryResponse>>>
{
    
}