import { axios, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type DeleteProductDTO = {
  productId: string;
};

export const deleteProduct = ({ productId }: DeleteProductDTO) => {
  return axios.delete(`/products/${productId}`);
};

type UseDeleteProductOptions = {
  config?: MutationConfig<typeof deleteProduct>;
};

export const useDeleteProduct = ({ config }: UseDeleteProductOptions = {}) => {
  return useMutation({
    onSuccess: async () => {
      toast.success('Product deleted');
    },
    ...config,
    mutationFn: deleteProduct,
  });
};
