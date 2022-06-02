import { axios, PaginationResponse } from 'eiromplays-ui';

import { Client, SearchClientDTO } from '../types';

export const searchClients = (data: SearchClientDTO): Promise<PaginationResponse<Client>> => {
  return axios.post('/clients/search', data);
};
