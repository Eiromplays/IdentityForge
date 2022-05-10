import { axios, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { EnableAuthenticatorViewModel } from '../types';

export const addAuthenticator = (data: EnableAuthenticatorViewModel): Promise<string[]> => {
  return axios.post(`https://localhost:7001/account/EnableAuthenticator`, data);
};

type UseAddAuthenticatorOptions = {
  config?: MutationConfig<typeof addAuthenticator>;
};

export const useAddAuthenticator = ({ config }: UseAddAuthenticatorOptions = {}) => {
  return useMutation({
    onSuccess: () => {
      toast.success('Authenticator added successfully');
    },
    ...config,
    mutationFn: addAuthenticator,
  });
};
