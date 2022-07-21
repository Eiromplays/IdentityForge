import { useSearch } from '@tanstack/react-location';
import { useMutation } from '@tanstack/react-query';
import {
  MutationConfig,
  axios,
  MessageResponse,
  queryClient,
  PaginationResponse,
  defaultPageIndex,
  defaultPageSize,
} from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { ApiScope } from '@/features/api-scopes';

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
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (newApiScope) => {
      await queryClient.cancelQueries(['search-api-scopes']);

      const previousApiScopes = queryClient.getQueryData<PaginationResponse<ApiScope>>([
        'search-api-scopes',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      queryClient.setQueryData(
        [
          'search-api-scopes',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ],
        [...(previousApiScopes?.data || []), newApiScope.data]
      );

      return { previousApiScopes };
    },
    onError: (error, __, context: any) => {
      toast.error('Failed to create ApiScope');
      toast.error(error.response?.data);
      if (context?.previousApiScopes) {
        queryClient.setQueryData(
          [
            'search-api-scopes',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          context.previousApiScopes
        );
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([
        'search-api-scopes',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success('ApiScope created');
    },
    ...config,
    mutationFn: createApiScope,
  });
};
