using AutoMapper;
using CommonModule.Interfaces;
using CommonModule.Shared.Common.BaseInterfaces;
using Dictionaries.Domain;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr;

public class MediatrDictionariesBase<TRequest, T, TResponse>: IRequestHandler<TRequest, List<TResponse>>
    where T : class, IActivatable
    where TRequest : IRequest<List<TResponse>>
{
    private readonly IMapper mapper;
    private readonly IReadGenericRepository<int, T, DictionariesDataContext> dictionaryRepository;

    public MediatrDictionariesBase(
        IMapper mapper,
        IReadGenericRepository<int, T, DictionariesDataContext> dictionaryRepository)
    {
        this.mapper = mapper;
        this.dictionaryRepository = dictionaryRepository;
    }

    public async Task<List<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        List<T> items = await this.dictionaryRepository.GetListAsync(c => c.IsActive, cancellationToken);
        return items.Select(r => this.mapper.Map<TResponse>(r)).ToList();
    }
}