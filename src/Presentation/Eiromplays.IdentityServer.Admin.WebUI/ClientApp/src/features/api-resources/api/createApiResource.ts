import { useSearch } from '@tanstack/react-location';
import {
  MutationConfig,
  axios,
  MessageResponse,
  queryClient,
  PaginationResponse,
} from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { ApiResource } from '@/features/api-resources';

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
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (newApiResource) => {
      await queryClient.cancelQueries(['search-api-resources']);

      const previousApiResources = queryClient.getQueryData<PaginationResponse<ApiResource>>([
        'search-api-resources',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);

      queryClient.setQueryData(
        ['search-api-resources', pagination?.index ?? 1, pagination?.size ?? 10],
        [...(previousApiResources?.data || []), newApiResource.data]
      );

      return { previousApiResources };
    },
    onError: (error, __, context: any) => {
      toast.error('Failed to create ApiResource');
      toast.error(error.response?.data);
      if (context?.previousApiResources) {
        queryClient.setQueryData(
          ['search-api-resources', pagination?.index ?? 1, pagination?.size ?? 10],
          context.previousApiResources
        );
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([
        'search-api-resources',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);
      toast.success('ApiResource created');
    },
    ...config,
    mutationFn: createApiResource,
  });
};
