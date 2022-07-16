import { MutationConfig, queryClient, axios } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { ApiScope } from '@/features/api-scopes';

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
    onMutate: async (updatingApiScope) => {
      await queryClient.cancelQueries(['api-scope', updatingApiScope?.apiScopeId]);

      const previousApiScope = queryClient.getQueryData<ApiScope>([
        'api-scope',
        updatingApiScope?.apiScopeId,
      ]);

      queryClient.setQueryData(['api-scope', updatingApiScope?.apiScopeId], {
        ...previousApiScope,
        ...updatingApiScope.data,
        id: updatingApiScope.apiScopeId,
      });

      return { previousApiScope };
    },
    onError: (error, __, context: any) => {
      toast.error('Failed to update ApiScope');
      toast.error(error.response?.data);
      if (context?.previousApiScope) {
        queryClient.setQueryData(
          ['api-scope', context.previousApiScope.id],
          context.previousApiScope
        );
      }
    },
    onSuccess: async (response, variables) => {
      await queryClient.refetchQueries(['api-scope', variables.apiScopeId]);
      toast.success('ApiScope Updated');
    },
    ...config,
    mutationFn: updateApiScope,
  });
};
