import { useMutation } from '@tanstack/react-query';
import { axios, MutationConfig, queryClient } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { UserRole } from '@/features/users';

export type UpdateUserRolesDTO = {
  userId: string;
  data: {
    userRoles: UserRole[];
    revokeUserSessions?: boolean;
  };
};

export const updateUserRoles = async ({ userId, data }: UpdateUserRolesDTO) => {
  return axios.post(`/users/${userId}/roles`, { UserRolesRequest: data });
};

type UseUpdateUserRolesOptions = {
  config?: MutationConfig<typeof updateUserRoles>;
};

export const useUpdateUserRoles = ({ config }: UseUpdateUserRolesOptions = {}) => {
  return useMutation({
    onMutate: async (updatingUserRoles) => {
      await queryClient.cancelQueries(['user', updatingUserRoles.userId, 'roles']);

      const previousUserRoles = queryClient.getQueryData<UserRole[]>([
        'user',
        updatingUserRoles.userId,
        'roles',
      ]);

      queryClient.setQueryData(
        ['user', updatingUserRoles.userId, 'roles'],
        [...(updatingUserRoles?.data?.userRoles || [])]
      );

      return { previousUserRoles };
    },
    onError: (error, variables, context: any) => {
      toast.error('Failed to update user roles');
      toast.error(error.response?.data);
      if (context?.previousUserRoles) {
        queryClient.setQueryData(['user', variables.userId, 'roles'], context.previousUserRoles);
      }
    },
    onSuccess: async (_, variables) => {
      await queryClient.refetchQueries(['user', variables.userId, 'roles']);
      toast.success(`${variables.userId}'s roles has been updated successfully`);
    },
    ...config,
    mutationFn: updateUserRoles,
  });
};
