import { Spinner } from '@/components/Elements/Spinner';
import {
  AuthUser,
  getUser,
  LoginCredentialsDTO,
  loginWithEmailAndPassword,
  logoutUser,
} from '@/features/auth';
import { initReactQueryAuth } from '@/providers/AuthProvider';

async function loadUser(): Promise<AuthUser> {
  const data = await getUser();

  return data as AuthUser;
}

async function loginFn(data: LoginCredentialsDTO) {
  const response = await loginWithEmailAndPassword(data);

  // TODO: Check if I can handle this, and signInResult better
  if (response?.validReturnUrl) window.location.href = response.validReturnUrl;

  if (response?.signInResult?.isLockedOut) {
    window.location.assign('/auth/lockout');
    return null;
  }
  if (response?.signInResult?.RequiresTwoFactor) {
    window.location.assign('/auth/login-two-factor');
    return null;
  }

  if (response?.signInResult?.isNotAllowed) {
    window.location.assign('/auth/not-allowed');
    return null;
  }

  const user = await loadUser();

  return user;
}

async function registerFn() {
  const user = await loadUser();

  return user;
}

async function logoutFn() {
  await logoutUser();
}

const authConfig = {
  loadUser,
  loginFn,
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

type RegisterCredentials = {
  test: string;
};

export const { AuthProvider, useAuth } = initReactQueryAuth<
  AuthUser | null,
  unknown,
  LoginCredentialsDTO,
  RegisterCredentials
>(authConfig);
