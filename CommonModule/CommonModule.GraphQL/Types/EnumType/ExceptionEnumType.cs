using CommonModule.Shared.Enums.AuditTrail;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.EnumType;

public class ExceptionEnumType : EnumerationGraphType<ExceptionEnum>
{
    public ExceptionEnumType()
    {
        Name = "ExceptionEnum";
        Description = "Enumeration for exception types.";
    }
}