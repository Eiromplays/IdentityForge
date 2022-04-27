import { useQuery } from 'react-query';

import { axios } from '@/lib/axios';
import { ExtractFnReturnType, QueryConfig } from '@/lib/react-query';
import { PaginationFilter, PaginationResponse } from '@/types';

import { User } from '../types';

export type SearchUserDTO = PaginationFilter & {
  isActive?: boolean;
};

export const searchUsers = (data: SearchUserDTO): Promise<PaginationResponse<User>> => {
  return axios.post('/users/search', data);
};

type QueryFnType = typeof searchUsers;

type UseSearchUsersOptions = {
  data: SearchUserDTO;
  config?: QueryConfig<QueryFnType>;
};

export const useSearchUsers = ({ data, config }: UseSearchUsersOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['search-users'],
    queryFn: () => searchUsers(data),
  });
};
