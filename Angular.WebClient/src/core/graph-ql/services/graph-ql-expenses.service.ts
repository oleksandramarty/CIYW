import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {ApolloQueryResult} from "@apollo/client";
import {GraphQlService} from "../graph-ql.service";
import {
    CREATE_EXPENSE, CREATE_PLANNED_EXPENSE,
    CREATE_USER_PROJECT,
    GET_FILTERED_EXPENSES,
    GET_FILTERED_PLANNED_EXPENSES,
    GET_FILTERED_USER_ALLOWED_PROJECTS,
    GET_FILTERED_USER_PROJECTS,
    GET_USER_PROJECT_BY_ID,
    REMOVE_EXPENSE, REMOVE_PLANNED_EXPENSE,
    UPDATE_EXPENSE, UPDATE_PLANNED_EXPENSE, UPDATE_USER_PROJECT
} from "../queries/graph-ql-expenses.query";
import {BaseGraphQlFilteredModel} from "../../models/common/base-graphql.model";
import {
    ColumnEnum,
    FilteredListResponseOfExpenseResponse,
    FilteredListResponseOfPlannedExpenseResponse, FilteredListResponseOfUserAllowedProjectResponse,
    FilteredListResponseOfUserProjectResponse,
    OrderDirectionEnum,
    UserProjectResponse
} from "../../api-clients/common-module.client";
import {ApolloBase} from "apollo-angular";

@Injectable({
    providedIn: 'root',
})
export class GraphQlExpensesService {
    constructor(
        private readonly apollo: GraphQlService
    ) {
    }

    get apolloClient(): ApolloBase<any> {
        return this.apollo.expenses;
    }

    public getFilteredExpenses(
        baseFilter: BaseGraphQlFilteredModel,
        userProjectId: string = '',
        categoryIds: number[] = []
    ): Observable<ApolloQueryResult<{
        expenses_get_filtered_expenses: FilteredListResponseOfExpenseResponse | undefined
    }>> {
        return this.apolloClient
            .watchQuery({
                query: GET_FILTERED_EXPENSES,
                variables: {
                    ...baseFilter,
                    userProjectId,
                    categoryIds
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{
            expenses_get_filtered_expenses: FilteredListResponseOfExpenseResponse | undefined
        }>>;
    }

    public getFilteredPlannedExpenses(
        baseFilter: BaseGraphQlFilteredModel,
        userProjectId: string = '',
        categoryIds: number[] = []
    ): Observable<ApolloQueryResult<{
        expenses_get_filtered_planned_expenses: FilteredListResponseOfPlannedExpenseResponse | undefined
    }>> {
        return this.apolloClient
            .watchQuery({
                query: GET_FILTERED_PLANNED_EXPENSES,
                variables: {
                    ...baseFilter,
                    userProjectId,
                    categoryIds
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{
            expenses_get_filtered_planned_expenses: FilteredListResponseOfPlannedExpenseResponse | undefined
        }>>;
    }

    public createOrUpdateExpense(
        id: string | undefined,
        title: string | undefined,
        description: string | undefined,
        amount: number | undefined,
        balanceId: string | undefined,
        date: Date | undefined,
        categoryId: number | undefined,
        userProjectId: string | undefined
    ): Observable<ApolloQueryResult<{ success: boolean }>> {
        return this.apolloClient
            .mutate({
                mutation: !!id ? UPDATE_EXPENSE : CREATE_EXPENSE,
                variables: {
                    ...(id && {id}),
                    title,
                    description,
                    amount,
                    balanceId,
                    date,
                    categoryId,
                    ...(!id && {userProjectId})
                },
            }) as Observable<ApolloQueryResult<{ success: boolean }>>;
    }

public createOrUpdatePlannedExpense(
    id: string | undefined,
    title: string | undefined,
    description: string | undefined,
    amount: number | undefined,
    balanceId: string | undefined,
    startDate: Date | undefined,
    endDate: Date | undefined,
    categoryId: number | undefined,
    userProjectId: string | undefined,
    frequencyId: number | undefined,
    isActive: boolean | undefined
): Observable<ApolloQueryResult<{ success: boolean }>> {
    return this.apolloClient
        .mutate({
            mutation: !!id ? UPDATE_PLANNED_EXPENSE : CREATE_PLANNED_EXPENSE,
            variables: {
                ...(id && { id }),
                title,
                description,
                amount,
                balanceId,
                startDate,
                endDate,
                categoryId,
                frequencyId,
                isActive,
                ...(!id && {userProjectId})
            },
        }) as Observable<ApolloQueryResult<{ success: boolean }>>;
}

    public removeExpense(id: string | undefined): Observable<ApolloQueryResult<{ success: boolean }>> {
        return this.apolloClient
            .mutate({
                mutation: REMOVE_EXPENSE,
                variables: {
                    id
                },
            }) as Observable<ApolloQueryResult<{ success: boolean }>>;
    }

    public removePlannedExpense(id: string | undefined): Observable<ApolloQueryResult<{ success: boolean }>> {
        return this.apolloClient
            .mutate({
                mutation: REMOVE_PLANNED_EXPENSE,
                variables: {
                    id
                },
            }) as Observable<ApolloQueryResult<{ success: boolean }>>;
    }

    public createUserProject(
        title: string,
        isActive: boolean
    ): Observable<ApolloQueryResult<{ success: boolean }>> {
        return this.apolloClient
            .mutate({
                mutation: CREATE_USER_PROJECT,
                variables: {
                    title,
                    isActive
                },
            }) as Observable<ApolloQueryResult<{ success: boolean }>>;
    }

    public updateUserProject(
        id: string,
        title: string,
        isActive: boolean
    ): Observable<ApolloQueryResult<{ success: boolean }>> {
        return this.apolloClient
            .mutate({
                mutation: UPDATE_USER_PROJECT,
                variables: {
                    title,
                    isActive
                },
            }) as Observable<ApolloQueryResult<{ success: boolean }>>;
    }

    public getUserProjectById(id: string): Observable<ApolloQueryResult<{
        expenses_get_user_project_by_id: UserProjectResponse
    }>> {
        return this.apolloClient
            .watchQuery({
                query: GET_USER_PROJECT_BY_ID,
                variables: {
                    id
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ expenses_get_user_project_by_id: UserProjectResponse }>>;
    }

    public getFilteredUserProjects(): Observable<ApolloQueryResult<{
        expenses_get_filtered_user_projects: FilteredListResponseOfUserProjectResponse
    }>> {
        return this.apolloClient
            .watchQuery({
                query: GET_FILTERED_USER_PROJECTS,
                variables: {
                    isFull: false,
                    pageNumber: 1,
                    pageSize: 10,
                    column: ColumnEnum.Created.toString(),
                    direction: OrderDirectionEnum.Desc.toString()
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{
            expenses_get_filtered_user_projects: FilteredListResponseOfUserProjectResponse
        }>>;
    }

    public getFilteredUserAllowedProjects(): Observable<ApolloQueryResult<{
        expenses_get_filtered_user_allowed_projects: FilteredListResponseOfUserAllowedProjectResponse
    }>> {
        return this.apolloClient
            .watchQuery({
                query: GET_FILTERED_USER_ALLOWED_PROJECTS,
                variables: {
                    isFull: false,
                    pageNumber: 1,
                    pageSize: 10,
                    column: ColumnEnum.Created.toString(),
                    direction: OrderDirectionEnum.Desc.toString()
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{
            expenses_get_filtered_user_allowed_projects: FilteredListResponseOfUserAllowedProjectResponse
        }>>;
    }
}