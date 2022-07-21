import { useMutation } from '@tanstack/react-query';
import { axios, MessageResponse, MutationConfig } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { identityServerUrl } from '@/utils/envVariables';

import { RegisterDto } from '../components/RegisterForm';

export type ExternalLoginConfirmationResponse = MessageResponse & {
  returnUrl: string;
};

export const externalLoginConfirmation = ({
  returnUrl,
  data,
}: {
  data: RegisterDto;
  returnUrl?: string;
}): Promise<ExternalLoginConfirmationResponse> => {
  return axios.post(
    `${identityServerUrl}/api/v1/account/external-logins/confirmation?returnUrl=${returnUrl}`,
    data
  );
};

type UseExternalLoginConfirmationOptions = {
  config?: MutationConfig<typeof externalLoginConfirmation>;
};

export const useExternalLoginConfirmation = ({
  config,
}: UseExternalLoginConfirmationOptions = {}) => {
  return useMutation({
    onSuccess: (response) => {
      toast.success('Successfully created account');
      toast.success(response.message);
    },
    ...config,
    mutationFn: externalLoginConfirmation,
  });
};
