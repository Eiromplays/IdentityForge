import { axios, MutationConfig, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { identityServerUrl } from '@/utils/envVariables';

export const disableAuthenticator = () => {
  return axios.post(`${identityServerUrl}/api/v1/manage/two-factor-authentication/disable`);
};

type UseDisableAuthenticatorOptions = {
  config?: MutationConfig<typeof disableAuthenticator>;
};

export const useDisableAuthenticator = ({ config }: UseDisableAuthenticatorOptions = {}) => {
  return useMutation({
    onSuccess: async () => {
      await queryClient.invalidateQueries(['two-factor-authentication']);
      toast.success('Authenticator disabled successfully');
    },
    ...config,
    mutationFn: disableAuthenticator,
  });
};
