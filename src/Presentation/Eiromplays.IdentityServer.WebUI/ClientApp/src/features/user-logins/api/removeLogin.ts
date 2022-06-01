import { axios, MutationConfig, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { ExternalLoginsResponse } from '../types';

type RemoveLoginData = {
  loginProvider: string;
  providerKey: string;
};

export const removeLogin = (data: RemoveLoginData) => {
  return axios.post(`/personal/remove-external-login`, data);
};

type UseRemoveLoginOptions = {
  config?: MutationConfig<typeof removeLogin>;
};

export const useRemoveLogin = ({ config }: UseRemoveLoginOptions = {}) => {
  return useMutation({
    onMutate: async (removedExternalLogin) => {
      await queryClient.cancelQueries('external-logins');

      const previousExternalLoginResponse =
        queryClient.getQueryData<ExternalLoginsResponse>('external-logins');

      queryClient.setQueryData(
        'external-logins',
        previousExternalLoginResponse?.currentLogins?.filter(
          (userLogin) => userLogin.providerKey !== removedExternalLogin.providerKey
        )
      );

      return { previousExternalLoginResponse };
    },
    onError: (_, __, context: any) => {
      if (context?.previousExternalLoginResponse) {
        queryClient.setQueryData('external-logins', context.previousExternalLoginResponse);
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries('external-logins');
      toast.success('External Login Removed');
    },
    ...config,
    mutationFn: removeLogin,
  });
};
