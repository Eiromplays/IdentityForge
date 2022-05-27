import { ReactLocation } from '@tanstack/react-location';
import {
  AppProvider,
  AuthProviderConfig,
  AuthUser,
  defaultAuthConfig,
  DefaultLocationGenerics,
  initializeAuth,
} from 'eiromplays-ui';
import { ReactQueryDevtools } from 'react-query/devtools';

import { AppRoutes } from './routes';

export type LocationGenerics = DefaultLocationGenerics & {
  Params: {
    invoiceId: string;
    userId: string;
    logId: string;
    roleId: string;
    key: string;
    Id: string;
    clientId: string;
  };
};

const location = new ReactLocation<LocationGenerics>();

const customAuthConfig = <
  User extends AuthUser | null = AuthUser,
  Error = unknown
>(): AuthProviderConfig<User | null, Error> => {
  return {
    ...defaultAuthConfig,
    loadUser: () => {
      return defaultAuthConfig.loadUser<User>({
        silentLoginProps: { useSilentLogin: true, redirectIfSilentLoginFailed: true },
      });
    },
  };
};

function App() {
  const { AuthProvider } = initializeAuth({ authConfig: customAuthConfig() });
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
