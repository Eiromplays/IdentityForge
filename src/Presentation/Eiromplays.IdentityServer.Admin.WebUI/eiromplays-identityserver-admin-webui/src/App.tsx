import { ReactLocation } from '@tanstack/react-location';
import { AppProvider, DefaultLocationGenerics } from 'eiromplays-ui';
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
  };
};

const location = new ReactLocation<LocationGenerics>();

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
