import { useQuery } from 'react-query';

import { axios } from '@/lib/axios';
import { ExtractFnReturnType, QueryConfig } from '@/lib/react-query';

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
