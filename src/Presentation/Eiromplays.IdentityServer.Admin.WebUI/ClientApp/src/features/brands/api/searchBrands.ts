import { axios, PaginationResponse } from 'eiromplays-ui';

import { Brand, SearchBrandDTO } from '../types';

export const searchBrands = (data: SearchBrandDTO): Promise<PaginationResponse<Brand>> => {
  return axios.post('/brands/search', data);
};
