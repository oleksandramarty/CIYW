import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {ApolloQueryResult, WatchQueryFetchPolicy} from "@apollo/client";
import {GraphQlService} from "../graph-ql.service";
import {
    CREATE_EXPENSE,
    CREATE_FAVORITE_EXPENSE,
    CREATE_PLANNED_EXPENSE,
    CREATE_USER_BALANCE,
    CREATE_USER_PROJECT,
    FILTERED_EXPENSES,
    FILTERED_FAVORITE_EXPENSES,
    FILTERED_PLANNED_EXPENSES,
    FILTERED_USER_ALLOWED_PROJECTS,
    FILTERED_USER_PROJECTS,
    USER_PROJECT_BY_ID,
    REMOVE_EXPENSE,
    REMOVE_FAVORITE_EXPENSE,
    REMOVE_PLANNED_EXPENSE,
    REMOVE_USER_BALANCE,
    UPDATE_EXPENSE,
    UPDATE_FAVORITE_EXPENSE,
    UPDATE_PLANNED_EXPENSE,
    UPDATE_USER_BALANCE,
    UPDATE_USER_PROJECT
} from "../queries/graph-ql-expenses.query";
import {BaseGraphQlFilteredModel} from "../../models/common/base-graphql.model";
import {
    AuditTrailActionEnum,
    AuditTrailEntityEnum, AuditTrailEnum,
    ColumnEnum, ExceptionEnum, FilteredListResponseOfAuditTrailResponse,
    FilteredListResponseOfExpenseResponse, FilteredListResponseOfFavoriteExpenseResponse,
    FilteredListResponseOfPlannedExpenseResponse, FilteredListResponseOfUserAllowedProjectResponse,
    FilteredListResponseOfUserProjectResponse,
    OrderDirectionEnum,
    UserProjectResponse
} from "../../api-models/common.models";
import {ApolloBase} from "apollo-angular";
import {FILTERED_AUDIT_TRAIL} from "../queries/graph-ql-audit-trail.query";

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

    public filteredExpenses(
        baseFilter: BaseGraphQlFilteredModel,
        userProjectId: string = '',
        categoryIds: number[] = []
    ): Observable<ApolloQueryResult<{
        expenses_filtered_expenses: FilteredListResponseOfExpenseResponse | undefined
    }>> {
        return this.apolloClient
            .watchQuery({
                query: FILTERED_EXPENSES,
                variables: {
                    ...baseFilter,
                    userProjectId,
                    categoryIds
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{
            expenses_filtered_expenses: FilteredListResponseOfExpenseResponse | undefined
        }>>;
    }

    public filteredPlannedExpenses(
        baseFilter: BaseGraphQlFilteredModel,
        userProjectId: string = '',
        categoryIds: number[] = []
    ): Observable<ApolloQueryResult<{
        expenses_filtered_planned_expenses: FilteredListResponseOfPlannedExpenseResponse | undefined
    }>> {
        return this.apolloClient
            .watchQuery({
                query: FILTERED_PLANNED_EXPENSES,
                variables: {
                    ...baseFilter,
                    userProjectId,
                    categoryIds
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{
            expenses_filtered_planned_expenses: FilteredListResponseOfPlannedExpenseResponse | undefined
        }>>;
    }

    public filteredFavoriteExpenses(
        baseFilter: BaseGraphQlFilteredModel,
        userProjectId: string = '',
        categoryIds: number[] = []
    ): Observable<ApolloQueryResult<{
        expenses_filtered_favorite_expenses: FilteredListResponseOfFavoriteExpenseResponse | undefined
    }>> {
        return this.apolloClient
            .watchQuery({
                query: FILTERED_FAVORITE_EXPENSES,
                variables: {
                    ...baseFilter,
                    userProjectId,
                    categoryIds
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{
            expenses_filtered_favorite_expenses: FilteredListResponseOfFavoriteExpenseResponse | undefined
        }>>;
    }

    public filteredAuditTrail(
        baseFilter: BaseGraphQlFilteredModel,
        entityType: AuditTrailEntityEnum,
        action: AuditTrailActionEnum,
        type: AuditTrailEnum,
        exceptionType: ExceptionEnum,
        entityId: string,
        userId: string,
        translationKey: string
    ): Observable<ApolloQueryResult<{
        audit_trail_filtered_audit_trail: FilteredListResponseOfAuditTrailResponse | undefined
    }>> {
        return this.apolloClient
            .watchQuery({
                query: FILTERED_AUDIT_TRAIL,
                variables: {
                    ...baseFilter,
                    entityType,
                    action,
                    type,
                    exceptionType,
                    entityId,
                    userId,
                    translationKey
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{
            audit_trail_filtered_audit_trail: FilteredListResponseOfAuditTrailResponse | undefined
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
        userProjectId: string | undefined,
        favoriteExpenseId: string | undefined
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
                    ...(!id && {userProjectId, favoriteExpenseId})
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
                    ...(id && {id}),
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

    public createUserBalance(
        title: string,
        isActive: boolean,
        currencyId: number,
        balanceTypeId: number,
        userProjectId: string,
        iconId: number
    ): Observable<ApolloQueryResult<{ success: boolean }>> {
        return this.apolloClient
            .mutate({
                mutation: CREATE_USER_BALANCE,
                variables: {
                    title,
                    isActive,
                    currencyId,
                    balanceTypeId,
                    userProjectId,
                    iconId
                },
            }) as Observable<ApolloQueryResult<{ success: boolean }>>;
    }

    public updateUserBalance(
        id: string,
        title: string,
        isActive: boolean,
        currencyId: number,
        balanceTypeId: number,
        userProjectId: string,
        iconId: number
    ): Observable<ApolloQueryResult<{ success: boolean }>> {
        return this.apolloClient
            .mutate({
                mutation: UPDATE_USER_BALANCE,
                variables: {
                    id,
                    title,
                    isActive,
                    currencyId,
                    balanceTypeId,
                    userProjectId,
                    iconId
                },
            }) as Observable<ApolloQueryResult<{ success: boolean }>>;
    }

    public removeUserBalance(id: string | undefined): Observable<ApolloQueryResult<{ success: boolean }>> {
        return this.apolloClient
            .mutate({
                mutation: REMOVE_USER_BALANCE,
                variables: {
                    id
                },
            }) as Observable<ApolloQueryResult<{ success: boolean }>>;
    }

    public createFavoriteExpense(
        title: string,
        description: string | undefined,
        limit: number | undefined,
        categoryId: number | undefined,
        frequencyId: number | undefined,
        currencyId: number,
        userProjectId: string,
        iconId: number
    ): Observable<ApolloQueryResult<{ success: boolean }>> {
        return this.apolloClient
            .mutate({
                mutation: CREATE_FAVORITE_EXPENSE,
                variables: {
                    title,
                    description,
                    limit,
                    categoryId,
                    frequencyId,
                    currencyId,
                    userProjectId,
                    iconId
                },
            }) as Observable<ApolloQueryResult<{ success: boolean }>>;
    }

    public updateFavoriteExpense(
        id: string,
        title: string,
        description: string | undefined,
        limit: number | undefined,
        categoryId: number | undefined,
        frequencyId: number | undefined,
        currencyId: number,
        userProjectId: string,
        iconId: number
    ): Observable<ApolloQueryResult<{ success: boolean }>> {
        return this.apolloClient
            .mutate({
                mutation: UPDATE_FAVORITE_EXPENSE,
                variables: {
                    id,
                    title,
                    description,
                    limit,
                    categoryId,
                    frequencyId,
                    currencyId,
                    userProjectId,
                    iconId
                },
            }) as Observable<ApolloQueryResult<{ success: boolean }>>;
    }

    public removeFavoriteExpense(id: string): Observable<ApolloQueryResult<{ success: boolean }>> {
        return this.apolloClient
            .mutate({
                mutation: REMOVE_FAVORITE_EXPENSE,
                variables: {
                    id
                },
            }) as Observable<ApolloQueryResult<{ success: boolean }>>;
    }

    public userProjectById(id: string): Observable<ApolloQueryResult<{
        expenses_user_project_by_id: UserProjectResponse
    }>> {
        return this.apolloClient
            .watchQuery({
                query: USER_PROJECT_BY_ID,
                variables: {
                    id
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ expenses_user_project_by_id: UserProjectResponse }>>;
    }

    public filteredUserProjects(fetchPolicy: WatchQueryFetchPolicy | undefined): Observable<ApolloQueryResult<{
        expenses_filtered_user_projects: FilteredListResponseOfUserProjectResponse
    }>> {
        return this.apolloClient
            .watchQuery({
                query: FILTERED_USER_PROJECTS,
                variables: {
                    isFull: false,
                    pageNumber: 1,
                    pageSize: 10,
                    column: ColumnEnum.CreatedAt.toString(),
                    direction: OrderDirectionEnum.Desc.toString()
                },
                fetchPolicy,
            }).valueChanges as Observable<ApolloQueryResult<{
            expenses_filtered_user_projects: FilteredListResponseOfUserProjectResponse
        }>>;
    }

    public filteredUserAllowedProjects(fetchPolicy: WatchQueryFetchPolicy | undefined): Observable<ApolloQueryResult<{
        expenses_filtered_user_allowed_projects: FilteredListResponseOfUserAllowedProjectResponse
    }>> {
        return this.apolloClient
            .watchQuery({
                query: FILTERED_USER_ALLOWED_PROJECTS,
                variables: {
                    isFull: false,
                    pageNumber: 1,
                    pageSize: 10,
                    column: ColumnEnum.CreatedAt.toString(),
                    direction: OrderDirectionEnum.Desc.toString()
                },
                fetchPolicy,
            }).valueChanges as Observable<ApolloQueryResult<{
            expenses_filtered_user_allowed_projects: FilteredListResponseOfUserAllowedProjectResponse
        }>>;
    }
}