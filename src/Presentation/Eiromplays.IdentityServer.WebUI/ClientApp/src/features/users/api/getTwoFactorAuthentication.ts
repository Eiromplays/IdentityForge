import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { identityServerUrl } from '@/utils/envVariables';

import { TwoFactorAuthenticationViewModel } from '../types';

export const getTwoFactorAuthentication = (): Promise<TwoFactorAuthenticationViewModel> => {
  return axios.get(`${identityServerUrl}/api/v1/account/TwoFactorAuthentication`);
};

type QueryFnType = typeof getTwoFactorAuthentication;

type UseTwoFactorAuthenticationOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const useTwoFactorAuthentication = ({ config }: UseTwoFactorAuthenticationOptions = {}) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['two-factor-authentication'],
    queryFn: () => getTwoFactorAuthentication(),
  });
};
