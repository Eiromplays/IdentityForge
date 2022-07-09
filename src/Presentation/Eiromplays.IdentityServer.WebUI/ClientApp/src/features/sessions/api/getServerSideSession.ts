import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { identityServerUrl } from '@/utils/envVariables';

import { ServerSideSession } from '../types';

export const getServerSideSession = ({ key }: { key: string }): Promise<ServerSideSession> => {
  return axios.get(`${identityServerUrl}/api/v1/manage/user-sessions/${key}`);
};

type QueryFnType = typeof getServerSideSession;

type UseServerSideSessionOptions = {
  key: string;
  config?: QueryConfig<QueryFnType>;
};

export const useServerSideSession = ({ key, config }: UseServerSideSessionOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['server-side-session', key],
    queryFn: () => getServerSideSession({ key }),
  });
};
