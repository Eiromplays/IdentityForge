import { axios, PaginationResponse } from 'eiromplays-ui';

import { ApiScope, SearchApiScopeDTO } from '../types';

export const searchApiScopes = (data: SearchApiScopeDTO): Promise<PaginationResponse<ApiScope>> => {
  return axios.post('/api-scopes/search', data);
};
