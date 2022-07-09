import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { UserSession } from '../types';

export const getBffUserSession = ({ key }: { key: string }): Promise<UserSession> => {
  return axios.get(`/user-sessions/${key}`);
};

type QueryFnType = typeof getBffUserSession;

type UseBffUserSessionOptions = {
  key: string;
  config?: QueryConfig<QueryFnType>;
};

export const useBffUserSession = ({ key, config }: UseBffUserSessionOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['bff-user-session', key],
    queryFn: () => getBffUserSession({ key }),
  });
};
