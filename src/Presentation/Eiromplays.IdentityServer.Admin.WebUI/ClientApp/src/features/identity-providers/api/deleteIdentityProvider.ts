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
import { IdentityResource } from '@/features/identity-resources';

export type DeleteIdentityProviderDTO = {
  identityProviderId: number;
};

export const deleteIdentityProvider = ({ identityProviderId }: DeleteIdentityProviderDTO) => {
  return axios.delete(`/identity-providers/${identityProviderId}`);
};

type UseDeleteIdentityProviderOptions = {
  config?: MutationConfig<typeof deleteIdentityProvider>;
};

export const useDeleteIdentityProvider = ({ config }: UseDeleteIdentityProviderOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (deletedIdentityProvider) => {
      await queryClient.cancelQueries(['search-identity-providers']);

      const previousIdentityProviders = queryClient.getQueryData<
        PaginationResponse<IdentityResource>
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
        previousIdentityProviders?.data?.filter(
          (identityProvider) => identityProvider.id !== deletedIdentityProvider.identityProviderId
        )
      );

      return { previousIdentityProviders };
    },
    onError: (_, __, context: any) => {
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
      toast.success('IdentityProvider deleted');
    },
    ...config,
    mutationFn: deleteIdentityProvider,
  });
};
