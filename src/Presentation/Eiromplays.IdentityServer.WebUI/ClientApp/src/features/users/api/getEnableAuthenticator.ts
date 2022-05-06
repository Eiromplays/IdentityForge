import { useQuery } from 'react-query';

import { axios } from '@/lib/axios';
import { ExtractFnReturnType, QueryConfig } from '@/lib/react-query';

import { EnableAuthenticatorViewModel } from '../types';

export const getEnableAuthenticator = (): Promise<EnableAuthenticatorViewModel> => {
  return axios.get(`https://localhost:7001/account/EnableAuthenticator`);
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