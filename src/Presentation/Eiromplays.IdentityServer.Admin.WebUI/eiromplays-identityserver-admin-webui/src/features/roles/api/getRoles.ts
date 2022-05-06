import { useQuery } from 'react-query';

import { ExtractFnReturnType, QueryConfig, axios } from 'eiromplays-ui';

import { Role } from '../types';

export const getRoles = (): Promise<Role[]> => {
  return axios.get(`/roles`);
};

type QueryFnType = typeof getRoles;

type UseRolesOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const useRoles = ({ config }: UseRolesOptions = {}) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['roles'],
    queryFn: () => getRoles(),
  });
};
