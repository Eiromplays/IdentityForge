import { axios, MutationConfig, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { UserRole } from '@/features/users';

export type UpdateUserRolesDTO = {
  userId: string;
  data: {
    userRoles: UserRole[];
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
    onSuccess: async (_, variables) => {
      await queryClient.refetchQueries(['user', variables.userId, 'roles']);
      toast.success(`${variables.userId}'s roles has been updated successfully`);
    },
    onError: (error) => {
      toast.error('Failed to update user roles');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: updateUserRoles,
  });
};
