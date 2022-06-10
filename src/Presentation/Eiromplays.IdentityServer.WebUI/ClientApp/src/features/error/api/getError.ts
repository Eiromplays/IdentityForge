import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { identityServerUrl } from '@/utils/envVariables';

import { ErrorMessage } from '../types';

export const getError = ({ errorId }: { errorId?: string }): Promise<ErrorMessage> => {
  return axios.get(`${identityServerUrl}/api/v1/error?errorId=${errorId}`);
};

type QueryFnType = typeof getError;

type UseErrorOptions = {
  errorId?: string;
  config?: QueryConfig<QueryFnType>;
};

export const useError = ({ errorId, config }: UseErrorOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['error'],
    queryFn: () => getError({ errorId }),
  });
};
