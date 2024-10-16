using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Icons;
using Dictionaries.Domain;
using Dictionaries.Domain.Models.Icons;
using Dictionaries.Mediatr.Mediatr.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionaries.Mediatr.Mediatr.Handlers;

public class GetIconCategoriesRequestHandler: IRequestHandler<GetIconCategoriesRequest, VersionedListResponse<IconCategoryResponse>>
{
    private readonly IDictionaryRepository<int, IconCategory, IconCategoryResponse, DictionariesDataContext> dictionaryRepository;
    
    public GetIconCategoriesRequestHandler(
        IDictionaryRepository<int, IconCategory, IconCategoryResponse, DictionariesDataContext> dictionaryRepository
        )
    {
        this.dictionaryRepository = dictionaryRepository;
    }
    
    public async Task<VersionedListResponse<IconCategoryResponse>> Handle(GetIconCategoriesRequest request, CancellationToken cancellationToken)
    {
        VersionedListResponse<IconCategoryResponse> response = await this.dictionaryRepository.GetDictionaryAsync(
            request.Version,
            cancellationToken,
            ic => ic.Include(i => i.Icons).OrderBy(i => i.Id));

        response.Items = response.Items.OrderBy(ic => ic.Id).ToList();
        return response;
    }
}