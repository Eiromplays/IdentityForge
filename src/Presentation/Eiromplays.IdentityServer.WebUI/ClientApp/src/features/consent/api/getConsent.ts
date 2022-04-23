import { useQuery } from 'react-query';

import { axios } from '@/lib/axios';
import { ExtractFnReturnType, QueryConfig } from '@/lib/react-query';

import { ConsentViewModel } from '../types';

export const getConsent = ({ returnUrl }: { returnUrl?: string }): Promise<ConsentViewModel> => {
  return axios.get(`https://localhost:7001/consent?returnUrl=${returnUrl}`);
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
