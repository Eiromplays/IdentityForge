import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { Brand } from '../types';

export type GetBrandDTO = {
  brandId: string;
};

export const getBrand = ({ brandId }: GetBrandDTO): Promise<Brand> => {
  return axios.get(`/brands/${brandId}`);
};

type QueryFnType = typeof getBrand;

type UseBrandOptions = {
  brandId: string;
  config?: QueryConfig<QueryFnType>;
};

export const useBrand = ({ brandId, config }: UseBrandOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['brand', brandId],
    queryFn: () => getBrand({ brandId: brandId }),
  });
};
