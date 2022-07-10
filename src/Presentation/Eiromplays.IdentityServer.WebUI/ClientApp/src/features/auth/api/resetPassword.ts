import { axios, MutationConfig, MessageResponse } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { identityServerUrl } from '@/utils/envVariables';

export type ResetPasswordDTO = {
  email: string;
  password: string;
  token: string;
};

export const resetPassword = ({ data }: { data: ResetPasswordDTO }): Promise<MessageResponse> => {
  return axios.post(`${identityServerUrl}/api/v1/account/reset-password`, data);
};

type UseResetPasswordOptions = {
  config?: MutationConfig<typeof resetPassword>;
};

export const useResetPassword = ({ config }: UseResetPasswordOptions = {}) => {
  return useMutation({
    onSuccess: (response) => {
      toast.success(response.message);
    },
    ...config,
    mutationFn: resetPassword,
  });
};
