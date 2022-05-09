import {
  axios,
  ExtractFnReturnType,
  QueryConfig,
  PaginationFilter,
  PaginationResponse,
} from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { Role } from '../types';

export type SearchRoleDTO = PaginationFilter;

export const searchRoles = (data: SearchRoleDTO): Promise<PaginationResponse<Role>> => {
  return axios.post('/roles/search', data);
};

type QueryFnType = typeof searchRoles;

type UseSearchRolesOptions = {
  data: SearchRoleDTO;
  config?: QueryConfig<QueryFnType>;
};

export const useSearchRoles = ({ data, config }: UseSearchRolesOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['search-roles', data.pageNumber],
    queryFn: () => searchRoles(data),
  });
};
