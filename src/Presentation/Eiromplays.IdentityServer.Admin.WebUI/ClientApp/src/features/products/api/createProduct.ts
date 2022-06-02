import { MutationConfig, axios, MessageResponse } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type CreateProductDTO = {
  data: {
    name: string;
    description: string;
    rate: number;
    brandId: string;
  };
};

export const createProduct = async ({ data }: CreateProductDTO): Promise<MessageResponse> => {
  return axios.post(`/products`, data);
};

type UseCreateProductOptions = {
  config?: MutationConfig<typeof createProduct>;
};

export const useCreateProduct = ({ config }: UseCreateProductOptions = {}) => {
  return useMutation({
    onSuccess: async (response) => {
      toast.success('Product Created');
      toast.success(response.message);
    },
    onError: (error) => {
      toast.error('Failed to create Product');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: createProduct,
  });
};
