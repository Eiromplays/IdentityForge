import { AuthUser, getUser } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import {
  Login2faCredentialsDto,
  LoginCredentialsDTO,
  loginWith2fa,
  loginWithEmailAndPassword,
  logoutUser,
  RegisterCredentialsDTO,
  registerWithEmailAndPassword,
} from '@/features/auth';

export const loadUser = async (): Promise<AuthUser | null> => {
  return await getUser();
};

export const loginFn = async (data: LoginCredentialsDTO) => {
  const response = await loginWithEmailAndPassword(data);

  // TODO: Check if I can handle this, and signInResult better
  if (response?.signInResult.succeeded) {
    toast.success('Login successful');
  }

  if (response?.validReturnUrl) {
    window.location.href = response?.validReturnUrl;
  } else if (response?.signInResult.succeeded) {
    window.location.href = '/bff/login';
  }

  if (response?.signInResult?.isLockedOut) {
    window.location.assign('/auth/lockout');
    return null;
  }

  const login2faViewModel = response as unknown as Login2faCredentialsDto;
  if (login2faViewModel?.rememberMe && login2faViewModel?.rememberMachine) {
    window.location.assign(
      `/auth/login2fa/${login2faViewModel.rememberMe}/${login2faViewModel?.returnUrl ?? ''}`
    );
    return null;
  }

  if (response?.signInResult?.isNotAllowed) {
    window.location.assign('/auth/not-allowed');
    return null;
  }

  return await loadUser();
};

export const login2faFn = async (data: Login2faCredentialsDto) => {
  await loginWith2fa(data);

  return await loadUser();
};

export const registerFn = async (data: RegisterCredentialsDTO) => {
  const response = await registerWithEmailAndPassword(data);

  if (response.message) toast.success(response.message);

  return await loadUser();
};

export const logoutFn = async (logoutId: string) => {
  await logoutUser({ logoutId: logoutId });
};
