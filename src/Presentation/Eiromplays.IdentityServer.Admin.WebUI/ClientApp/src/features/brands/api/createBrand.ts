import { MutationConfig, axios, MessageResponse } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type CreateBrandDTO = {
  data: {
    name: string;
    description: string;
  };
};

export const createBrand = async ({ data }: CreateBrandDTO): Promise<MessageResponse> => {
  return axios.post(`/brands`, data);
};

type UseCreateBrandOptions = {
  config?: MutationConfig<typeof createBrand>;
};

export const useCreateBrand = ({ config }: UseCreateBrandOptions = {}) => {
  return useMutation({
    onSuccess: async (response) => {
      toast.success('Brand Created');
      toast.success(response.message);
    },
    onError: (error) => {
      toast.error('Failed to create Brand');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: createBrand,
  });
};
