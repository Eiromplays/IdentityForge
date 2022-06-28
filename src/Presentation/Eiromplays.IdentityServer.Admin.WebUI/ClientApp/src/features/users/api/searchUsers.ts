import { axios, PaginationFilter, PaginationResponse } from 'eiromplays-ui';

import { User } from '../types';

export type SearchUserDTO = PaginationFilter & {
  isActive?: boolean;
};

export const searchUsers = (data: SearchUserDTO): Promise<PaginationResponse<User>> => {
  return axios.post('/users/search', data);
};
