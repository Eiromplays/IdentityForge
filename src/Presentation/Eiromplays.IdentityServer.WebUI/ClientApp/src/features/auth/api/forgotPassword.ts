import { axios, MutationConfig, MessageResponse } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

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
