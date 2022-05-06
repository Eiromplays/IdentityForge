import { useQuery } from 'react-query';

import { ExtractFnReturnType, QueryConfig, axios } from 'eiromplays-ui';

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
