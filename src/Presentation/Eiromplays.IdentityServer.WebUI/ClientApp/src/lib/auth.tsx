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
  if (!response) throw new Error('Login failed');

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
