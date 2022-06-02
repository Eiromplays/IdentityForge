import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { identityServerUrl } from '@/utils/envVariables';

import { EnableAuthenticatorViewModel } from '../types';

export const getEnableAuthenticator = (): Promise<EnableAuthenticatorViewModel> => {
  return axios.get(`${identityServerUrl}/account/EnableAuthenticator`);
};

type QueryFnType = typeof getEnableAuthenticator;

type UseEnableAuthenticatorOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const useEnableAuthenticator = ({ config }: UseEnableAuthenticatorOptions = {}) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['enable-authenticator'],
    queryFn: () => getEnableAuthenticator(),
  });
};
