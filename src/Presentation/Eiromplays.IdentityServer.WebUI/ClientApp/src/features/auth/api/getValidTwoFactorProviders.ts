import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { identityServerUrl } from '@/utils/envVariables';

export const getValidTwoFactorProviders = (): Promise<string[]> => {
  return axios.get(`${identityServerUrl}/api/v1/account/valid-two-factor-providers`);
};

type QueryFnType = typeof getValidTwoFactorProviders;

type UseGetValidTwoFactorProvidersOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const useGetValidTwoFactorProviders = ({
  config,
}: UseGetValidTwoFactorProvidersOptions = {}) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['valid-two-factor-providers'],
    queryFn: () => getValidTwoFactorProviders(),
  });
};
