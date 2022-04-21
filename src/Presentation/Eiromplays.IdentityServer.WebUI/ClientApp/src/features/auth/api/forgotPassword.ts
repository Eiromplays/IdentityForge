import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { axios } from '@/lib/axios';
import { MutationConfig } from '@/lib/react-query';
import { MessageResponse } from '@/types';

type ForgotPasswordDTO = {
  email: string;
};

export const forgotPassword = ({ data }: { data: ForgotPasswordDTO }): Promise<MessageResponse> => {
  return axios.post(`/users/forgot-password`, data);
};

type UseForgotPasswordOptions = {
  config?: MutationConfig<typeof forgotPassword>;
};

export const useForgotPassword = ({ config }: UseForgotPasswordOptions = {}) => {
  return useMutation({
    onSuccess: (response) => {
      toast.success(response.message);
    },
    ...config,
    mutationFn: forgotPassword,
  });
};
