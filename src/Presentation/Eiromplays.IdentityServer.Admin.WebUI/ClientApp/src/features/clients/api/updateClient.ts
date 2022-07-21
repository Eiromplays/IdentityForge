import { useMutation } from '@tanstack/react-query';
import { MutationConfig, queryClient, axios } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { Client } from '@/features/clients';

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
    onMutate: async (updatingClient) => {
      await queryClient.cancelQueries(['client', updatingClient?.clientId]);

      const previousClient = queryClient.getQueryData<Client>(['client', updatingClient?.clientId]);

      queryClient.setQueryData(['client', updatingClient?.clientId], {
        ...previousClient,
        ...updatingClient.data,
        id: updatingClient.clientId,
      });

      return { previousClient };
    },
    onError: (error, __, context: any) => {
      toast.error('Failed to update client');
      toast.error(error.response?.data);
      if (context?.previousClient) {
        queryClient.setQueryData(['client', context.previousClient.id], context.previousClient);
      }
    },
    onSuccess: async (response, variables) => {
      await queryClient.refetchQueries(['client', variables.clientId]);
      toast.success('Client Updated');
    },
    ...config,
    mutationFn: updateClient,
  });
};
