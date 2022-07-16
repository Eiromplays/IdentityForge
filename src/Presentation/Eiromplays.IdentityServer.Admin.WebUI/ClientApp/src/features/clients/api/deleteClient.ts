import { useSearch } from '@tanstack/react-location';
import { axios, MutationConfig, PaginationResponse, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { Client } from '@/features/clients';

export type DeleteClientDTO = {
  clientId: number;
};

export const deleteClient = ({ clientId }: DeleteClientDTO) => {
  return axios.delete(`/clients/${clientId}`);
};

type UseDeleteClientOptions = {
  config?: MutationConfig<typeof deleteClient>;
};

export const useDeleteClient = ({ config }: UseDeleteClientOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (deletedClient) => {
      await queryClient.cancelQueries(['search-clients']);

      const previousClients = queryClient.getQueryData<PaginationResponse<Client>>([
        'search-clients',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);

      queryClient.setQueryData(
        ['search-clients', pagination?.index ?? 1, pagination?.size ?? 10],
        previousClients?.data?.filter((client) => client.id !== deletedClient.clientId)
      );

      return { previousClients };
    },
    onError: (_, __, context: any) => {
      if (context?.previousClients) {
        queryClient.setQueryData(
          ['search-clients', pagination?.index ?? 1, pagination?.size ?? 10],
          context.previousClients
        );
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([
        'search-clients',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);
      toast.success('Client deleted');
    },
    ...config,
    mutationFn: deleteClient,
  });
};
