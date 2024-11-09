using CommonModule.Shared.Enums.AuditTrail;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.EnumType;

public class AuditTrailActionEnumType : EnumerationGraphType<AuditTrailActionEnum>
{
    public AuditTrailActionEnumType()
    {
        Name = "AuditTrailActionEnum";
        Description = "Enumeration for audit trail action types.";
    }
}