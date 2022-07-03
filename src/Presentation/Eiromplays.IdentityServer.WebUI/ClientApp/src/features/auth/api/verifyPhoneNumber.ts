import { axios, MessageResponse, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { identityServerUrl } from '@/utils/envVariables';

export type VerifyPhoneNumberDTO = {
  userId: string;
  code: string;
};

export const verifyPhoneNumber = (data: VerifyPhoneNumberDTO): Promise<MessageResponse> => {
  return axios.get(
    `${identityServerUrl}/api/v1/account/confirm-phone-number?userId=${data.userId}&code=${data.code}`
  );
};

type UseVerifyPhoneNumberOptions = {
  config?: MutationConfig<typeof verifyPhoneNumber>;
};

export const useVerifyPhoneNumber = ({ config }: UseVerifyPhoneNumberOptions = {}) => {
  return useMutation({
    onSuccess: (response) => {
      alert(response);
      toast.success(response.message);
    },
    ...config,
    mutationFn: verifyPhoneNumber,
  });
};
