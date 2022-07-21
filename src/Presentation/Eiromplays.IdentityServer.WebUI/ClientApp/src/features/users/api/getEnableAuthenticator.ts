import { useQuery } from '@tanstack/react-query';
import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';

import { identityServerUrl } from '@/utils/envVariables';

import { GetEnableAuthenticatorResponse } from '../types';

export const getEnableAuthenticator = (): Promise<GetEnableAuthenticatorResponse> => {
  return axios.get(`${identityServerUrl}/api/v1/manage/two-factor-authentication/enable`);
};

type QueryFnType = typeof getEnableAuthenticator;

type UseEnableAuthenticatorOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const useEnableAuthenticator = ({ config }: UseEnableAuthenticatorOptions = {}) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['get-enable-authenticator'],
    queryFn: () => getEnableAuthenticator(),
  });
};
