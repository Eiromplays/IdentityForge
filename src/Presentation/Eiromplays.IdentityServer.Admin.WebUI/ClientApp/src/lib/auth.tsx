import { Spinner } from '@/components/Elements/Spinner';
import { AuthUser, getUser } from '@/features/auth';
import { initReactQueryAuth } from '@/providers/AuthProvider';

async function loadUser(): Promise<AuthUser> {
  const data = await getUser();

  return data as AuthUser;
}

async function loginFn() {
  const user = await loadUser();

  return user;
}

async function login2faFn() {
  const user = await loadUser();

  return user;
}

async function registerFn() {
  const user = await loadUser();

  return user;
}

async function logoutFn() {
  const user = await loadUser();

  if (user?.logoutUrl) {
    window.location.href = user.logoutUrl;
  }
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
  unknown,
  unknown,
  unknown
>(authConfig);
