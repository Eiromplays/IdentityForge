import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { GrantsViewModel } from '../types';

export const getGrants = (): Promise<GrantsViewModel> => {
  return axios.get(`https://localhost:7001/grants`);
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
