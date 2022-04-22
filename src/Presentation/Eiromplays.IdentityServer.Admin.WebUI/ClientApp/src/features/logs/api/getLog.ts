import { useQuery } from 'react-query';

import { axios } from '@/lib/axios';
import { ExtractFnReturnType, QueryConfig } from '@/lib/react-query';

import { Log } from '../types';

export const getLog = ({ logId }: { logId: string }): Promise<Log> => {
  return axios.get(`/logs/${logId}`);
};

type QueryFnType = typeof getLog;

type UseLogOptions = {
  logId: string;
  config?: QueryConfig<QueryFnType>;
};

export const useLog = ({ logId, config }: UseLogOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['log', logId],
    queryFn: () => getLog({ logId }),
  });
};
