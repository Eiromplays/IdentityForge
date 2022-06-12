import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { identityServerUrl } from '@/utils/envVariables';

import { Grant } from '../types';

export const getGrants = (): Promise<Grant[]> => {
  return axios.get(`${identityServerUrl}/api/v1/grants`);
};

type QueryFnType = typeof getGrants;

type useGrantsOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const useGrants = ({ config }: useGrantsOptions = {}) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['grants'],
    queryFn: () => getGrants(),
  });
};
