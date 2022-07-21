import { useSearch } from '@tanstack/react-location';
import { useMutation } from '@tanstack/react-query';
import {
  MutationConfig,
  axios,
  MessageResponse,
  queryClient,
  PaginationResponse,
  defaultPageSize,
  defaultPageIndex,
} from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { Role } from '@/features/roles';

export type CreateRoleDTO = {
  data: {
    name: string;
    description: string;
  };
};

export const createRole = async ({ data }: CreateRoleDTO): Promise<MessageResponse> => {
  return axios.post(`/roles`, data);
};

type UseUpdateRoleOptions = {
  config?: MutationConfig<typeof createRole>;
};

export const useCreateRole = ({ config }: UseUpdateRoleOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (newRole) => {
      await queryClient.cancelQueries(['search-roles']);

      const previousRoles = queryClient.getQueryData<PaginationResponse<Role>>([
        'search-roles',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      queryClient.setQueryData(
        [
          'search-roles',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ],
        [...(previousRoles?.data || []), newRole.data]
      );

      return { previousRoles };
    },
    onError: (error, __, context: any) => {
      toast.error('Failed to create role');
      toast.error(error.response?.data);
      if (context?.previousRoles) {
        queryClient.setQueryData(
          [
            'search-roles',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          context.previousRoles
        );
      }
    },
    onSuccess: async (response) => {
      await queryClient.invalidateQueries([
        'search-roles',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success(response.message);
    },
    ...config,
    mutationFn: createRole,
  });
};
