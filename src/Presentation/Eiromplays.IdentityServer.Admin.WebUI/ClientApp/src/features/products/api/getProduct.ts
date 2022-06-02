import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { ProductDetails } from '../types';

export type GetApiProductDTO = {
  productId: string;
};

export const getProduct = ({ productId }: GetApiProductDTO): Promise<ProductDetails> => {
  return axios.get(`/products/${productId}`);
};

type QueryFnType = typeof getProduct;

type UseProductOptions = {
  productId: string;
  config?: QueryConfig<QueryFnType>;
};

export const useProduct = ({ productId, config }: UseProductOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['product', productId],
    queryFn: () => getProduct({ productId: productId }),
  });
};
