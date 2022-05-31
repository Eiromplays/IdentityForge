import { axios } from 'eiromplays-ui';

import { identityServerUrl } from '@/utils/envVariables';

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
  return axios.post(`${identityServerUrl}/spa/Login`, data);
};

export type Login2faCredentialsDto = {
  twoFactorCode: string;
  rememberMachine: boolean;
  rememberMe: boolean;
  returnUrl?: string;
  error?: string;
};

export const loginWith2fa = (data: Login2faCredentialsDto): Promise<LoginConsentResponse> => {
  return axios.post(`${identityServerUrl}/spa/LoginWith2Fa`, data);
};
