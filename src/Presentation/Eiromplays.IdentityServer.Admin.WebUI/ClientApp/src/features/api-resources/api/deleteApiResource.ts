import { axios, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type DeleteIdentityResourceDTO = {
  apiResourceId: number;
};

export const deleteApiResource = ({ apiResourceId }: DeleteIdentityResourceDTO) => {
  return axios.delete(`/api-resources/${apiResourceId}`);
};

type UseDeleteApiResourceResourceOptions = {
  config?: MutationConfig<typeof deleteApiResource>;
};

export const useDeleteApiResource = ({ config }: UseDeleteApiResourceResourceOptions = {}) => {
  return useMutation({
    onSuccess: async () => {
      toast.success('ApiResource deleted');
    },
    ...config,
    mutationFn: deleteApiResource,
  });
};
