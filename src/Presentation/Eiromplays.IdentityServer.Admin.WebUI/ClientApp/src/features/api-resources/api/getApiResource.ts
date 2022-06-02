import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { ApiResource } from '../types';

export type GetApiResourceDTO = {
  apiResourceId: number;
};

export const getApiResource = ({ apiResourceId }: GetApiResourceDTO): Promise<ApiResource> => {
  return axios.get(`/api-resources/${apiResourceId}`);
};

type QueryFnType = typeof getApiResource;

type UseApiResourceOptions = {
  apiResourceId: number;
  config?: QueryConfig<QueryFnType>;
};

export const useApiResource = ({ apiResourceId, config }: UseApiResourceOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['api-resource', apiResourceId],
    queryFn: () => getApiResource({ apiResourceId: apiResourceId }),
  });
};
