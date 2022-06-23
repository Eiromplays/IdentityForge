import { AuthUser, getUser } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import {
  GetLogin2FaResponse,
  Login2faCredentialsDto,
  LoginCredentialsDTO,
  loginWith2fa,
  loginWithEmailAndPassword,
  logoutUser,
  RegisterCredentialsDTO,
  registerWithEmailAndPassword,
} from '@/features/auth';

export const loadUser = async () => {
  return await getUser({
    authenticatedProps: {
      useAuthenticated: false,
      isAuthenticatedUrl: 'https://localhost:7001/api/v1/account/is-authenticated',
    },
  });
};

export const loginFn = async (data: LoginCredentialsDTO): Promise<AuthUser | null> => {
  const response = await loginWithEmailAndPassword(data);

  if (response?.twoFactorReturnUrl){
    window.location.href = response.twoFactorReturnUrl;
    return null;
  }

  // TODO: Check if I can handle this, and signInResult better
  if (response?.signInResult?.succeeded) {
    toast.success('Login successful');
  }

  if (response?.validReturnUrl) {
    window.location.href = response?.validReturnUrl;
  } else if (response?.signInResult?.succeeded) {
    window.location.href = '/bff/login';
  }

  if (response?.signInResult?.isLockedOut) {
    window.location.assign('/auth/lockout');
    return null;
  }

  if (response?.signInResult?.isNotAllowed) {
    window.location.assign('/auth/not-allowed');
    return null;
  }

  return await loadUser();
};

export const login2faFn = async (data: Login2faCredentialsDto): Promise<AuthUser | null> => {
  const response = await loginWith2fa(data);

  if (response?.signInResult?.succeeded) {
    toast.success('Login successful');
  }

  if (response?.validReturnUrl) {
    window.location.href = response?.validReturnUrl;
  } else if (response?.signInResult?.succeeded) {
    window.location.href = '/bff/login';
  }

  if (response?.signInResult?.isLockedOut) {
    window.location.assign('/auth/lockout');
    return null;
  }

  if (response?.signInResult?.isNotAllowed) {
    window.location.assign('/auth/not-allowed');
    return null;
  }

  return await loadUser();
};

export const registerFn = async (data: RegisterCredentialsDTO): Promise<AuthUser | null> => {
  const response = await registerWithEmailAndPassword(data);

  if (response.message) toast.success(response.message);

  return await loadUser();
};

export const logoutFn = async (logoutId: string) => {
  await logoutUser({ logoutId: logoutId });
};
