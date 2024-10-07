import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {ApolloQueryResult} from "@apollo/client";
import {GraphQlService} from "../graph-ql.service";
import {
    CREATE_EXPENSE, CREATE_USER_PROJECT,
    GET_FILTERED_EXPENSES,
    GET_FILTERED_PLANNED_EXPENSES, GET_USER_ALLOWED_PROJECTS, GET_USER_PROJECT_BY_ID, GET_USER_PROJECTS, REMOVE_EXPENSE,
    UPDATE_EXPENSE
} from "../queries/graph-ql-expenses.query";
import {
    ColumnEnum,
    ListWithIncludeResponseOfExpenseResponse, ListWithIncludeResponseOfPlannedExpenseResponse,
    OrderDirectionEnum, UserAllowedProjectResponse, UserProjectResponse
} from "../../api-clients/common-module.client";

@Injectable({
    providedIn: 'root',
})
export class GraphQlExpensesService {
    constructor(
        private readonly apollo: GraphQlService
    ) {
    }

    public getFilteredExpenses(
        dateFrom: Date | undefined,
        dateTo: Date | undefined,
        amountFrom: number | undefined,
        amountTo: number | undefined,
        isFull: boolean = false,
        pageNumber: number = 1,
        pageSize: number = 10,
        column: ColumnEnum = ColumnEnum.Date,
        direction: OrderDirectionEnum = OrderDirectionEnum.Desc,
        query: string = '',
        userProjectId: string = '',
        categoryIds: number[] = []
    ): Observable<ApolloQueryResult<{
        expenses_get_filtered_expenses: ListWithIncludeResponseOfExpenseResponse | undefined
    }>> {
        return this.apollo.expenses
            .watchQuery({
                query: GET_FILTERED_EXPENSES,
                variables: {
                    ...this.apollo.handleBaseFilter(dateFrom, dateTo, amountFrom, amountTo, isFull, pageNumber, pageSize, column, direction, query),
                    userProjectId,
                    categoryIds
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{
            expenses_get_filtered_expenses: ListWithIncludeResponseOfExpenseResponse | undefined
        }>>;
    }

    public getFilteredPlannedExpenses(
        dateFrom: Date | undefined,
        dateTo: Date | undefined,
        amountFrom: number | undefined,
        amountTo: number | undefined,
        isFull: boolean = false,
        pageNumber: number = 1,
        pageSize: number = 10,
        column: ColumnEnum = ColumnEnum.Date,
        direction: OrderDirectionEnum = OrderDirectionEnum.Desc,
        query: string = '',
        userProjectId: string = '',
        categoryIds: number[] = []
    ): Observable<ApolloQueryResult<{
        expenses_get_filtered_planned_expenses: ListWithIncludeResponseOfPlannedExpenseResponse | undefined
    }>> {
        return this.apollo.expenses
            .watchQuery({
                query: GET_FILTERED_PLANNED_EXPENSES,
                variables: {
                    ...this.apollo.handleBaseFilter(dateFrom, dateTo, amountFrom, amountTo, isFull, pageNumber, pageSize, column, direction, query),
                    userProjectId,
                    categoryIds
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{
            expenses_get_filtered_planned_expenses: ListWithIncludeResponseOfPlannedExpenseResponse | undefined
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
        return this.apollo.expenses
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
                    userProjectId
                },
            }) as Observable<ApolloQueryResult<{ success: boolean }>>;
    }

    public removeExpense(id: string | undefined): Observable<ApolloQueryResult<{ success: boolean }>> {
        return this.apollo.expenses
            .mutate({
                mutation: REMOVE_EXPENSE,
                variables: {
                    id
                },
            }) as Observable<ApolloQueryResult<{ success: boolean }>>;
    }

    public createUserProject(
        title: string,
        currencyIds: number[],
        isActive: boolean
    ): Observable<ApolloQueryResult<{ success: boolean }>> {
        return this.apollo.expenses
            .mutate({
                mutation: CREATE_USER_PROJECT,
                variables: {
                    title,
                    currencyIds,
                    isActive
                },
            }) as Observable<ApolloQueryResult<{ success: boolean }>>;
    }

    public getUserProjectById(id: string): Observable<ApolloQueryResult<{ expenses_get_user_project_by_id: UserProjectResponse }>> {
        return this.apollo.expenses
            .watchQuery({
                query: GET_USER_PROJECT_BY_ID,
                variables: {
                    id
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ expenses_get_user_project_by_id: UserProjectResponse }>>;
    }

    public getUSerProjects(): Observable<ApolloQueryResult<{ expenses_get_user_projects: UserProjectResponse[] }>> {
        return this.apollo.expenses
            .watchQuery({
                query: GET_USER_PROJECTS,
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ expenses_get_user_projects: UserProjectResponse[] }>>;
    }

    public getUserAllowedProjects(): Observable<ApolloQueryResult<{ expenses_get_user_allowed_projects: UserAllowedProjectResponse[] }>> {
        return this.apollo.expenses
            .watchQuery({
                query: GET_USER_ALLOWED_PROJECTS,
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ expenses_get_user_allowed_projects: UserAllowedProjectResponse[] }>>;
    }
}