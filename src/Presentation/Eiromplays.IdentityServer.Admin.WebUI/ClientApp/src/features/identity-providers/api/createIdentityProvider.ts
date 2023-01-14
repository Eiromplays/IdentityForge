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

import { IdentityProvider } from '../types';

export type CreateIdentityProviderDTO = {
  data: {
    scheme: string;
    displayName: string;
    enabled: boolean;
    type: string;
    properties: Record<string, string>;
  };
};

export const createIdentityProvider = async ({
  data,
}: CreateIdentityProviderDTO): Promise<MessageResponse> => {
  return axios.post(`/identity-providers`, data);
};

type UseCreateIdentityProviderOptions = {
  config?: MutationConfig<typeof createIdentityProvider>;
};

export const useCreateIdentityProvider = ({ config }: UseCreateIdentityProviderOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (newIdentityProvider) => {
      await queryClient.cancelQueries(['search-identity-providers']);

      const previousIdentityProviders = queryClient.getQueryData<
        PaginationResponse<IdentityProvider>
      >([
        'search-identity-providers',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      queryClient.setQueryData(
        [
          'search-identity-providers',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ],
        [...(previousIdentityProviders?.data || []), newIdentityProvider.data]
      );

      return { previousIdentityProviders };
    },
    onError: (error, __, context: any) => {
      toast.error('Failed to create IdentityProvider');
      toast.error(error.response?.data);
      if (context?.previousIdentityProviders) {
        queryClient.setQueryData(
          [
            'search-identity-providers',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          context.previousIdentityProviders
        );
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([
        'search-identity-providers',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success('IdentityProvider created');
    },
    ...config,
    mutationFn: createIdentityProvider,
  });
};
