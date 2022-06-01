import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { identityServerUrl } from '@/utils/envVariables';

import { LoginViewModel } from '../types';

export const getLogin = ({ returnUrl }: { returnUrl?: string }): Promise<LoginViewModel> => {
  return axios.get(
    `${identityServerUrl}/spa/login?returnUrl=${encodeURIComponent(returnUrl || '')}`
  );
};

type QueryFnType = typeof getLogin;

type UseLoginOptions = {
  returnUrl?: string;
  config?: QueryConfig<QueryFnType>;
};

export const useLogin = ({ returnUrl, config }: UseLoginOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['login'],
    queryFn: () => getLogin({ returnUrl: returnUrl }),
  });
};
