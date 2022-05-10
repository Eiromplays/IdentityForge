import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { Log } from '../types';

export const getLogs = (): Promise<Log[]> => {
  return axios.get(`/logs`);
};

type QueryFnType = typeof getLogs;

type useLogsOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const useLogs = ({ config }: useLogsOptions = {}) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['logs'],
    queryFn: () => getLogs(),
  });
};
