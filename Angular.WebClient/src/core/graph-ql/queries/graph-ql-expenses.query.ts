import {gql} from "@apollo/client";

export const GET_FILTERED_EXPENSES = gql`
    query GetFilteredExpenses(
        $isFull: Boolean,
        $pageNumber: Int,
        $pageSize: Int,
        $dateFrom: DateTime,
        $dateTo: DateTime,
        $column: String,
        $direction: String,
        $query: String,
        $amountFrom: Decimal,
        $amountTo: Decimal,
        $userProjectId: ID,
        $categoryIds: [Int]
    ) {
        expenses_get_filtered_expenses(
            isFull: $isFull,
            pageNumber: $pageNumber,
            pageSize: $pageSize,
            dateFrom: $dateFrom,
            dateTo: $dateTo,
            column: $column,
            direction: $direction,
            query: $query,
            amountFrom: $amountFrom,
            amountTo: $amountTo,
            userProjectId: $userProjectId,
            categoryIds: $categoryIds
        ) {
            entities {
                id
                created
                modified
                title
                description
                amount
                balanceBefore
                balanceAfter
                balanceId
                date
                categoryId
                userProjectId
                createdUserId
                version
            }
            paginator {
                pageNumber
                pageSize
                isFull
            }
            totalCount
        }
    }
`;

export const GET_FILTERED_PLANNED_EXPENSES = gql`
    query GetFilteredPlannedExpenses(
        $isFull: Boolean,
        $pageNumber: Int,
        $pageSize: Int,
        $dateFrom: DateTime,
        $dateTo: DateTime,
        $column: String,
        $direction: String,
        $query: String,
        $amountFrom: Decimal,
        $amountTo: Decimal,
        $userProjectId: ID,
        $categoryIds: [Int]
    ) {
        expenses_get_filtered_planned_expenses(
            isFull: $isFull,
            pageNumber: $pageNumber,
            pageSize: $pageSize,
            dateFrom: $dateFrom,
            dateTo: $dateTo,
            column: $column,
            direction: $direction,
            query: $query,
            amountFrom: $amountFrom,
            amountTo: $amountTo,
            userProjectId: $userProjectId,
            categoryIds: $categoryIds
        ) {
            entities {
                id
                created
                modified
                title
                description
                amount
                balanceBefore
                balanceAfter
                categoryId
                balanceId
                startDate
                endDate
                userId
                userProjectId
                frequencyId
                isActive
                version
            }
            paginator {
                pageNumber
                pageSize
                isFull
            }
            totalCount
        }
    }
`;

export const CREATE_EXPENSE = gql`
    mutation CreateOrUpdateExpenseInput(
        $title: String!,
        $description: String,
        $amount: Decimal!,
        $date: DateTime!,
        $categoryId: Int!,
        $userProjectId: ID!,
        $balanceId: ID!
    ) {
        expenses_create_expense(
            input: {
                title: $title,
                description: $description,
                amount: $amount,
                date: $date,
                categoryId: $categoryId,
                userProjectId: $userProjectId,
                balanceId: $balanceId
            }
        )
    }
`;

export const UPDATE_EXPENSE = gql`
    mutation CreateOrUpdateExpenseInput(
        $id: ID!,
        $title: String!,
        $description: String,
        $amount: Decimal!,
        $date: DateTime!,
        $categoryId: Int!,
        $userProjectId: ID!,
        $balanceId: ID!
    ) {
        expenses_update_expense(
            id: $id,
            input: {
                title: $title,
                description: $description,
                amount: $amount,
                date: $date,
                categoryId: $categoryId,
                userProjectId: $userProjectId,
                balanceId: $balanceId
            }
        )
    }
`;

export const REMOVE_EXPENSE = gql`
    mutation RemoveExpense($id: ID!) {
        expenses_remove_expense(id: $id)
        {
            success
        }
    }
`;

export const CREATE_USER_PROJECT = gql`
    mutation CreateUserProjectInput($title: String!, $isActive: Boolean!, $currencyIds: [Int!]!) {
        expenses_create_user_project(input: { title: $title, isActive: $isActive, currencyIds: $currencyIds }) {
            id
            title
            isActive
            currencyIds
        }
    }
`;

export const GET_USER_PROJECT_BY_ID = gql`
    query GetUserProjectById($id: ID!) {
        expenses_get_user_project_by_id(id: $id) {
            id
            title
            isActive
            createdUserId
            balances {
                id
                amount
                currency
                created
                modified
            }
            version
            created
            modified
        }
    }
`;

export const GET_USER_PROJECTS = gql`
    query GetUserProjects {
        expenses_get_user_projects {
            id
            title
            isActive
            createdUserId
            balances {
                id
                amount
                currency
                created
                modified
            }
            version
            created
            modified
        }
    }
`;

export const GET_USER_ALLOWED_PROJECTS = gql`
    query GetUserAllowedProjects {
        expenses_get_user_allowed_projects {
            id
            userProjectId
            userProject {
                id
                title
                isActive
                createdUserId
                balances {
                    id
                    amount
                    currency
                    created
                    modified
                }
                version
                created
                modified
            }
            userId
            isReadOnly
        }
    }
`;

