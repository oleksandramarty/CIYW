using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Expenses;
using Dictionaries.Domain;
using Dictionaries.Domain.Models.Expenses;
using Dictionaries.Mediatr.Mediatr.Requests;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Handlers;

public class GetFrequenciesRequestHandler(
    IDictionaryRepository<int, Frequency, FrequencyResponse, DictionariesDataContext> dictionaryRepository)
    : MediatrDictionaryBase<GetFrequenciesRequest, int, Frequency, FrequencyResponse, DictionariesDataContext>(
        dictionaryRepository), IRequestHandler<GetFrequenciesRequest, VersionedList<FrequencyResponse>>;