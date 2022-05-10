import { axios, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export const disableAuthenticator = () => {
  return axios.post(`https://localhost:7001/account/DisableAuthenticator`);
};

type UseDisableAuthenticatorOptions = {
  config?: MutationConfig<typeof disableAuthenticator>;
};

export const useDisableAuthenticator = ({ config }: UseDisableAuthenticatorOptions = {}) => {
  return useMutation({
    onSuccess: () => {
      toast.success('Authenticator disabled successfully');
    },
    ...config,
    mutationFn: disableAuthenticator,
  });
};
