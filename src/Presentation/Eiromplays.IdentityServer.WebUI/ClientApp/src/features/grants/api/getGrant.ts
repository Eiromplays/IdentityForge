import { useQuery } from 'react-query';

import { axios } from '@/lib/axios';
import { ExtractFnReturnType, QueryConfig } from '@/lib/react-query';

import { Grant } from '../types';

export const getGrant = ({ clientId }: { clientId: string }): Promise<Grant> => {
  return axios.get(`https://localhost:7001/grants/${clientId}`);
};

type QueryFnType = typeof getGrant;

type UseGrantOptions = {
  clientId: string;
  config?: QueryConfig<QueryFnType>;
};

export const useGrant = ({ clientId, config }: UseGrantOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['grant', clientId],
    queryFn: () => getGrant({ clientId }),
  });
};
