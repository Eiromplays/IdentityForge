import { axios, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type DeleteClientDTO = {
  identityResourceId: number;
};

export const deleteIdentityResource = ({ identityResourceId }: DeleteClientDTO) => {
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
