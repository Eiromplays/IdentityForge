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
import { IdentityResource } from '@/features/identity-resources';

export type CreateIdentityResourceDTO = {
  data: {
    name: string;
    displayName: string;
    description: string;
    showInDiscoveryDocument: boolean;
    emphasize: boolean;
    enabled: boolean;
    required: boolean;
    nonEditable: boolean;
  };
};

export const createIdentityResource = async ({
  data,
}: CreateIdentityResourceDTO): Promise<MessageResponse> => {
  return axios.post(`/identity-resources`, data);
};

type UseCreateIdentityResourceOptions = {
  config?: MutationConfig<typeof createIdentityResource>;
};

export const useCreateIdentityResource = ({ config }: UseCreateIdentityResourceOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (newIdentityResource) => {
      await queryClient.cancelQueries(['search-identity-resources']);

      const previousIdentityResources = queryClient.getQueryData<
        PaginationResponse<IdentityResource>
      >(['search-identity-resources', pagination?.index ?? 1, pagination?.size ?? 10]);

      queryClient.setQueryData(
        ['search-identity-resources', pagination?.index ?? 1, pagination?.size ?? 10],
        [...(previousIdentityResources?.data || []), newIdentityResource.data]
      );

      return { previousIdentityResources };
    },
    onError: (error, __, context: any) => {
      toast.error('Failed to create IdentityResource');
      toast.error(error.response?.data);
      if (context?.previousIdentityResources) {
        queryClient.setQueryData(
          ['search-identity-resources', pagination?.index ?? 1, pagination?.size ?? 10],
          context.previousIdentityResources
        );
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([
        'search-identity-resources',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);
      toast.success('IdentityResource created');
    },
    ...config,
    mutationFn: createIdentityResource,
  });
};
