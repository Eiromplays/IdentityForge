import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { UserSession } from '../types';

export const getUserSession = ({ key }: { key: string }): Promise<UserSession> => {
  return axios.get(`/user-sessions/${key}`);
};

type QueryFnType = typeof getUserSession;

type UseUserSessionOptions = {
  key: string;
  config?: QueryConfig<QueryFnType>;
};

export const useUserSession = ({ key, config }: UseUserSessionOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['user-session', key],
    queryFn: () => getUserSession({ key }),
  });
};
