import { toast } from 'react-toastify';

import { Spinner } from '@/components/Elements/Spinner';
import {
  AuthUser,
  getUser,
  RegisterCredentialsDTO,
  LoginCredentialsDTO,
  Login2faCredentialsDto,
  loginWithEmailAndPassword,
  logoutUser,
  loginWith2fa,
  registerWithEmailAndPassword,
} from '@/features/auth';
import { initReactQueryAuth } from '@/providers/AuthProvider';

async function loadUser(): Promise<AuthUser> {
  const data = await getUser();

  return data as AuthUser;
}

async function loginFn(data: LoginCredentialsDTO) {
  const response = await loginWithEmailAndPassword(data);

  // TODO: Check if I can handle this, and signInResult better

  if (response?.validReturnUrl) {
    console.log(response.validReturnUrl);
    window.location.href = response.validReturnUrl;
  }

  if (response?.signInResult?.isLockedOut) {
    window.location.assign('/auth/lockout');
    return null;
  }

  const login2faViewModel = response as unknown as Login2faCredentialsDto;
  if (
    login2faViewModel?.rememberMe !== undefined &&
    login2faViewModel?.rememberMachine !== undefined
  ) {
    window.location.assign(
      `/auth/login2fa/${login2faViewModel.rememberMe}/${login2faViewModel?.returnUrl ?? ''}`
    );
    return null;
  }

  if (response?.signInResult?.isNotAllowed) {
    window.location.assign('/auth/not-allowed');
    return null;
  }

  const user = await loadUser();

  return user;
}

async function login2faFn(data: Login2faCredentialsDto) {
  const response = await loginWith2fa(data);
  console.log(response);

  const user = await loadUser();

  return user;
}

async function registerFn(data: RegisterCredentialsDTO) {
  const response = await registerWithEmailAndPassword(data);

  if (response.message) toast.success(response.message);

  const user = await loadUser();

  return user;
}

async function logoutFn() {
  await logoutUser();
}

const authConfig = {
  loadUser,
  loginFn,
  login2faFn,
  registerFn,
  logoutFn,
  LoaderComponent() {
    return (
      <div className="w-screen h-screen flex justify-center items-center">
        <Spinner />
      </div>
    );
  },
};

export const { AuthProvider, useAuth } = initReactQueryAuth<
  AuthUser | null,
  unknown,
  LoginCredentialsDTO,
  Login2faCredentialsDto,
  RegisterCredentialsDTO
>(authConfig);
