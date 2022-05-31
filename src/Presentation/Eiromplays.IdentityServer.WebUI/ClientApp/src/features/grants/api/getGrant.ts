import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { identityServerUrl } from '@/utils/envVariables';

import { Grant } from '../types';

export const getGrant = ({ clientId }: { clientId: string }): Promise<Grant> => {
  return axios.get(`${identityServerUrl}/grants/${clientId}`);
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
