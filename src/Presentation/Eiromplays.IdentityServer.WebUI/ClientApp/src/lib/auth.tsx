import CircularProgress from '@mui/material/CircularProgress';
import { initReactQueryAuth } from 'react-query-auth';

import { getUser, User } from '@/features/auth';

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
  window.location.assign(window.location.origin as unknown as string);
}

const authConfig = {
  loadUser,
  loginFn,
  registerFn,
  logoutFn,
  LoaderComponent() {
    return (
      <div className="w-screen h-screen flex justify-center items-center">
        <CircularProgress />
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
  User | null,
  unknown,
  LoginCredentials,
  RegisterCredentials
>(authConfig);
