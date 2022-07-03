import { axios, MessageResponse } from 'eiromplays-ui';

import { identityServerUrl } from '@/utils/envVariables';

import { LoginResponse } from '../types';

export type LoginCredentialsDTO = {
  provider: string;
  login: string;
  password: string;
  returnUrl?: string;
  rememberMe?: boolean;
};

export const loginWithEmailAndPasswordOrPhoneNumber = (
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

export const loginWith2fa = (data: Login2faCredentialsDto): Promise<LoginResponse> => {
  return axios.post(`${identityServerUrl}/api/v1/account/login2fa`, data);
};

export type SendVerificationCodeDto = {
  phoneNumber: string;
};

export const sendVerificationCode = (data: SendVerificationCodeDto): Promise<MessageResponse> => {
  return axios.post(`${identityServerUrl}/api/v1/account/send-verification-code`, data);
};
