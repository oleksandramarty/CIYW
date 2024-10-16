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

export const GET_FILTERED_FAVORITE_EXPENSES = gql`
    query GetFilteredFavoriteExpenses(
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
        expenses_get_filtered_favorite_expenses(
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
                limit
                currentAmount
                categoryId
                frequencyId
                currencyId
                endDate
                userProjectId
                iconId
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

export const CREATE_PLANNED_EXPENSE = gql`
    mutation CreatePlannedExpense(
        $title: String!,
        $description: String,
        $amount: Decimal!,
        $categoryId: Int!,
        $balanceId: ID!,
        $startDate: DateTime!,
        $endDate: DateTime,
        $userProjectId: ID!,
        $frequencyId: Int!,
        $isActive: Boolean!
    ) {
        expenses_create_planned_expense(
            input: {
                title: $title,
                description: $description,
                amount: $amount,
                categoryId: $categoryId,
                balanceId: $balanceId,
                startDate: $startDate,
                endDate: $endDate,
                userProjectId: $userProjectId,
                frequencyId: $frequencyId,
                isActive: $isActive
            }
        )
    }
`;

export const UPDATE_EXPENSE = gql`
    mutation CreateOrUpdateExpenseInput(
        $id: Guid!,
        $title: String!,
        $description: String,
        $amount: Decimal!,
        $date: DateTime!,
        $categoryId: Int!,
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
                balanceId: $balanceId
            }
        )
    }
`;

export const UPDATE_PLANNED_EXPENSE = gql`
    mutation UpdatePlannedExpense(
        $id: Guid!,
        $title: String!,
        $description: String,
        $amount: Decimal!,
        $categoryId: Int!,
        $balanceId: ID!,
        $startDate: DateTime!,
        $endDate: DateTime,
        $frequencyId: Int!,
        $isActive: Boolean!
    ) {
        expenses_update_planned_expense(
            id: $id,
            input: {
                title: $title,
                description: $description,
                amount: $amount,
                categoryId: $categoryId,
                balanceId: $balanceId,
                startDate: $startDate,
                endDate: $endDate,
                frequencyId: $frequencyId,
                isActive: $isActive
            }
        )
    }
`;

export const REMOVE_EXPENSE = gql`
    mutation RemoveExpense($id: Guid!) {
        expenses_remove_expense(id: $id)
    }
`;

export const REMOVE_PLANNED_EXPENSE = gql`
    mutation RemovePlannedExpense($id: Guid!) {
        expenses_remove_planned_expense(id: $id)
    }
`;

export const CREATE_USER_PROJECT = gql`
    mutation CreateUserProjectInput($title: String!, $isActive: Boolean!) {
        expenses_create_user_project(input: { title: $title, isActive: $isActive })
    }
`;

export const UPDATE_USER_PROJECT = gql`
    mutation UpdateUserProjectInput($id: Guid!, $title: String!, $isActive: Boolean!) {
        expenses_create_user_project(id: $id, input: { title: $title, isActive: $isActive })
    }
`;

export const CREATE_USER_BALANCE = gql`
    mutation CreateUserBalanceInput(
        $title: String!,
        $isActive: Boolean!,
        $currencyId: Int!,
        $balanceTypeId: Int!
        $userProjectId: Guid!
        $iconId: Int!
    ) {
        expenses_create_user_balance(
            input: {
                title: $title,
                isActive: $isActive,
                currencyId: $currencyId,
                balanceTypeId: $balanceTypeId
                userProjectId: $userProjectId
                iconId: $iconId
            })
    }
`;

export const UPDATE_USER_BALANCE = gql`
    mutation UpdateUserBalanceInput(
        $id: Guid!,
        $title: String!,
        $isActive: Boolean!,
        $currencyId: Int!,
        $balanceTypeId: Int!
        $userProjectId: Guid!
        $iconId: Int!
    ) {
        expenses_update_user_balance(
            id: $id,
            input: {
                title: $title,
                isActive: $isActive,
                currencyId: $currencyId,
                balanceTypeId: $balanceTypeId
                userProjectId: $userProjectId
                iconId: $iconId
            })
    }
