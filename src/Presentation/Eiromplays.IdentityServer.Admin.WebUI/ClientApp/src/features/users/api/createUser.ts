import { useSearch } from '@tanstack/react-location';
import { axios, MessageResponse, MutationConfig, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';

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
    onSuccess: async (response) => {
      await queryClient.refetchQueries([
        'search-users',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);
      toast.success('User Created');
      toast.success(response.message);
    },
    onError: (error) => {
      toast.error('Failed to create user');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: createUser,
  });
};
