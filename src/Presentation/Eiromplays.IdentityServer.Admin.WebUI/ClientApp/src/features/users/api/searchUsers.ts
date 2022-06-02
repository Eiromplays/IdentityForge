import {
  axios,
  ExtractFnReturnType,
  QueryConfig,
  PaginationFilter,
  PaginationResponse,
} from 'eiromplays-ui';
import { useQuery } from 'react-query';

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
    queryKey: ['search-users', data.pageNumber],
    queryFn: () => searchUsers(data),
  });
};
