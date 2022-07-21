import { useSearch } from '@tanstack/react-location';
import { useMutation } from '@tanstack/react-query';
import {
  axios,
  defaultPageIndex,
  defaultPageSize,
  MutationConfig,
  PaginationResponse,
  queryClient,
} from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';

import { Role } from '../types';

export type DeleteRoleDTO = {
  roleId: string;
};

export const deleteRole = ({ roleId }: DeleteRoleDTO) => {
  return axios.delete(`/roles/${roleId}`);
};

type UseDeleteRoleOptions = {
  config?: MutationConfig<typeof deleteRole>;
};

export const useDeleteRole = ({ config }: UseDeleteRoleOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (deletedRole) => {
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
        previousRoles?.data?.filter((role) => role.id !== deletedRole.roleId)
      );

      return { previousRoles };
    },
    onError: (_, __, context: any) => {
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
    onSuccess: async () => {
      await queryClient.invalidateQueries([
        'search-roles',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success('Role deleted');
    },
    ...config,
    mutationFn: deleteRole,
  });
};
