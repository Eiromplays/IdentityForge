import {
  axios,
  ExtractFnReturnType,
  QueryConfig,
  PaginationFilter,
  PaginationResponse,
} from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { PersistedGrant } from '../types';

export type SearchPersistedGrantDTO = PaginationFilter;

export const searchPersistedGrants = (
  data: SearchPersistedGrantDTO
): Promise<PaginationResponse<PersistedGrant>> => {
  return axios.post('/persisted-grants/search', data);
};

type QueryFnType = typeof searchPersistedGrants;

type UseSearchPersistedGrantsOptions = {
  data: SearchPersistedGrantDTO;
  config?: QueryConfig<QueryFnType>;
};

export const useSearchPersistedGrants = ({ data, config }: UseSearchPersistedGrantsOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['search-persisted-grants', data.pageNumber],
    queryFn: () => searchPersistedGrants(data),
  });
};
