using CommonModule.GraphQL.MutationResolver;

namespace AuditTrail.GraphQL;

public class ExpensesRootMutation: GraphQLMutationHelper
{
    public ExpensesRootMutation()
    {
        Name = "Mutation";
        this.AddAuditTrailMutations();
    }
}