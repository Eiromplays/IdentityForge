import { axios, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type DeleteIdentityResourceDTO = {
  identityResourceId: number;
};

export const deleteIdentityResource = ({ identityResourceId }: DeleteIdentityResourceDTO) => {
  return axios.delete(`/identity-resources/${identityResourceId}`);
};

type UseDeleteIdentityResourceOptions = {
  config?: MutationConfig<typeof deleteIdentityResource>;
};

export const useDeleteIdentityResource = ({ config }: UseDeleteIdentityResourceOptions = {}) => {
  return useMutation({
    onSuccess: async () => {
      toast.success('IdentityResource deleted');
    },
    ...config,
    mutationFn: deleteIdentityResource,
  });
};
