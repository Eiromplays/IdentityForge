import Button from '@mui/material/Button';
import CircularProgress from '@mui/material/CircularProgress';
import CssBaseline from '@mui/material/CssBaseline';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import useMediaQuery from '@mui/material/useMediaQuery';
import * as React from 'react';
import { ErrorBoundary } from 'react-error-boundary';
import { HelmetProvider } from 'react-helmet-async';
import {
  MdPersonOutline,
  MdBadge,
  MdPin,
  MdPassword,
  MdDescription,
  MdCheckCircle,
} from 'react-icons/md';
import { QueryClientProvider } from 'react-query';
import { ReactQueryDevtools } from 'react-query/devtools';
import { BrowserRouter as Router } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';

import { SideBar } from '@/components/SideBar/SideBar';
import { AuthProvider } from '@/lib/auth';
import { queryClient } from '@/lib/react-query';

import 'react-toastify/dist/ReactToastify.min.css';

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
  children: React.ReactNode;
};

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export const AppProvider = ({ children }: AppProviderProps) => {
  const prefersDarkMode = useMediaQuery('(prefers-color-scheme: dark)');

  const theme = React.useMemo(
    () =>
      createTheme({
        palette: {
          mode: prefersDarkMode ? 'dark' : 'light',
        },
      }),
    [prefersDarkMode]
  );

  return (
    <React.Suspense
      fallback={
        <div className="flex items-center justify-center w-screen h-screen">
          <CircularProgress />
        </div>
      }
    >
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <ErrorBoundary FallbackComponent={ErrorFallback}>
          <HelmetProvider>
            <QueryClientProvider client={queryClient}>
              {process.env.NODE_ENV !== 'test' && <ReactQueryDevtools />}
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
                <SideBar
                  pages={[
                    {
                      name: 'My Profile',
                      url: '/profile',
                      icon: <MdPersonOutline size={48} color="white" />,
                    },
                    {
                      name: 'Personal Data',
                      url: '/personalData',
                      icon: <MdBadge size={48} color="white" />,
                    },
                    {
                      name: 'Two-factor authentication',
                      url: '/two-factorAuthentication',
                      icon: <MdPin size={48} color="white" />,
                    },
                    {
                      name: 'Change Password',
                      url: '/changePassword',
                      icon: <MdPassword size={48} color="white" />,
                    },
                    {
                      name: 'Discovery Document',
                      url: '/discoveryDocument',
                      icon: <MdDescription size={48} color="white" />,
                    },
                    {
                      name: 'Persisted Grants',
                      url: '/persistedGrants',
                      icon: <MdCheckCircle size={48} color="white" />,
                    },
                  ]}
                  settings={[
                    { name: 'Show User Session', url: '/user-session' },
                    { name: 'Profile', url: '/profile' },
                  ]}
                />
                <Router>{children}</Router>
              </AuthProvider>
            </QueryClientProvider>
          </HelmetProvider>
        </ErrorBoundary>
      </ThemeProvider>
    </React.Suspense>
  );
};
