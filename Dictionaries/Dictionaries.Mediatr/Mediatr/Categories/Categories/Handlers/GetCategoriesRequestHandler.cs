using AutoMapper;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using Dictionaries.Domain;
using Dictionaries.Domain.Models.Categories;
using Dictionaries.Mediatr.Mediatr.Categories.Categories.Requests;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Categories.Categories.Handlers;

public class GetCategoriesRequestHandler : MediatrTreeBase<DictionariesDataContext, Category, CategoryResponse, int, int?>, IRequestHandler<GetCategoriesRequest, List<TreeNodeResponse<CategoryResponse>>>
{
    private readonly IMapper mapper;
    private readonly IEntityValidator<DictionariesDataContext> entityValidator;
    private readonly IReadGenericRepository<int, Category, DictionariesDataContext> categoryRepository;

    public GetCategoriesRequestHandler(
        IMapper mapper, 
        IEntityValidator<DictionariesDataContext> entityValidator, 
        IReadGenericRepository<int, Category, DictionariesDataContext> categoryRepository): base(mapper, entityValidator, categoryRepository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.categoryRepository = categoryRepository;
    }

    public async Task<List<TreeNodeResponse<CategoryResponse>>> Handle(GetCategoriesRequest request, CancellationToken cancellationToken)
    {
        List<Category> categories = await this.categoryRepository.GetListAsync(r => r.IsActive, cancellationToken);
        
        List<Category> mainCategories = categories.Where(c => c.ParentId == null).ToList();

        List<TreeNodeResponse<CategoryResponse>> response = await BuildSummitsTreeNode(mainCategories, categories.Where(c => c.ParentId.HasValue), cancellationToken);
        
        return response;
    }
}