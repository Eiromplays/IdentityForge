import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { IdentityResource } from '../types';

export type GetIdentityResourceDTO = {
  identityResourceId: number;
};

export const getIdentityResource = ({
  identityResourceId,
}: GetIdentityResourceDTO): Promise<IdentityResource> => {
  return axios.get(`/identity-resources/${identityResourceId}`);
};

type QueryFnType = typeof getIdentityResource;

type UseIdentityResourceOptions = {
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
