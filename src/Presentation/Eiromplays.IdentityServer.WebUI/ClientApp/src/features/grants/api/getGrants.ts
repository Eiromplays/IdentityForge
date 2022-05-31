import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { identityServerUrl } from '@/utils/envVariables';

import { GrantsViewModel } from '../types';

export const getGrants = (): Promise<GrantsViewModel> => {
  return axios.get(`${identityServerUrl}/grants`);
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
