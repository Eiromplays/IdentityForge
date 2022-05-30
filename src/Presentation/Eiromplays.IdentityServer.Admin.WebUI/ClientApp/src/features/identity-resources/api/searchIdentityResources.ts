import { axios, PaginationResponse } from 'eiromplays-ui';

import { IdentityResource, SearchIdentityResourceDTO } from '../types';

export const searchIdentityResources = (
  data: SearchIdentityResourceDTO
): Promise<PaginationResponse<IdentityResource>> => {
  return axios.post('/identity-resources/search', data);
};
