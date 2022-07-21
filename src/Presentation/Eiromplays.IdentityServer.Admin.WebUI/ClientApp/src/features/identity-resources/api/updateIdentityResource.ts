import { useMutation } from '@tanstack/react-query';
import { MutationConfig, queryClient, axios } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { IdentityResource } from '@/features/identity-resources';

export type UpdateIdentityResourceDTO = {
  identityResourceId: number;
  data: {
    id: number;
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

export const updateIdentityResource = async ({
  identityResourceId,
  data,
}: UpdateIdentityResourceDTO) => {
  return axios.put(`/identity-resources/${identityResourceId}`, data);
};

type UseUpdateIdentityResourceOptions = {
  config?: MutationConfig<typeof updateIdentityResource>;
};

export const useUpdateIdentityResource = ({ config }: UseUpdateIdentityResourceOptions = {}) => {
  return useMutation({
    onMutate: async (updatingIdentityResource) => {
      await queryClient.cancelQueries([
        'identity-resource',
        updatingIdentityResource?.identityResourceId,
      ]);

      const previousIdentityResource = queryClient.getQueryData<IdentityResource>([
        'identity-resource',
        updatingIdentityResource?.identityResourceId,
      ]);

      queryClient.setQueryData(
        ['identity-resource', updatingIdentityResource?.identityResourceId],
        {
          ...previousIdentityResource,
          ...updatingIdentityResource.data,
          id: updatingIdentityResource.identityResourceId,
        }
      );

      return { previousIdentityResource };
    },
    onError: (error, __, context: any) => {
      toast.error('Failed to update IdentityResource');
      toast.error(error.response?.data);
      if (context?.previousIdentityResource) {
        queryClient.setQueryData(
          ['identity-resource', context.previousIdentityResource.id],
          context.previousIdentityResource
        );
      }
    },
    onSuccess: async (response, variables) => {
      await queryClient.refetchQueries(['identity-resource', variables.identityResourceId]);
      toast.success('IdentityResource Updated');
    },
    ...config,
    mutationFn: updateIdentityResource,
  });
};
