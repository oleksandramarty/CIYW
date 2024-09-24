using AutoMapper;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using Dictionaries.Domain;
using Dictionaries.Mediatr.Mediatr.Categories.Categories.Requests;
using Expenses.Domain.Models.Categories;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Categories.Categories.Handlers;

public class GetCategoriesRequestHandler: IRequestHandler<GetCategoriesRequest, List<CategoryResponse>>
{
    private readonly IMapper mapper;
    private readonly IReadGenericRepository<int, Category, DictionariesDataContext> categoryRepository;

    public GetCategoriesRequestHandler(
        IMapper mapper,
        IReadGenericRepository<int, Category, DictionariesDataContext> categoryRepository)
    {
        this.mapper = mapper;
        this.categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryResponse>> Handle(GetCategoriesRequest request, CancellationToken cancellationToken)
    {
        List<Category> categories = await this.categoryRepository.GetListAsync(c => c.IsActive, cancellationToken);
        return categories.Select(r => this.mapper.Map<CategoryResponse>(r)).ToList();
    }
}