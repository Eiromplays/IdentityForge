import { MutationConfig, queryClient, axios } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

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

export const updateIdentityResource = async ({ identityResourceId, data }: UpdateIdentityResourceDTO) => {
  return axios.put(`/identity-resources/${identityResourceId}`, data);
};

type UseUpdateIdentityResourceOptions = {
  config?: MutationConfig<typeof updateIdentityResource>;
};

export const useUpdateIdentityResource = ({ config }: UseUpdateIdentityResourceOptions = {}) => {
  return useMutation({
    onSuccess: async (_, variables) => {
      await queryClient.invalidateQueries(['identity-resource', variables.identityResourceId]);
      toast.success('IdentityResource Updated');
    },
    onError: (error) => {
      toast.error('Failed to update IdentityResource');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: updateIdentityResource,
  });
};
