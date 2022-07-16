import { axios, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { identityServerUrl } from '@/utils/envVariables';

export type ResendPhoneNumberConfirmationDTO = {
  phoneNumber: string;
};

export type ResendPhoneNumberConfirmationResponse = {
  message: string;
  returnUrl: string;
};

export const resendPhoneNumberConfirmation = ({
  data,
}: {
  data: ResendPhoneNumberConfirmationDTO;
  returnUrl?: string;
}): Promise<ResendPhoneNumberConfirmationResponse> => {
  return axios.post(`${identityServerUrl}/api/v1/account/resend-phone-number-confirmation`, data);
};

export type UseResendPhoneNumberConfirmationOptions = {
  config?: MutationConfig<typeof resendPhoneNumberConfirmation>;
};

export const useResendPhoneNumberConfirmation = ({
  config,
}: UseResendPhoneNumberConfirmationOptions = {}) => {
  return useMutation({
    onSuccess: (response) => {
      if (response.returnUrl) window.location.href = response.returnUrl;
      toast.success(response.message);
    },
    ...config,
    mutationFn: resendPhoneNumberConfirmation,
  });
};
