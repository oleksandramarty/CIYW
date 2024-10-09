using CommonModule.Shared.Enums.Expenses;
using CommonModule.Shared.Responses.Dictionaries.Models.Expenses;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Expenses;

public class FrequencyResponseType : ObjectGraphType<FrequencyResponse>
{
    public FrequencyResponseType()
    {
        Field(x => x.Id);
        Field(x => x.Title);
        Field(x => x.Description);
        Field(x => x.IsActive);
        Field<EnumerationGraphType<FrequencyEnum>>("frequencyEnum", resolve: context => context.Source.FrequencyEnum);
    }
}