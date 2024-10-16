export const apolloEnvironments = {
    authGateway: 'authGateway',
    localizations: 'localizations',
    expenses: 'expenses',
    dictionaries: 'dictionaries',
    auditTrail: 'auditTrail'
};

export const apolloFetchPolicy = {
    cacheFirst: 'cache-first',
    cacheAndNetwork: 'cache-and-network',
    networkOnly: 'network-only',
    cacheOnly: 'cache-only',
    noCache: 'no-cache',
    standby: 'standby'
};

export function handleIntId(id: string | number | undefined): number | undefined {
    return id && id !== '0' && id !== 0 ? Number(id) : undefined;
}