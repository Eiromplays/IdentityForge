import { Outlet } from '@tanstack/react-location';
import { Spinner, MainLayout, lazyImport } from 'eiromplays-ui';
import { Suspense } from 'react';

import { ConsentRoutes } from '@/features/consent';
import { GrantsRoutes } from '@/features/grants';
import { LogsRoutes } from '@/features/logs';
import { UserSessionsRoutes } from '@/features/user-sessions';
const { Dashboard } = lazyImport(() => import('@/features/misc'), 'Dashboard');
const { Profile } = lazyImport(() => import('@/features/users'), 'Profile');
const { PersonalData } = lazyImport(() => import('@/features/users'), 'PersonalData');
const { TwoFactorAuthentication } = lazyImport(
  () => import('@/features/users'),
  'TwoFactorAuthentication'
);
const { ChangePassword } = lazyImport(() => import('@/features/users'), 'ChangePassword');

const App = () => {
  return (
    <MainLayout>
      <Suspense
        fallback={
          <div className="h-full w-full flex items-center justify-center">
            <Spinner size="xl" />
          </div>
        }
      >
        <Outlet />
      </Suspense>
    </MainLayout>
  );
};

export const protectedRoutes = [
  {
    path: '/app',
    element: <App />,
    children: [
      { path: '/', element: <Dashboard /> },
      GrantsRoutes,
      UserSessionsRoutes,
      LogsRoutes,
      { path: 'profile', element: <Profile /> },
      { path: 'personal-data', element: <PersonalData /> },
      { path: 'two-factor-authentication', element: <TwoFactorAuthentication /> },
      { path: 'change-password', element: <ChangePassword /> },
      ConsentRoutes,
      { path: '*', element: <Dashboard /> },
    ],
  },
];
