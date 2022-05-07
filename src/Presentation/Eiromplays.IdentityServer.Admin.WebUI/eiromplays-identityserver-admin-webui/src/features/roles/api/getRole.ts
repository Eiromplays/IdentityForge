import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { Role } from '../types';

export type GetRoleDTO = {
  roleId: string;
};

export const getRole = ({ roleId }: GetRoleDTO): Promise<Role> => {
  return axios.get(`/roles/${roleId}`);
};

type QueryFnType = typeof getRole;

type UseRoleOptions = {
  roleId: string;
  config?: QueryConfig<QueryFnType>;
};

export const useRole = ({ roleId, config }: UseRoleOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['role', roleId],
    queryFn: () => getRole({ roleId }),
  });
};
