import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { UserSession } from '../types';

export const getUserSessions = (): Promise<UserSession[]> => {
  return axios.get(`/personal/user-sessions`);
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
