import { Spinner } from '@/components/Elements/Spinner';
import { AuthUser, getUser } from '@/features/auth';
import { initReactQueryAuth } from '@/providers/AuthProvider';

async function loadUser() {
  const data = await getUser();
  return data;
}

async function loginFn() {
  const user = await loadUser();
  return user;
}

async function registerFn() {
  const user = await loadUser();
  return user;
}

async function logoutFn() {
  const user = await loadUser();
  window.location.assign(user.sessionInfo.logoutUrl);
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

type LoginCredentials = {
  test: string;
};

type RegisterCredentials = {
  test: string;
};

export const { AuthProvider, useAuth } = initReactQueryAuth<
  AuthUser | null,
  unknown,
  LoginCredentials,
  RegisterCredentials
>(authConfig);
