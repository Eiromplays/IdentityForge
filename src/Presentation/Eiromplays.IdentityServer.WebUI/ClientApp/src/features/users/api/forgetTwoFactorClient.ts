import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { axios } from '@/lib/axios';
import { MutationConfig } from '@/lib/react-query';

export const forgetTwoFactorClient = () => {
  return axios.post(`https://localhost:7001/account/ForgetTwoFactorClient`);
};

type UseForgetTwoFactorClientOptions = {
  config?: MutationConfig<typeof forgetTwoFactorClient>;
};

export const useForgetTwoFactorClient = ({ config }: UseForgetTwoFactorClientOptions = {}) => {
  return useMutation({
    onSuccess: () => {
      toast.success(
        'The current browser has been forgotten. Next time you login from this browser you will be prompted for a 2fa code.'
      );
    },
    ...config,
    mutationFn: forgetTwoFactorClient,
  });
};
