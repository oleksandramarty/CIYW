using CommonModule.Shared.Enums;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.EnumType;

public class DictionaryEnumType: EnumerationGraphType<DictionaryEnum>
{
    public DictionaryEnumType()
    {
        Name = "Dictionary";
        Description = "The dictionaries available for querying.";
    }
}