using AutoMapper;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Currencies;
using CommonModule.Shared.Responses.Localizations;
using CommonModule.Shared.Responses.Localizations.Models.Locales;
using Localizations.Domain;
using Localizations.Domain.Models.Locales;
using Localizations.Mediatr.Mediatr.Locations.Requests;
using MediatR;

namespace Localizations.Mediatr.Mediatr.Locations.Handlers;

public class GetLocalesRequestHandler(
    IDictionaryRepository<int, Locale, LocaleResponse, LocalizationsDataContext> dictionaryRepository)
    : MediatrDictionaryBase<GetLocalesRequest, int, Locale, LocaleResponse, LocalizationsDataContext>(
        dictionaryRepository), IRequestHandler<GetLocalesRequest, VersionedList<LocaleResponse>>;