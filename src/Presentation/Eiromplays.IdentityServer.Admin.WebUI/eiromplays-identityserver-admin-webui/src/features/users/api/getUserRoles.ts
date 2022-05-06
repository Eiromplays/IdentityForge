import { useQuery } from 'react-query';
import { axios, QueryConfig, ExtractFnReturnType } from 'eiromplays-ui';

import { UserRole } from '../types';

export type GetUserRolesDTO = {
  userId: string;
};

export const getUserRoles = ({ userId }: GetUserRolesDTO): Promise<UserRole[]> => {
  return axios.get(`/users/${userId}/roles`);
};

type QueryFnType = typeof getUserRoles;

type UseUserRolesOptions = {
  userId: string;
  config?: QueryConfig<QueryFnType>;
};

export const useUserRoles = ({ userId, config }: UseUserRolesOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: [`user-${userId}-roles`],
    queryFn: () => getUserRoles({ userId }),
  });
};
