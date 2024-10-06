using CommonModule.Shared.Responses.Expenses.Models.Projects;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.Expenses.Models.Projects
{
    public class UserAllowedProjectResponseType : ObjectGraphType<UserAllowedProjectResponse>
    {
        public UserAllowedProjectResponseType()
        {
            Field(x => x.Id);
            Field(x => x.UserProjectId);
            Field<UserProjectResponseType>(
                name: "userProject",
                resolve: context => context.Source.UserProject
            );
            Field(x => x.UserId);
            Field(x => x.IsReadOnly);
        }
    }
}