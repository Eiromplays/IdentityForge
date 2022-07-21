import { useMutation } from '@tanstack/react-query';
import { MutationConfig, queryClient, axios } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { ApiResource } from '@/features/api-resources';

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

export type UseUpdateApiResourceOptions = {
  config?: MutationConfig<typeof updateApiResource>;
};

export const useUpdateApiResource = ({ config }: UseUpdateApiResourceOptions = {}) => {
  return useMutation({
    onMutate: async (updatingApiResource) => {
      await queryClient.cancelQueries(['api-resource', updatingApiResource?.apiResourceId]);

      const previousApiResource = queryClient.getQueryData<ApiResource>([
        'api-resource',
        updatingApiResource?.apiResourceId,
      ]);

      queryClient.setQueryData(['api-resource', updatingApiResource?.apiResourceId], {
        ...previousApiResource,
        ...updatingApiResource.data,
        id: updatingApiResource.apiResourceId,
      });

      return { previousApiResource };
    },
    onError: (error, __, context: any) => {
      toast.error('Failed to update ApiResource');
      toast.error(error.response?.data);
      if (context?.previousApiResource) {
        queryClient.setQueryData(
          ['api-resource', context.previousApiResource.id],
          context.previousApiResource
        );
      }
    },
    onSuccess: async (response, variables) => {
      await queryClient.refetchQueries(['api-resource', variables.apiResourceId]);
      toast.success('ApiResource Updated');
    },
    ...config,
    mutationFn: updateApiResource,
  });
};
