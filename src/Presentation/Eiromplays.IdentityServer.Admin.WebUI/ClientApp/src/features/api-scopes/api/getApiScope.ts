import { useQuery } from '@tanstack/react-query';
import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';

import { ApiScope } from '../types';

export type GetIdentityResourceDTO = {
  apiScopeId: number;
};

export const getApiScope = ({ apiScopeId }: GetIdentityResourceDTO): Promise<ApiScope> => {
  return axios.get(`/api-scopes/${apiScopeId}`);
};

type QueryFnType = typeof getApiScope;

type UseIdentityResourceOptions = {
  apiScopeId: number;
  config?: QueryConfig<QueryFnType>;
};

export const useApiScope = ({ apiScopeId, config }: UseIdentityResourceOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['api-scope', apiScopeId],
    queryFn: () => getApiScope({ apiScopeId: apiScopeId }),
  });
};
