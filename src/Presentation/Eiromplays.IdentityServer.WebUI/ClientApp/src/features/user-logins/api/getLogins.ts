import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { ExternalLoginsResponse } from '../types';

export const getLogins = (): Promise<ExternalLoginsResponse> => {
  return axios.get(`/personal/external-logins`);
};

type QueryFnType = typeof getLogins;

type useUserLoginOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const useUserLogins = ({ config }: useUserLoginOptions = {}) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['external-logins'],
    queryFn: () => getLogins(),
  });
};
