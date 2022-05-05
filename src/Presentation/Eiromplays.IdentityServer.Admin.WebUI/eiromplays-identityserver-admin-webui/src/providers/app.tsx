import * as React from 'react';
import { ErrorBoundary } from 'react-error-boundary';
import { HelmetProvider } from 'react-helmet-async';
import { QueryClientProvider } from 'react-query';
import { ReactQueryDevtools } from 'react-query/devtools';
import { ToastContainer } from 'react-toastify';
import { Router, ReactLocation, Outlet, Route } from '@tanstack/react-location';
import { ReactLocationDevtools } from '@tanstack/react-location-devtools';

import { Button, Spinner, AuthProvider, queryClient } from 'eiromplays-ui';

import 'react-toastify/dist/ReactToastify.css';

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
  routes: Route<any>[];
  location?: ReactLocation;
  children?: React.ReactNode;
};

export const AppProvider = ({ routes, location = new ReactLocation(), children }: AppProviderProps) => {
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
            <AuthProvider>
              <Router location={location} routes={routes}>
                <ReactQueryDevtools position="bottom-right" />
                <ReactLocationDevtools />
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
              </Router>
            </AuthProvider>
          </QueryClientProvider>
        </HelmetProvider>
      </ErrorBoundary>
    </React.Suspense>
  );
};
