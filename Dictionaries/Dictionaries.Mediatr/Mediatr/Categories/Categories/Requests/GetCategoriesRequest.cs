using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Categories.Categories.Requests;

public class GetCategoriesRequest: IRequest<List<CategoryResponse>>
{
    
}