import { useSearch } from '@tanstack/react-location';
import { useMutation } from '@tanstack/react-query';
import {
  MutationConfig,
  axios,
  MessageResponse,
  queryClient,
  PaginationResponse,
  defaultPageIndex,
  defaultPageSize,
} from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { Client } from '@/features/clients';

export type CreateClientDTO = {
  data: {
    enabled: boolean;
    clientId: string;
    protocolType: string;
    requireClientSecret: boolean;
    clientName: string;
    description: string;
    clientUri: string;
    logoUri: string;
    requireConsent: boolean;
    allowRememberConsent: boolean;
    alwaysIncludeUserClaimsInIdToken: boolean;
    allowedGrantTypes: string[];
    requirePkce: boolean;
    allowPlainTextPkce: boolean;
    requireRequestObject: boolean;
    allowAccessTokensViaBrowser: boolean;
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
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      queryClient.setQueryData(
        [
          'search-clients',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ],
        [...(previousClients?.data || []), newClient.data]
      );

      return { previousClients };
    },
    onError: (error, __, context: any) => {
      toast.error('Failed to create Client');
      toast.error(error?.response?.data?.message);
      Object.entries(error?.response?.data?.errors || {}).forEach(([, value]) => {
        toast.error(`${value}`, { className: 'break-all' });
      });
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
    onSuccess: async (response) => {
      await queryClient.invalidateQueries([
        'search-clients',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success(response.message);
    },
    ...config,
    mutationFn: createClient,
  });
};
