import { axios } from 'eiromplays-ui';

import { identityServerUrl } from '@/utils/envVariables';

import {LoginResponse} from '../types';

export type LoginCredentialsDTO = {
  login: string;
  password: string;
  returnUrl?: string;
  rememberMe?: boolean;
};

export const loginWithEmailAndPassword = (
  data: LoginCredentialsDTO
): Promise<LoginResponse> => {
  return axios.post(`${identityServerUrl}/api/v1/account/login`, data);
};

export type Login2faCredentialsDto = {
  twoFactorCode: string;
  rememberMachine: boolean;
  rememberMe: boolean;
  returnUrl?: string;
  error?: string;
};

export type GetLogin2FaResponse = {
  returnUrl: string;
  rememberMe: boolean;
};

export const loginWith2fa = (data: Login2faCredentialsDto): Promise<LoginResponse> => {
  return axios.post(`${identityServerUrl}/api/v1/account/login2fa`, data);
};
