import { PaginationFilter } from 'eiromplays-ui';

export type ApiScope = {
  id: number;
  enabled: boolean;
  name: string;
  displayName: string;
  description: string;
  required: boolean;
  emphasize: boolean;
  showInDiscoveryDocument: boolean;
  userClaims: ApiScopeClaimDto[];
  properties: ApiScopePropertyDto[];
  created: number;
  updated: number;
  lastAccessed: number;
  nonEditable: boolean;
};

export type ApiScopeClaimDto = {
  id: number;
  type: string;
  scopeId: number;
};

export type ApiScopePropertyDto = {
  id: number;
  key: string;
  value: string;
  scopeId: number;
};

export type SearchApiScopeDTO = PaginationFilter;
