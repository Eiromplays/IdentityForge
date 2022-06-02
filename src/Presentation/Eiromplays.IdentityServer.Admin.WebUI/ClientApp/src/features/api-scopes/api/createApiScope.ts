import { MutationConfig, axios, MessageResponse } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type CreateApiScopeDTO = {
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

export const createApiScope = async ({ data }: CreateApiScopeDTO): Promise<MessageResponse> => {
  return axios.post(`/api-scopes`, data);
};

type UseCreateApiScopeOptions = {
  config?: MutationConfig<typeof createApiScope>;
};

export const useCreateApiScope = ({ config }: UseCreateApiScopeOptions = {}) => {
  return useMutation({
    onSuccess: async (response) => {
      toast.success('ApiScope Created');
      toast.success(response.message);
    },
    onError: (error) => {
      toast.error('Failed to create ApiScope');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: createApiScope,
  });
};
