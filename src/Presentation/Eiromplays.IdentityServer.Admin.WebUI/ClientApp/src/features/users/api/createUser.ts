import { axios, MessageResponse, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

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
  return useMutation({
    onSuccess: async (response) => {
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
