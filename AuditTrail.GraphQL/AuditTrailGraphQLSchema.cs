using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace AuditTrail.GraphQL;

public class AuditTrailGraphQLSchema : Schema
{
    public AuditTrailGraphQLSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<AuditTrailRootQuery>();
        Mutation = serviceProvider.GetRequiredService<ExpensesRootMutation>();
    }
}