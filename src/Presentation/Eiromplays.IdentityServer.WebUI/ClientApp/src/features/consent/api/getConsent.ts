import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { ConsentViewModel } from '../types';

export const getConsent = ({ returnUrl }: { returnUrl?: string }): Promise<ConsentViewModel> => {
  return axios.get(`/consent?returnUrl=${returnUrl}`);
};

type QueryFnType = typeof getConsent;
type UseConsentOptions = {
  returnUrl?: string;
  config?: QueryConfig<QueryFnType>;
};

export const useConsent = ({ returnUrl, config }: UseConsentOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['consent'],
    queryFn: () => getConsent({ returnUrl }),
  });
};
