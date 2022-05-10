import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { LoginViewModel } from '../types';

export const getLogin = ({ returnUrl }: { returnUrl?: string }): Promise<LoginViewModel> => {
  return axios.get(`https://localhost:7001/spa/login?returnUrl=${returnUrl}`);
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
    queryFn: () => getLogin({ returnUrl }),
  });
};
