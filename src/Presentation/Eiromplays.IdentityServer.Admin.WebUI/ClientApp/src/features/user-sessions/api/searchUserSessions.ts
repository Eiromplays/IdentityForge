import {
  axios,
  ExtractFnReturnType,
  QueryConfig,
  PaginationFilter,
  PaginationResponse,
} from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { UserSession } from '../types';

export type SearchUserSessionDTO = PaginationFilter;

export const searchUserSessions = (
  data: SearchUserSessionDTO
): Promise<PaginationResponse<UserSession>> => {
  return axios.post('/user-sessions/search', data);
};

type QueryFnType = typeof searchUserSessions;

type UseSearchUserSessionsOptions = {
  data: SearchUserSessionDTO;
  config?: QueryConfig<QueryFnType>;
};

export const useSearchUserSessions = ({ data, config }: UseSearchUserSessionsOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['search-user-sessions', data.pageNumber],
    queryFn: () => searchUserSessions(data),
  });
};
