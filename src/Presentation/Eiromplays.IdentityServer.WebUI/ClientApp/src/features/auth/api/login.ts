import { axios } from '@/lib/axios';

import { LoginConsentResponse } from '../types';

export type LoginCredentialsDTO = {
  login: string;
  password: string;
  returnUrl?: string;
};

export const loginWithEmailAndPassword = (
  data: LoginCredentialsDTO
): Promise<LoginConsentResponse> => {
  return axios.post('https://localhost:7001/spa/Login', data, {
    withCredentials: true,
  });
};
