import { ReactLocation } from '@tanstack/react-location';
import {
  AddDataToRequestIgnoreUrls,
  AppProvider,
  axios,
  DefaultLocationGenerics,
  initializeAuth,
  Spinner,
} from 'eiromplays-ui';
import React from 'react';
import { ReactQueryDevtools } from 'react-query/devtools';

import {
  AuthUser,
  Login2faCredentialsDto,
  LoginCredentialsDTO,
  RegisterCredentialsDTO,
} from '@/features/auth';
import { loadUser, login2faFn, loginFn, logoutFn, registerFn } from '@/lib/auth';

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
    returnUrl: string;
    ReturnUrl: string;
    errorId: string;
    logoutId: string;
  };
};

const location = new ReactLocation<LocationGenerics>();

axios.defaults.withCredentials = true;
AddDataToRequestIgnoreUrls.push(`/consent`, `/spa/Login`, `/spa/externalLoginConfirmation`);

const { AuthProvider } = initializeAuth<
  AuthUser,
  LoginCredentialsDTO,
  Login2faCredentialsDto,
  RegisterCredentialsDTO
>({
  authConfig: {
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
