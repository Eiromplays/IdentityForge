import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { identityServerUrl } from '@/utils/envVariables';

export const getSend2FaVerificationCode = (): Promise<string[]> => {
  return axios.get(`${identityServerUrl}/api/v1/account/send-2fa-verification-code`);
};

type QueryFnType = typeof getSend2FaVerificationCode;

type UseLogoutOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const useGetSend2FaVerificationCode = ({ config }: UseLogoutOptions = {}) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['get-send-2fa-verification-code'],
    queryFn: () => getSend2FaVerificationCode(),
  });
};
