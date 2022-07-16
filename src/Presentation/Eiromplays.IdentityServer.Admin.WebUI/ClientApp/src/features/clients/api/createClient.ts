import { useSearch } from '@tanstack/react-location';
import {
  MutationConfig,
  axios,
  MessageResponse,
  queryClient,
  PaginationResponse,
} from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { Client } from '@/features/clients';

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
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (newClient) => {
      await queryClient.cancelQueries(['search-clients']);

      const previousClients = queryClient.getQueryData<PaginationResponse<Client>>([
        'search-clients',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);

      queryClient.setQueryData(
        ['search-clients', pagination?.index ?? 1, pagination?.size ?? 10],
        [...(previousClients?.data || []), newClient.data]
      );

      return { previousClients };
    },
    onError: (error, __, context: any) => {
      toast.error('Failed to create Client');
      toast.error(error.response?.data);
      if (context?.previousClients) {
        queryClient.setQueryData(
          ['search-clients', pagination?.index ?? 1, pagination?.size ?? 10],
          context.previousClients
        );
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([
        'search-api-clients',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);
      toast.success('Client created');
    },
    ...config,
    mutationFn: createClient,
  });
};
