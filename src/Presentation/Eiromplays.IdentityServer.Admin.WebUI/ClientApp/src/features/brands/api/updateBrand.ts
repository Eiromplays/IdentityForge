import { MutationConfig, queryClient, axios } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type UpdateBrandDTO = {
  brandId: string;
  data: {
    id: string;
    name: string;
    description: string;
  };
};

export const updateBrand = async ({ brandId, data }: UpdateBrandDTO) => {
  return axios.put(`/brands/${brandId}`, data);
};

type UseUpdateBrandOptions = {
  config?: MutationConfig<typeof updateBrand>;
};

export const useUpdateBrand = ({ config }: UseUpdateBrandOptions = {}) => {
  return useMutation({
    onSuccess: async (_, variables) => {
      await queryClient.invalidateQueries(['brand', variables.brandId]);
      toast.success('Brand Updated');
    },
    onError: (error) => {
      toast.error('Failed to update Brand');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: updateBrand,
  });
};
