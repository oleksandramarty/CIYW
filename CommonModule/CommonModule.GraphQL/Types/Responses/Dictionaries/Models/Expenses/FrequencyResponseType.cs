using CommonModule.GraphQL.Types.EnumType;
using CommonModule.Shared.Enums.Expenses;
using CommonModule.Shared.Responses.Dictionaries.Models.Expenses;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Expenses;

public sealed class FrequencyResponseType : ObjectGraphType<FrequencyResponse>
{
    public FrequencyResponseType()
    {
        Field(x => x.Id);
        Field(x => x.Title, nullable: true);
        Field(x => x.Description);
        Field(x => x.Status, type: typeof(StatusEnumType));
        Field(x => x.Type, type: typeof(EnumerationGraphType<FrequencyEnum>));
    }
}