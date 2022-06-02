import { MutationConfig, queryClient, axios } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type UpdateProductDTO = {
  productId: string;
  data: {
    id: string;
    name: string;
    description: string;
    rate: number;
    brandId: string;
    deleteCurrentImage: boolean;
  };
};

export const updateProduct = async ({ productId, data }: UpdateProductDTO) => {
  return axios.put(`/products/${productId}`, data);
};

type UseUpdateProductOptions = {
  config?: MutationConfig<typeof updateProduct>;
};

export const useUpdateProduct = ({ config }: UseUpdateProductOptions = {}) => {
  return useMutation({
    onSuccess: async (_, variables) => {
      await queryClient.invalidateQueries(['product', variables.productId]);
      toast.success('Product Updated');
    },
    onError: (error) => {
      toast.error('Failed to update Product');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: updateProduct,
  });
};
