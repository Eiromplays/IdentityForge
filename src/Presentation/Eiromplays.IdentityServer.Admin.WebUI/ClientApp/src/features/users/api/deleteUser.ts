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
import { User } from '@/features/users';

export type DeleteUserDTO = {
  userId: string;
};

export const deleteUser = ({ userId }: DeleteUserDTO) => {
  return axios.delete(`/users/${userId}`);
};

export type UseDeleteUserOptions = {
  config?: MutationConfig<typeof deleteUser>;
};

export const useDeleteUser = ({ config }: UseDeleteUserOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (deletedUser) => {
      await queryClient.cancelQueries([
        'search-users',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      const previousUsers = queryClient.getQueryData<PaginationResponse<User>>([
        'search-users',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      queryClient.setQueryData(
        [
          'search-users',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ],
        previousUsers?.data?.filter((user) => user.id !== deletedUser.userId)
      );

      return { previousUsers };
    },
    onError: (_, __, context: any) => {
      if (context?.previousUsers) {
        queryClient.setQueryData(
          [
            'search-users',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          context.previousUsers
        );
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([
        'search-users',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success('User deleted');
    },
    ...config,
    mutationFn: deleteUser,
  });
};
