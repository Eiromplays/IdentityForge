import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { axios, MutationConfig, queryClient } from 'eiromplays-ui';

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
  return useMutation({
    onMutate: async (deletedUser) => {
      await queryClient.cancelQueries('roles');

      const previousRoles = queryClient.getQueryData<Role[]>('roles');

      queryClient.setQueryData(
        'roles',
        previousRoles?.filter((discussion) => discussion.id !== deletedUser.roleId)
      );

      return { previousRoles };
    },
    onError: (_, __, context: any) => {
      if (context?.previousRoles) {
        queryClient.setQueryData('roles', context.previousRoles);
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries('roles');
      toast.success('Role deleted');
    },
    ...config,
    mutationFn: deleteRole,
  });
};
