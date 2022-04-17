import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { useAuth } from '@/lib/auth';
import { axios } from '@/lib/axios';
import { MutationConfig } from '@/lib/react-query';

export type ChangePasswordDTO = {
  data: {
    password: string;
    newPassword: string;
    confirmNewPassword: string;
  };
};

export const changePassword = async ({ data }: ChangePasswordDTO) => {
  return axios.put(`/personal/change-password`, data);
};

type UseChangePasswordOptions = {
  config?: MutationConfig<typeof changePassword>;
};

export const useChangePassword = ({ config }: UseChangePasswordOptions = {}) => {
  const { refetchUser } = useAuth();

  return useMutation({
    onSuccess: async () => {
      toast.success('Password Updated');
      await refetchUser();
    },
    onError: (error) => {
      toast.error('Failed to update password');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: changePassword,
  });
};
