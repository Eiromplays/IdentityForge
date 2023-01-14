import { PaginationFilter } from 'eiromplays-ui';

export type IdentityProvider = {
  id: number;
  scheme: string;
  displayName: string;
  enabled: boolean;
  type: string;
  properties: Record<string, string>;
};

export type SearchIdentityProviderDTO = PaginationFilter;
