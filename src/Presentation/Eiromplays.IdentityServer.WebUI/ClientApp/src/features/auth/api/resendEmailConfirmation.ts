import { useMutation } from '@tanstack/react-query';
import { axios, MutationConfig, MessageResponse } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { identityServerUrl } from '@/utils/envVariables';

export type ResendEmailConfirmationDTO = {
  email: string;
};

export const resendEmailConfirmation = ({
  data,
}: {
  data: ResendEmailConfirmationDTO;
}): Promise<MessageResponse> => {
  return axios.post(`${identityServerUrl}/api/v1/account/resend-email-confirmation`, data);
};

export type UseResendEmailConfirmationOptions = {
  config?: MutationConfig<typeof resendEmailConfirmation>;
};

export const useResendEmailConfirmation = ({ config }: UseResendEmailConfirmationOptions = {}) => {
  return useMutation({
    onSuccess: (response) => {
      toast.success(response.message);
    },
    ...config,
    mutationFn: resendEmailConfirmation,
  });
};
