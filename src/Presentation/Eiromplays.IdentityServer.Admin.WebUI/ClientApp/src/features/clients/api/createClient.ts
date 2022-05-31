import { MutationConfig, axios, MessageResponse } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type CreateClientDTO = {
  data: {
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

export const createClient = async ({ data }: CreateClientDTO): Promise<MessageResponse> => {
  return axios.post(`/clients`, data);
};

type UseCreateClientOptions = {
  config?: MutationConfig<typeof createClient>;
};

export const useCreateClient = ({ config }: UseCreateClientOptions = {}) => {
  return useMutation({
    onSuccess: async (response) => {
      toast.success('Client created');
      toast.success(response.message);
    },
    onError: (error) => {
      toast.error('Failed to create client');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: createClient,
  });
};
