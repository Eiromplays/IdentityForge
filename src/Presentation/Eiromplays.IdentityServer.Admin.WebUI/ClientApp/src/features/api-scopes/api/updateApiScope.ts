import { MutationConfig, queryClient, axios } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type UpdateApiScopeDTO = {
  apiScopeId: number;
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

export const updateApiScope = async ({ apiScopeId, data }: UpdateApiScopeDTO) => {
  return axios.put(`/api-scopes/${apiScopeId}`, data);
};

type UseUpdateApiScopeOptions = {
  config?: MutationConfig<typeof updateApiScope>;
};

export const useUpdateApiScope = ({ config }: UseUpdateApiScopeOptions = {}) => {
  return useMutation({
    onSuccess: async (_, variables) => {
      await queryClient.invalidateQueries(['api-scope', variables.apiScopeId]);
      toast.success('ApiScope Updated');
    },
    onError: (error) => {
      toast.error('Failed to update ApiScope');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: updateApiScope,
  });
};
