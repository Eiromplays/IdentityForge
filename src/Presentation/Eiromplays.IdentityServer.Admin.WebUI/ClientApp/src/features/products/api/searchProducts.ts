import { axios, PaginationResponse } from 'eiromplays-ui';

import { Product, SearchProductDTO } from '../types';

export const searchProducts = (data: SearchProductDTO): Promise<PaginationResponse<Product>> => {
  return axios.post('/products/search', data);
};
