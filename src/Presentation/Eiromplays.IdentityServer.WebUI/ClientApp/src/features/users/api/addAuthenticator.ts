import { axios, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { identityServerUrl } from '@/utils/envVariables';

import { EnableAuthenticatorRequest } from '../types';

export const addAuthenticator = (data: EnableAuthenticatorRequest): Promise<string[]> => {
  return axios.post(`${identityServerUrl}/api/v1/manage/two-factor-authentication/enable`, data);
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
