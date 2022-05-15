import { ReactLocation } from '@tanstack/react-location';
import {
  AddDataToRequestIgnoreUrls,
  AppProvider,
  axios,
  DefaultLocationGenerics,
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
    rememberMe: string;
    returnUrl: string;
    clientId: string;
    email: string;
    userName: string;
    loginProvider: string;
  };
  Search: {
    returnUrl: string;
  };
};

const location = new ReactLocation<LocationGenerics>();
axios.defaults.withCredentials = true;
AddDataToRequestIgnoreUrls.push('https://localhost:7001/consent');

function App() {
  return (
    <AppProvider<LocationGenerics> location={location} routes={AppRoutes()}>
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
