import { PaginationFilter } from 'eiromplays-ui';

export type ApiResource = {
  id: number;
  enabled: boolean;
  name: string;
  displayName: string;
  description: string;
  allowedAccessTokenSigningAlgorithms: string;
  showInDiscoveryDocument: boolean;
  requireResourceIndicator: boolean;
  secrets: ApiResourceSecretDto[];
  scopes: ApiResourceScopeDto[];
  userClaims: ApiResourceClaimDto[];
  properties: ApiResourcePropertyDto[];
  created: number;
  updated: number;
  nonEditable: boolean;
};

export type ApiResourceClaimDto = {
  id: number;
  type: string;
  apiResourceId: number;
};

export type ApiResourcePropertyDto = {
  id: number;
  key: string;
  value: string;
  apiResourceId: number;
};

export type ApiResourceSecretDto = {
  id: number;
  description: string;
  value: string;
  expiration: number;
  type: string;
  created: number;
  apiResourceId: number;
};

export type ApiResourceScopeDto = {
  id: number;
  scope: string;
  apiResourceId: number;
};

export type SearchApiResourceDTO = PaginationFilter;
