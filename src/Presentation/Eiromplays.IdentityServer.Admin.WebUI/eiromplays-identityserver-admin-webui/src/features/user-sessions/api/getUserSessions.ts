import { useQuery } from 'react-query';

import { axios } from '@/lib/axios';
import { ExtractFnReturnType, QueryConfig } from '@/lib/react-query';

import { UserSession } from '../types';

export const getUserSessions = (): Promise<UserSession[]> => {
  return axios.get(`/user-sessions`);
};

type QueryFnType = typeof getUserSessions;

type useUserSessionOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const useUserSessions = ({ config }: useUserSessionOptions = {}) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['user-sessions'],
    queryFn: () => getUserSessions(),
  });
};
