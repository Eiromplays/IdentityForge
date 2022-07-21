import { useQuery } from '@tanstack/react-query';
import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';

import { Client } from '../types';

export type GetClientDTO = {
  clientId: number;
};

export const getClient = ({ clientId }: GetClientDTO): Promise<Client> => {
  return axios.get(`/clients/${clientId}`);
};

type QueryFnType = typeof getClient;

type UseClientOptions = {
  clientId: number;
  config?: QueryConfig<QueryFnType>;
};

export const useClient = ({ clientId, config }: UseClientOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['client', clientId],
    queryFn: () => getClient({ clientId: clientId }),
  });
};
