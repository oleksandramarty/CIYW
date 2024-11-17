using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Requests;

public class CategoriesRequest: BaseVersionEntity, IRequest<VersionedListResponse<CategoryResponse>>
{
    
}