import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { axios } from '@/lib/axios';
import { MutationConfig } from '@/lib/react-query';

export const removeAuthenticator = () => {
  return axios.post(`https://localhost:7001/account/DisableAuthenticator`);
};

type UseRemoveAuthenticatorOptions = {
  config?: MutationConfig<typeof removeAuthenticator>;
};

export const useRemoveAuthenticator = ({ config }: UseRemoveAuthenticatorOptions = {}) => {
  return useMutation({
    onSuccess: () => {
      toast.success('Authenticator removed successfully');
    },
    ...config,
    mutationFn: removeAuthenticator,
  });
};
