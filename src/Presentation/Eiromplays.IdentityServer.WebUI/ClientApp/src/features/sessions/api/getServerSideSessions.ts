import { useQuery } from '@tanstack/react-query';
import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';

import { identityServerUrl } from '@/utils/envVariables';

import { ServerSideSessionsQueryResult } from '../types';

export const getServerSideSessions = (): Promise<ServerSideSessionsQueryResult> => {
  return axios.get(`${identityServerUrl}/api/v1/manage/user-sessions`);
};

type QueryFnType = typeof getServerSideSessions;

type UseServerSideSessionOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const useServerSideSessions = ({ config }: UseServerSideSessionOptions = {}) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['server-side-sessions'],
    queryFn: () => getServerSideSessions(),
  });
};
