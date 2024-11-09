using CommonModule.Shared.Enums.AuditTrail;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.EnumType;

public class AuditTrailEntityEnumType : EnumerationGraphType<AuditTrailEntityEnum>
{
    public AuditTrailEntityEnumType()
    {
        Name = "AuditTrailEntityEnum";
        Description = "Enumeration for AuditTrailEntityType.";
    }
}