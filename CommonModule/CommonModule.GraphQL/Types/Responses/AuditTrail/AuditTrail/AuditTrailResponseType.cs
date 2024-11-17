using CommonModule.GraphQL.Types.EnumType;
using CommonModule.Shared.Enums.AuditTrail;
using CommonModule.Shared.Responses.AuditTrail;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.AuditTrail.AuditTrail;

public sealed class AuditTrailResponseType : ObjectGraphType<AuditTrailResponse>
{
    public AuditTrailResponseType()
    {
        Field(x => x.Id);
        Field(x => x.CreatedAt);
        Field(x => x.EntityType, nullable: true, type: typeof(AuditTrailEntityEnumType));
        Field(x => x.Action, nullable: true, type: typeof(AuditTrailActionEnumType));
        Field(x => x.Type, type: typeof(AuditTrailEnumType));
        Field(x => x.ExceptionType, nullable: true, type: typeof(ExceptionEnumType));
        Field(x => x.Message, nullable: true);
        Field(x => x.EntityId, nullable: true);
        Field(x => x.OldValue, nullable: true);
        Field(x => x.NewValue, nullable: true);
        Field(x => x.Payload, nullable: true);
        Field(x => x.Uri, nullable: true);
        Field(x => x.UserId, nullable: true);
        Field(x => x.ArchiveDate, nullable: true);
    }
}

