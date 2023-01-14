import { useQuery } from '@tanstack/react-query';
import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';

import { IdentityProvider } from '../types';

export type GetIdentityProviderDTO = {
  identityProviderId: number;
};

export const getIdentityProvider = ({
  identityProviderId,
}: GetIdentityProviderDTO): Promise<IdentityProvider> => {
  return axios.get(`/identity-providers/${identityProviderId}`);
};

export type QueryFnType = typeof getIdentityProvider;

export type UseIdentityProviderOptions = {
  identityProviderId: number;
  config?: QueryConfig<QueryFnType>;
};

export const useIdentityProvider = ({ identityProviderId, config }: UseIdentityProviderOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['identity-provider', identityProviderId],
    queryFn: () => getIdentityProvider({ identityProviderId: identityProviderId }),
  });
};
