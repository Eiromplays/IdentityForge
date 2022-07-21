import { useSearch } from '@tanstack/react-location';
import { useMutation } from '@tanstack/react-query';
import {
  axios,
  defaultPageIndex,
  defaultPageSize,
  MessageResponse,
  MutationConfig,
  PaginationResponse,
  queryClient,
} from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { User } from '@/features/users';

export type CreateUserDTO = {
  data: {
    username: string;
    firstName: string;
    lastName: string;
    email: string;
    gravatarEmail: string;
    password: string;
    confirmPassword: string;
    phoneNumber: string;
  };
};

export const createUser = async ({ data }: CreateUserDTO): Promise<MessageResponse> => {
  return axios.post(`/users`, data);
};

type UseCreateUserOptions = {
  config?: MutationConfig<typeof createUser>;
};

export const useCreateUser = ({ config }: UseCreateUserOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (newUser) => {
      await queryClient.cancelQueries(['search-users']);

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
        [...(previousUsers?.data || []), newUser.data]
      );

      return { previousUsers };
    },
    onError: (error, __, context: any) => {
      toast.error('Failed to create user');
      toast.error(error.response?.data);
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
    onSuccess: async (response) => {
      await queryClient.invalidateQueries([
        'search-users',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success('User created');
      toast.success(response.message);
    },
    ...config,
    mutationFn: createUser,
  });
};
