import { MutationConfig, queryClient, axios } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type UpdateApiResourceDTO = {
  apiResourceId: number;
  data: {
    id: number;
    name: string;
    displayName: string;
    description: string;
    showInDiscoveryDocument: boolean;
    allowedAccessTokenSigningAlgorithms: string;
    enabled: boolean;
    requireResourceIndicator: boolean;
    nonEditable: boolean;
  };
};

export const updateApiResource = async ({ apiResourceId, data }: UpdateApiResourceDTO) => {
  return axios.put(`/api-resources/${apiResourceId}`, data);
};

type UseUpdateApiResourceOptions = {
  config?: MutationConfig<typeof updateApiResource>;
};

export const useUpdateApiResource = ({ config }: UseUpdateApiResourceOptions = {}) => {
  return useMutation({
    onSuccess: async (_, variables) => {
      await queryClient.invalidateQueries(['api-resource', variables.apiResourceId]);
      toast.success('ApiResource Updated');
    },
    onError: (error) => {
      toast.error('Failed to update ApiResource');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: updateApiResource,
  });
};
