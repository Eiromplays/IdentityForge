import * as React from 'react';
import { ErrorBoundary } from 'react-error-boundary';
import { HelmetProvider } from 'react-helmet-async';
import { QueryClientProvider } from 'react-query';
import { ReactQueryDevtools } from 'react-query/devtools';
import { ToastContainer } from 'react-toastify';
import { Router, ReactLocation, Outlet } from '@tanstack/react-location';
import { ReactLocationDevtools } from '@tanstack/react-location-devtools';

import { queryClient, AuthProvider, Button, Spinner } from 'eiromplays-ui';

import 'react-toastify/dist/ReactToastify.css';
import { Landing, NotFound } from '@/features/misc';

const ErrorFallback = () => {
  return (
    <div
      className="text-red-500 w-screen h-screen flex flex-col justify-center items-center"
      role="alert"
    >
      <h2 className="text-lg font-semibold">Ooops, something went wrong :( </h2>
      <Button className="mt-4" onClick={() => window.location.assign(window.location.origin)}>
        Refresh
      </Button>
    </div>
  );
};

type AppProviderProps = {
  children?: React.ReactNode;
};

const location = new ReactLocation();

export const AppProvider = ({ children }: AppProviderProps) => {
  return (
    <React.Suspense
      fallback={
        <div className="flex items-center justify-center w-screen h-screen dark:bg-lighter-black">
          <Spinner size="xl" />
        </div>
      }
    >
      <ErrorBoundary FallbackComponent={ErrorFallback}>
        <HelmetProvider>
          <QueryClientProvider client={queryClient}>
            <Router location={location} routes={[{path: '/', element: <Landing />}, {path: '*', element: <NotFound />}]}>
              <ReactQueryDevtools position='bottom-right' />
              <ReactLocationDevtools />
              <AuthProvider>
                <ToastContainer
                  position="top-right"
                  theme="dark"
                  autoClose={5000}
                  hideProgressBar={false}
                  newestOnTop
                  closeOnClick
                  rtl={false}
                  pauseOnFocusLoss
                  draggable
                  pauseOnHover
                />
                <Outlet />
                {children}
              </AuthProvider>
            </Router>
          </QueryClientProvider>
        </HelmetProvider>
      </ErrorBoundary>
    </React.Suspense>
  );
};
