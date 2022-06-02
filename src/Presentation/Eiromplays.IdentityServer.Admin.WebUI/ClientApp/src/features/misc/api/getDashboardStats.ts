import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { DashboardStats } from '../types';

export const getDashboardStats = (): Promise<DashboardStats> => {
  return axios.get(`/dashboard/stats`);
};

type QueryFnType = typeof getDashboardStats;

type UseStatsOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const useDashboardStats = ({ config }: UseStatsOptions = {}) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['stats'],
    queryFn: getDashboardStats,
  });
};
