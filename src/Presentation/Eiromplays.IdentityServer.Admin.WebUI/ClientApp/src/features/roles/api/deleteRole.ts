import { useSearch } from '@tanstack/react-location';
import { axios, MutationConfig, PaginationResponse, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { ServerSideSession } from '@/features/sessions';

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
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);

      queryClient.setQueryData(
        ['search-roles', pagination?.index ?? 1, pagination?.size ?? 10],
        previousRoles?.data?.filter((role) => role.id !== deletedRole.roleId)
      );

      return { previousRoles };
    },
    onError: (_, __, context: any) => {
      if (context?.previousRoles) {
        queryClient.setQueryData(
          ['search-roles', pagination?.index ?? 1, pagination?.size ?? 10],
          context.previousRoles
        );
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([
        'search-roles',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);
      toast.success('Role deleted');
    },
    ...config,
    mutationFn: deleteRole,
  });
};
