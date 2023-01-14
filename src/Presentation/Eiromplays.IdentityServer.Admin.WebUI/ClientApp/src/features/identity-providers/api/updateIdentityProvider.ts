import { useMutation } from '@tanstack/react-query';
import { MutationConfig, queryClient, axios } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { IdentityResource } from '@/features/identity-resources';

export type UpdateIdentityProviderDTO = {
  identityProviderId: number;
  data: {
    id: number;
    scheme: string;
    displayName: string;
    enabled: boolean;
    type: string;
    properties: Record<string, string>;
  };
};

export const updateIdentityProvider = async ({
  identityProviderId,
  data,
}: UpdateIdentityProviderDTO) => {
  return axios.put(`/identity-providers/${identityProviderId}`, data);
};

type UseUpdateIdentityProviderOptions = {
  config?: MutationConfig<typeof updateIdentityProvider>;
};

export const useUpdateIdentityProvider = ({ config }: UseUpdateIdentityProviderOptions = {}) => {
  return useMutation({
    onMutate: async (updatingIdentityProvider) => {
      await queryClient.cancelQueries([
        'identity-provider',
        updatingIdentityProvider?.identityProviderId,
      ]);

      const previousIdentityProvider = queryClient.getQueryData<IdentityResource>([
        'identity-provider',
        updatingIdentityProvider?.identityProviderId,
      ]);

      queryClient.setQueryData(
        ['identity-provider', updatingIdentityProvider?.identityProviderId],
        {
          ...previousIdentityProvider,
          ...updatingIdentityProvider.data,
          id: updatingIdentityProvider.identityProviderId,
        }
      );

      return { previousIdentityProvider };
    },
    onError: (error, __, context: any) => {
      toast.error('Failed to update IdentityProvider');
      toast.error(error.response?.data);
      if (context?.previousIdentityProvider) {
        queryClient.setQueryData(
          ['identity-provider', context.previousIdentityProvider.id],
          context.previousIdentityProvider
        );
      }
    },
    onSuccess: async (response, variables) => {
      await queryClient.refetchQueries(['identity-provider', variables.identityProviderId]);
      toast.success('IdentityProvider Updated');
    },
    ...config,
    mutationFn: updateIdentityProvider,
  });
};
