import { useMutation } from '@tanstack/react-query';
import { axios, MessageResponse, MutationConfig } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { identityServerUrl } from '@/utils/envVariables';

export type ConfirmPhoneNumberDTO = {
  userId: string;
  code: string;
};

export type ConfirmPhoneNumberResponse = MessageResponse & {
  returnUrl: string;
};

export const confirmPhoneNumber = (
  data: ConfirmPhoneNumberDTO
): Promise<ConfirmPhoneNumberResponse> => {
  return axios.get(
    `${identityServerUrl}/api/v1/account/confirm-phone-number?userId=${data.userId}&code=${data.code}`
  );
};

type UseConfirmPhoneNumberOptions = {
  config?: MutationConfig<typeof confirmPhoneNumber>;
};

export const useConfirmPhoneNUmber = ({ config }: UseConfirmPhoneNumberOptions = {}) => {
  return useMutation({
    onSuccess: (response) => {
      toast.success(response.message);

      if (response.returnUrl) {
        window.location.href = response.returnUrl;
      }
    },
    ...config,
    mutationFn: confirmPhoneNumber,
  });
};
