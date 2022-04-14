import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { axios } from '@/lib/axios';
import { MutationConfig } from '@/lib/react-query';

import { EnableAuthenticatorViewModel } from '../types';

export const addAuthenticator = (data: EnableAuthenticatorViewModel): Promise<any> => {
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
