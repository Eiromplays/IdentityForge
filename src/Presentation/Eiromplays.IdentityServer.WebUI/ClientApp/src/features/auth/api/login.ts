import { axios } from '@/lib/axios';

import { LoginConsentResponse } from '../types';

export type LoginCredentialsDTO = {
  login: string;
  password: string;
  returnUrl?: string;
  rememberMe?: boolean;
};

export const loginWithEmailAndPassword = (
  data: LoginCredentialsDTO
): Promise<LoginConsentResponse> => {
  return axios.post('https://localhost:7001/spa/Login', data);
};

export type Login2faCredentialsDto = {
  twoFactorCode: string;
  rememberMachine: boolean;
  rememberMe: boolean;
  returnUrl?: string;
  error?: string;
};

export const loginWith2fa = (data: Login2faCredentialsDto): Promise<LoginConsentResponse> => {
  return axios.post('https://localhost:7001/spa/LoginWith2Fa', data);
};
