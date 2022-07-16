import { useSearch } from '@tanstack/react-location';
import { axios, MutationConfig, PaginationResponse, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { ApiResource } from '@/features/api-resources';

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
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (deletedApiResource) => {
      await queryClient.cancelQueries(['search-api-resources']);

      const previousApiResources = queryClient.getQueryData<PaginationResponse<ApiResource>>([
        'search-api-resources',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);

      queryClient.setQueryData(
        ['search-api-resources', pagination?.index ?? 1, pagination?.size ?? 10],
        previousApiResources?.data?.filter(
          (apiResource) => apiResource.id !== deletedApiResource.apiResourceId
        )
      );

      return { previousApiResources };
    },
    onError: (_, __, context: any) => {
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
      toast.success('ApiResource deleted');
    },
    ...config,
    mutationFn: deleteApiResource,
  });
};
