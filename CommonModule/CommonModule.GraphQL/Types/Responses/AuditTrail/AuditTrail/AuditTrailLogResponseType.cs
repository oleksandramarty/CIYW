using CommonModule.Shared.Enums.AuditTrail;
using CommonModule.Shared.Responses.AuditTrail.AuditTrail;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.AuditTrail.AuditTrail
{
    public class AuditTrailLogResponseType : ObjectGraphType<AuditTrailLogResponse>
    {
        public AuditTrailLogResponseType()
        {
            Field(x => x.Id);
            Field(x => x.AuthorId);
            Field(x => x.Date);
            Field(x => x.Entity, type: typeof(EnumerationGraphType<AuditTrailEntityEnum>));
            Field(x => x.EntityId);
            Field(x => x.Method, type: typeof(EnumerationGraphType<AuditTrailMethodEnum>));
            Field(x => x.Uri);
            Field(x => x.OldValue, nullable: true);
            Field(x => x.NewValue, nullable: true);
        }
    }
}