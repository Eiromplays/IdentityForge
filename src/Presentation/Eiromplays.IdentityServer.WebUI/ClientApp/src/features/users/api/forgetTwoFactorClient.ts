import { useMutation } from '@tanstack/react-query';
import { axios, MutationConfig, queryClient } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { identityServerUrl } from '@/utils/envVariables';

export const forgetTwoFactorClient = () => {
  return axios.post(`${identityServerUrl}/api/v1/manage/two-factor-authentication/forget`);
};

type UseForgetTwoFactorClientOptions = {
  config?: MutationConfig<typeof forgetTwoFactorClient>;
};

export const useForgetTwoFactorClient = ({ config }: UseForgetTwoFactorClientOptions = {}) => {
  return useMutation({
    onSuccess: async () => {
      await queryClient.invalidateQueries(['two-factor-authentication']);
      toast.success(
        'The current browser has been forgotten. Next time you login from this browser you will be prompted for a 2fa code.'
      );
    },
    ...config,
    mutationFn: forgetTwoFactorClient,
  });
};
