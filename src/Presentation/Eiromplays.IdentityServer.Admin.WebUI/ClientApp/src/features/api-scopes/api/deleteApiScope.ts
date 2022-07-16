import { useSearch } from '@tanstack/react-location';
import { axios, MutationConfig, PaginationResponse, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { ApiScope } from '@/features/api-scopes';

export type DeleteApiScopeDTO = {
  apiScopeId: number;
};

export const deleteApiScope = ({ apiScopeId }: DeleteApiScopeDTO) => {
  return axios.delete(`/api-scopes/${apiScopeId}`);
};

type UseDeleteApiScopeOptions = {
  config?: MutationConfig<typeof deleteApiScope>;
};

export const useDeleteApiScope = ({ config }: UseDeleteApiScopeOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (deletedApiScope) => {
      await queryClient.cancelQueries(['search-api-scopes']);

      const previousApiScopes = queryClient.getQueryData<PaginationResponse<ApiScope>>([
        'search-api-scopes',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);

      queryClient.setQueryData(
        ['search-api-scopes', pagination?.index ?? 1, pagination?.size ?? 10],
        previousApiScopes?.data?.filter((apiScope) => apiScope.id !== deletedApiScope.apiScopeId)
      );

      return { previousApiScopes };
    },
    onError: (_, __, context: any) => {
      if (context?.previousApiScopes) {
        queryClient.setQueryData(
          ['search-api-scopes', pagination?.index ?? 1, pagination?.size ?? 10],
          context.previousApiScopes
        );
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([
        'search-api-scopes',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);
      toast.success('ApiScope deleted');
    },
    ...config,
    mutationFn: deleteApiScope,
  });
};
