import { PaginationFilter } from 'eiromplays-ui';

export type IdentityResource = {
    id: number;
    enabled: boolean;
    name: string;
    displayName: string;
    description: string;
    required: boolean;
    emphasize: boolean;
    showInDiscoveryDocument: boolean;
    userClaims: IdentityResourceClaimDto[];
    properties: IdentityResourcePropertyDto[];
    created: number;
    updated: number;
    nonEditable: boolean;
}

export type IdentityResourceClaimDto = {
    id: number;
    type: string;
    identityResourceId: number;
}

export type IdentityResourcePropertyDto = {
    id: number;
    key: string;
    value: string;
    identityResourceId: number;
}

export type SearchIdentityResourceDTO = PaginationFilter;
