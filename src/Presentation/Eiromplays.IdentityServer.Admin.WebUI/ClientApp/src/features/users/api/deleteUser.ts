import { useSearch } from '@tanstack/react-location';
import { axios, MutationConfig, PaginationResponse, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { User } from '@/features/users';

export type DeleteUserDTO = {
  userId: string;
};

export const deleteUser = ({ userId }: DeleteUserDTO) => {
  return axios.delete(`/users/${userId}`);
};

type UseDeleteUserOptions = {
  config?: MutationConfig<typeof deleteUser>;
};

export const useDeleteUser = ({ config }: UseDeleteUserOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (deletedUser) => {
      await queryClient.cancelQueries(['search-users']);

      const previousUsers = queryClient.getQueryData<PaginationResponse<User>>([
        'search-roles',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);

      queryClient.setQueryData(
        ['search-users', pagination?.index ?? 1, pagination?.size ?? 10],
        previousUsers?.data?.filter((user) => user.id !== deletedUser.userId)
      );

      return { previousUsers };
    },
    onError: (_, __, context: any) => {
      if (context?.previousUsers) {
        queryClient.setQueryData(
          ['search-users', pagination?.index ?? 1, pagination?.size ?? 10],
          context.previousUsers
        );
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([
        'search-users',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);
      toast.success('User deleted');
    },
    ...config,
    mutationFn: deleteUser,
  });
};
