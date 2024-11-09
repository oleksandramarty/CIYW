using CommonModule.GraphQL.QueryResolver;

namespace AuditTrail.GraphQL;

public class AuditTrailRootQuery: GraphQLQueryHelper
{
    public AuditTrailRootQuery()
    {
        this.AddAuditTrailQueries();
    }
}  