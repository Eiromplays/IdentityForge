import { useQuery } from 'react-query';

import { axios } from '@/lib/axios';
import { ExtractFnReturnType, QueryConfig } from '@/lib/react-query';

import { PersistedGrant } from '../types';

export const getGrant = ({
  persistedGrantKey,
}: {
  persistedGrantKey: string;
}): Promise<PersistedGrant> => {
  return axios.get(`/persisted-grants/${persistedGrantKey}`);
};

type QueryFnType = typeof getGrant;

type UsePersistedGrantOptions = {
  persistedGrantKey: string;
  config?: QueryConfig<QueryFnType>;
};

export const usePersistedGrant = ({ persistedGrantKey, config }: UsePersistedGrantOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['persisted-grant', persistedGrantKey],
    queryFn: () => getGrant({ persistedGrantKey }),
  });
};
