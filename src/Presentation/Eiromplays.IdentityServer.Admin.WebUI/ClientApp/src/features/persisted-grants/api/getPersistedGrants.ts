import { useQuery } from '@tanstack/react-query';
import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';

import { PersistedGrant } from '../types';

export const getPersistedGrants = (): Promise<PersistedGrant[]> => {
  return axios.get(`/persisted-grants`);
};

type QueryFnType = typeof getPersistedGrants;

type usePersistedGrantsOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const usePersistedGrants = ({ config }: usePersistedGrantsOptions = {}) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['persisted-grants'],
    queryFn: () => getPersistedGrants(),
  });
};
