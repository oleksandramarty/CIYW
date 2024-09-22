using CommonModule.Shared.Responses.Categories;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Categories.Requests;

public class GetCategoriesRequest: IRequest<List<CategoryResponse>>
{
    
}