`;

export const REMOVE_USER_BALANCE = gql`
    mutation RemovePlannedExpense($id: Guid!) {
        expenses_remove_user_balance(id: $id)
    }
`;

export const CREATE_FAVORITE_EXPENSE = gql`
    mutation CreateFavoriteExpenseInput(
        $title: String!,
        $description: String,
        $limit: Decimal,
        $categoryId: Int,
        $frequencyId: Int,
        $currencyId: Int!,
        $userProjectId: ID!,
        $iconId: Int!
    ) {
        expenses_create_favorite_expense(
            input: {
                title: $title,
                description: $description,
                limit: $limit,
                categoryId: $categoryId,
                frequencyId: $frequencyId,
                currencyId: $currencyId,
                userProjectId: $userProjectId,
                iconId: $iconId
            })
    }
`;

export const UPDATE_FAVORITE_EXPENSE = gql`
    mutation UpdateFavoriteExpenseInput(
        $id: Guid!,
        $title: String!,
        $description: String,
        $limit: Decimal,
        $categoryId: Int,
        $frequencyId: Int,
        $currencyId: Int!,
        $userProjectId: ID!,
        $iconId: Int!
    ) {
        expenses_update_favorite_expense(
            id: $id,
            input: {
                title: $title,
                description: $description,
                limit: $limit,
                categoryId: $categoryId,
                frequencyId: $frequencyId,
                currencyId: $currencyId,
                userProjectId: $userProjectId,
                iconId: $iconId
            })
    }
`;

export const REMOVE_FAVORITE_EXPENSE = gql`
    mutation RemoveFavoriteExpense($id: Guid!) {
        expenses_remove_favorite_expense(id: $id)
    }
`;

export const GET_USER_PROJECT_BY_ID = gql`
    query GetUserProjectById($id: Guid) {
        expenses_get_user_project_by_id(id: $id) {
            id
            title
            isActive
            createdUserId
            balances {
                id
                title
                iconId
                created
                modified
                amount
                currencyId
                userProjectId
                balanceTypeId
                isActive
                version
                userId
            }
            version
            created
            modified
        }
    }
`;

export const GET_FILTERED_USER_PROJECTS = gql`
    query GetFilteredUserProjects(
        $isFull: Boolean,
        $pageNumber: Int,
        $pageSize: Int,
        $dateFrom: DateTime,
        $dateTo: DateTime,
        $column: String,
        $direction: String,
        $query: String,
        $amountFrom: Decimal,
        $amountTo: Decimal
    ) {
        expenses_get_filtered_user_projects(
            isFull: $isFull,
            pageNumber: $pageNumber,
            pageSize: $pageSize,
            dateFrom: $dateFrom,
            dateTo: $dateTo,
            column: $column,
            direction: $direction,
            query: $query,
            amountFrom: $amountFrom,
            amountTo: $amountTo
        ) {
            entities {
                id
                title
                isActive
                createdUserId
                balances {
                    id
                    title
                    iconId
                    created
                    modified
                    amount
                    currencyId
                    userProjectId
                    balanceTypeId
                    isActive
                    version
                    userId
                }
                version
                created
                modified
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

export const GET_FILTERED_USER_ALLOWED_PROJECTS = gql`
    query GetFilteredUserAllowedProjects(
        $isFull: Boolean,
        $pageNumber: Int,
        $pageSize: Int,
        $dateFrom: DateTime,
        $dateTo: DateTime,
        $column: String,
        $direction: String,
        $query: String,
        $amountFrom: Decimal,
        $amountTo: Decimal
    ) {
        expenses_get_filtered_user_allowed_projects(
            isFull: $isFull,
            pageNumber: $pageNumber,
            pageSize: $pageSize,
            dateFrom: $dateFrom,
            dateTo: $dateTo,
            column: $column,
            direction: $direction,
            query: $query,
            amountFrom: $amountFrom,
            amountTo: $amountTo
        ) {
            entities {
                id
                userProjectId
                userProject {
                    id
                    title
                    isActive
                    createdUserId
                    balances {
                        id
                        title
                        iconId
                        created
                        modified
                        amount
                        currencyId
                        userProjectId
                        balanceTypeId
                        isActive
                        version
                        userId
                    }
                    version
                    created
                    modified
                }
                userId
                isReadOnly
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
