import { useMutation } from '@tanstack/react-query';
import { axios, MutationConfig } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { Send2FaVerificationCodeDto, Send2FaVerificationCodeResponse } from '@/features/auth';
import { identityServerUrl } from '@/utils/envVariables';

export const send2FaVerificationCode = ({
  data,
}: {
  data: Send2FaVerificationCodeDto;
  returnUrl?: string;
}): Promise<Send2FaVerificationCodeResponse> => {
  return axios.post(`${identityServerUrl}/api/v1/account/send-2fa-verification-code`, data);
};

type UseSend2FaVerificationCodeOptions = {
  config?: MutationConfig<typeof send2FaVerificationCode>;
};

export const useSend2FaVerificationCode = ({ config }: UseSend2FaVerificationCodeOptions = {}) => {
  return useMutation({
    onSuccess: (response) => {
      toast.success(response.message);
    },
    ...config,
    mutationFn: send2FaVerificationCode,
  });
};
