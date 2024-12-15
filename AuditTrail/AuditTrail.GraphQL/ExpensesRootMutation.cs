using CommonModule.GraphQL.MutationResolver;

namespace AuditTrail.GraphQL;

public class ExpensesRootMutation: GraphQlMutationHelper
{
    public ExpensesRootMutation()
    {
        Name = "Mutation";
        AddAuditTrailMutations();
    }
}