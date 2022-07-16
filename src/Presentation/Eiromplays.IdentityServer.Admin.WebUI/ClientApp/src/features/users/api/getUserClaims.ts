import { axios, QueryConfig, ExtractFnReturnType } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { UserClaim, UserRole } from '../types';

export type GetUserClaimsDTO = {
  userId: string;
};

export const getUserClaims = ({ userId }: GetUserClaimsDTO): Promise<UserClaim[]> => {
  return axios.get(`/users/${userId}/claims`);
};

export type QueryFnType = typeof getUserClaims;

export type UseUserClaimsOptions = {
  userId: string;
  config?: QueryConfig<QueryFnType>;
};

export const useUserClaims = ({ userId, config }: UseUserClaimsOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['user', userId, 'claims'],
    queryFn: () => getUserClaims({ userId }),
  });
};
