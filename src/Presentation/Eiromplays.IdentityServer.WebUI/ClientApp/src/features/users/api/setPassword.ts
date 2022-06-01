import { useAuth, axios, MutationConfig, MessageResponse } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type SetPasswordDTO = {
  data: {
    password: string;
    confirmPassword: string;
  };
};

export const changePassword = async ({ data }: SetPasswordDTO): Promise<MessageResponse> => {
  return axios.put(`/personal/set-password`, data);
};

type UseSetPasswordOptions = {
  config?: MutationConfig<typeof changePassword>;
};

export const useSetPassword = ({ config }: UseSetPasswordOptions = {}) => {
  const { refetchUser } = useAuth();

  return useMutation({
    onSuccess: async (response) => {
      toast.success('Password set');
      toast.success(response.message);
      await refetchUser();
    },
    onError: (error) => {
      toast.error('Failed to set password');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: changePassword,
  });
};
