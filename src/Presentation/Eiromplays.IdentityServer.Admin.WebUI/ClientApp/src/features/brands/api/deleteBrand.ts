import { axios, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type DeleteBrandDTO = {
  brandId: string;
};

export const deleteBrand = ({ brandId }: DeleteBrandDTO) => {
  return axios.delete(`/brands/${brandId}`);
};

type UseDeleteBrandOptions = {
  config?: MutationConfig<typeof deleteBrand>;
};

export const useDeleteBrand = ({ config }: UseDeleteBrandOptions = {}) => {
  return useMutation({
    onSuccess: async () => {
      toast.success('Brand deleted');
    },
    ...config,
    mutationFn: deleteBrand,
  });
};
