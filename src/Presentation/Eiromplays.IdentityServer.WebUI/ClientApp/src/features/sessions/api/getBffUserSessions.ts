import { useQuery } from '@tanstack/react-query';
import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';

import { UserSession } from '../types';

export const getBffUserSessions = (): Promise<UserSession[]> => {
  return axios.get(`/personal/user-sessions`);
};

type QueryFnType = typeof getBffUserSessions;

type UseBffUserSessionOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const useBffUserSessions = ({ config }: UseBffUserSessionOptions = {}) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['bff-sessions'],
    queryFn: () => getBffUserSessions(),
  });
};
