import { useSearch } from '@tanstack/react-location';
import { useMutation } from '@tanstack/react-query';
import {
  axios,
  defaultPageIndex,
  defaultPageSize,
  MutationConfig,
  PaginationResponse,
  queryClient,
} from 'eiromplays-ui';
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
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      queryClient.setQueryData(
        [
          'search-clients',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ],
        previousClients?.data?.filter((client) => client.id !== deletedClient.clientId)
      );

      return { previousClients };
    },
    onError: (_, __, context: any) => {
      if (context?.previousClients) {
        queryClient.setQueryData(
          [
            'search-clients',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          context.previousClients
        );
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([
        'search-clients',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success('Client deleted');
    },
    ...config,
    mutationFn: deleteClient,
  });
};
