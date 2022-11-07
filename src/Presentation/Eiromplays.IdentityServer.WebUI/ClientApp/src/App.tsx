import { ReactLocation } from '@tanstack/react-location';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import {
  AppProvider,
  axios,
  defaultAuthConfig,
  DefaultLocationGenerics,
  initializeAuth,
  Spinner,
} from 'eiromplays-ui';
import React from 'react';

import {
  AuthUser,
  Login2faCredentialsDto,
  LoginCredentialsDTO,
  RegisterCredentialsDTO,
} from '@/features/auth';
import { login2faFn, loginFn, logoutFn, registerFn } from '@/lib/auth';

import { AppRoutes } from './routes';

export type LocationGenerics = DefaultLocationGenerics & {
  Params: {
    userId: string;
    logId: string;
    roleId: string;
    key: string;
    Id: string;
    rememberMe: string;
    returnUrl: string;
    clientId: string;
    email: string;
    userName: string;
    loginProvider: string;
  };
  Search: {
    userId: string;
    returnUrl: string;
    ReturnUrl: string;
    errorId: string;
    logoutId: string;
    rememberMe: string;
    email: string;
    userName: string;
    loginProvider: string;
    token: string;
    message: string;
  };
};

const location = new ReactLocation<LocationGenerics>();

axios.defaults.withCredentials = true;

const { AuthProvider } = initializeAuth<
  AuthUser,
  LoginCredentialsDTO,
  Login2faCredentialsDto,
  RegisterCredentialsDTO
>({
  authConfig: {
    loadUser: () => {
      return defaultAuthConfig.loadUser<AuthUser>({
        customClaims: [
          { type: 'phone_number' },
          { type: 'phone_number_verified', valueType: typeof true },
        ],
      });
    },
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
  },
});

function App() {
  return (
    <AppProvider<LocationGenerics>
      location={location}
      routes={AppRoutes()}
      CustomAuthProvider={AuthProvider}
    >
      <ReactQueryDevtools
        toggleButtonProps={{
          style: {
            marginLeft: '4rem',
          },
        }}
      />
    </AppProvider>
  );
}

export default App;
