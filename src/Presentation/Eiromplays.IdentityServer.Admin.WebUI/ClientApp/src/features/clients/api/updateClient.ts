import { MutationConfig, queryClient, axios } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type UpdateClientDTO = {
  clientId: number;
  data: {
    id: number;
    clientId: string;
    clientName: string;
    description: string;
    clientUri: string;
    logoUri: string;
    enabled: boolean;
    requireConsent: boolean;
    allowRememberConsent: boolean;
  };
};

export const updateClient = async ({ clientId, data }: UpdateClientDTO) => {
  return axios.put(`/clients/${clientId}`, data);
};

type UseUpdateClientOptions = {
  config?: MutationConfig<typeof updateClient>;
};

export const useUpdateClient = ({ config }: UseUpdateClientOptions = {}) => {
  return useMutation({
    onSuccess: async (_, variables) => {
      await queryClient.invalidateQueries(['client', variables.clientId]);
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
