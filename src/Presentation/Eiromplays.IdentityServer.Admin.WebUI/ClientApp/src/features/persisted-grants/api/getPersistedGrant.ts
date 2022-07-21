import { useQuery } from '@tanstack/react-query';
import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';

import { PersistedGrant } from '../types';

export const getPersistedGrant = ({
  persistedGrantKey,
}: {
  persistedGrantKey: string;
}): Promise<PersistedGrant> => {
  return axios.get(`/persisted-grants/${persistedGrantKey}`);
};

type QueryFnType = typeof getPersistedGrant;

type UsePersistedGrantOptions = {
  persistedGrantKey: string;
  config?: QueryConfig<QueryFnType>;
};

export const usePersistedGrant = ({ persistedGrantKey, config }: UsePersistedGrantOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['persisted-grant', persistedGrantKey],
    queryFn: () => getPersistedGrant({ persistedGrantKey: persistedGrantKey }),
  });
};
