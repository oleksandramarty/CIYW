using CommonModule.Shared.Enums.AuditTrail;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.EnumType;

public class AuditTrailEnumType : EnumerationGraphType<AuditTrailEnum>
{
    public AuditTrailEnumType()
    {
        Name = "AuditTrailEnum";
        Description = "Enumeration for AuditTrail.";
    }
}