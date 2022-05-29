import { MutationConfig, queryClient, axios } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type UpdateClientDTO = {
  data: {
    id: string;
    name: string;
    description: string;
  };
};

export const updateClient = async ({ data }: UpdateClientDTO) => {
  return axios.put(`/clients/${data.id}`, data);
};

type UseUpdateClientOptions = {
  config?: MutationConfig<typeof updateClient>;
};

export const useUpdateClient = ({ config }: UseUpdateClientOptions = {}) => {
  return useMutation({
    onSuccess: async () => {
      toast.success('Client Updated');
    },
    onError: (error) => {
      toast.error('Failed to update client');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: updateClient,
  });
};
