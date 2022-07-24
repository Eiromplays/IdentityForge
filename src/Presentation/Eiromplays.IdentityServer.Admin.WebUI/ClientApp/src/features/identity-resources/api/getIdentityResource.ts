import { useQuery } from '@tanstack/react-query';
import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';

import { IdentityResource } from '../types';

export type GetIdentityResourceDTO = {
  identityResourceId: number;
};

export const getIdentityResource = ({
  identityResourceId,
}: GetIdentityResourceDTO): Promise<IdentityResource> => {
  return axios.get(`/identity-resources/${identityResourceId}`);
};

export type QueryFnType = typeof getIdentityResource;

export type UseIdentityResourceOptions = {
  identityResourceId: number;
  config?: QueryConfig<QueryFnType>;
};

export const useIdentityResource = ({ identityResourceId, config }: UseIdentityResourceOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['identity-resource', identityResourceId],
    queryFn: () => getIdentityResource({ identityResourceId: identityResourceId }),
  });
};
