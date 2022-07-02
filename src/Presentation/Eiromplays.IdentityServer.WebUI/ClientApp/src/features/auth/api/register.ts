import { axios } from 'eiromplays-ui';

import { identityServerUrl } from '@/utils/envVariables';

export type RegisterCredentialsDTO = {
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  password: string;
  confirmPassword: string;
};

export type RegisterResponse = {
  message: string;
};

export const registerWithEmailAndPassword = (
  data: RegisterCredentialsDTO
): Promise<RegisterResponse> => {
  return axios.post(`${identityServerUrl}/api/v1/account/register`, data);
};
