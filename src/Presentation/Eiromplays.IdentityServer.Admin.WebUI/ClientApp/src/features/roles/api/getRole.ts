import { useQuery } from '@tanstack/react-query';
import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';

import { Role } from '../types';

export type GetRoleDTO = {
  roleId: string;
};

export const getRole = ({ roleId }: GetRoleDTO): Promise<Role> => {
  return axios.get(`/roles/${roleId}`);
};

export type QueryFnType = typeof getRole;

export type UseRoleOptions = {
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
