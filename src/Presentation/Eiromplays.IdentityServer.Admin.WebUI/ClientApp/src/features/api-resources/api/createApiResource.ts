import { MutationConfig, axios, MessageResponse } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type CreateApiResourceDTO = {
  data: {
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

export const createApiResource = async ({
  data,
}: CreateApiResourceDTO): Promise<MessageResponse> => {
  return axios.post(`/api-resources`, data);
};

type UseCreateApiResourceOptions = {
  config?: MutationConfig<typeof createApiResource>;
};

export const useCreateApiResource = ({ config }: UseCreateApiResourceOptions = {}) => {
  return useMutation({
    onSuccess: async (response) => {
      toast.success('ApiResource Created');
      toast.success(response.message);
    },
    onError: (error) => {
      toast.error('Failed to create ApiResource');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: createApiResource,
  });
};
