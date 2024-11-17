using CommonModule.GraphQL.QueryResolver;

namespace AuditTrail.GraphQL;

public class AuditTrailRootQuery: GraphQlQueryHelper
{
    public AuditTrailRootQuery()
    {
        this.AddAuditTrailQueries();
    }
}  