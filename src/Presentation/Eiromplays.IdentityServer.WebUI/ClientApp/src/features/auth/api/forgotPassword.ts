import { useMutation } from '@tanstack/react-query';
import { axios, MutationConfig, MessageResponse } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { identityServerUrl } from '@/utils/envVariables';

type ForgotPasswordDTO = {
  email: string;
};

export const forgotPassword = ({ data }: { data: ForgotPasswordDTO }): Promise<MessageResponse> => {
  return axios.post(`${identityServerUrl}/api/v1/account/forgot-password`, data);
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
