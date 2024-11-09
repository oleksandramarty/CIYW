using CommonModule.Shared.Enums.AuditTrail;
using CommonModule.Shared.Responses.AuditTrail;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.AuditTrail.AuditTrail
{
    public class AuditTrailLogResponseType : ObjectGraphType<AuditTrailResponse>
    {
        public AuditTrailLogResponseType()
        {
            Field(x => x.Id);
            Field(x => x.EntityId);
            Field(x => x.Uri);
            Field(x => x.OldValue, nullable: true);
            Field(x => x.NewValue, nullable: true);
            Field(x => x.EntityType, nullable: true, type: typeof(EnumerationGraphType<AuditTrailEntityTypeEnum>));
            Field(x => x.Action, nullable: true, type: typeof(EnumerationGraphType<AuditTrailActionTypeEnum>));
            Field(x => x.Type, type: typeof(EnumerationGraphType<AuditTrailTypeEnum>));
            Field(x => x.ExceptionType, nullable: true, type: typeof(EnumerationGraphType<ExceptionTypeEnum>));
            Field(x => x.Message, nullable: true);
            Field(x => x.Payload, nullable: true);
            Field(x => x.UserId, nullable: true);
            Field(x => x.ArchiveDate);
        }
    }
}