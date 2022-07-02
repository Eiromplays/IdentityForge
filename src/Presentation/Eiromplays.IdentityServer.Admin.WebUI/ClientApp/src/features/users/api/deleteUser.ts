import { useSearch } from '@tanstack/react-location';
import { axios, MutationConfig, PaginationResponse, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';

import { User } from '../types';

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
    onError: (_, __, context: any) => {
      if (context?.previousUsers) {
        queryClient.setQueryData(
          ['search-users', pagination?.index ?? 1, pagination?.size ?? 10],
          context.previousUsers
        );
      }
    },
    onSuccess: async () => {
      await queryClient.refetchQueries([
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
