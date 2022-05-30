import { axios, PaginationResponse } from 'eiromplays-ui';

import { ApiResource, SearchApiResourceDTO } from '../types';

export const searchApiResources = (
  data: SearchApiResourceDTO
): Promise<PaginationResponse<ApiResource>> => {
  return axios.post('/api-resources/search', data);
};
