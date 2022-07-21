import { useQuery } from '@tanstack/react-query';
import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';

import { Log } from '../types';

export const getLogs = (): Promise<Log[]> => {
  return axios.get(`/personal/logs`);
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
