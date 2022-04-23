import { Suspense } from 'react';
import { Navigate, Outlet } from 'react-router-dom';

import { Spinner } from '@/components/Elements';
import { MainLayout } from '@/components/Layout';
// eslint-disable-next-line no-restricted-imports
import { NotAllowed } from '@/features/auth/components/NotAllowed';
import { ROLES, useAuthorization } from '@/lib/authorization';
import { lazyImport } from '@/utils/lazyImport';

const { GrantsRoutes } = lazyImport(() => import('@/features/grants'), 'GrantsRoutes');
const { UserSessionsRoutes } = lazyImport(
  () => import('@/features/user-sessions'),
  'UserSessionsRoutes'
);
const { LogsRoutes } = lazyImport(() => import('@/features/logs'), 'LogsRoutes');
const { Dashboard } = lazyImport(() => import('@/features/misc'), 'Dashboard');
const { Profile } = lazyImport(() => import('@/features/users'), 'Profile');
const { PersonalData } = lazyImport(() => import('@/features/users'), 'PersonalData');
const { TwoFactorAuthentication } = lazyImport(
  () => import('@/features/users'),
  'TwoFactorAuthentication'
);
const { ChangePassword } = lazyImport(() => import('@/features/users'), 'ChangePassword');

const App = () => {
  const { checkAccess } = useAuthorization();

  if (!checkAccess({ allowedRoles: [ROLES.ADMINISTRATOR] })) return <NotAllowed />;

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
      { path: 'grants/*', element: <GrantsRoutes /> },
      { path: 'user-sessions/*', element: <UserSessionsRoutes /> },
      { path: 'logs/*', element: <LogsRoutes /> },
      { path: 'profile', element: <Profile /> },
      { path: 'personal-data', element: <PersonalData /> },
      { path: 'two-factor-authentication', element: <TwoFactorAuthentication /> },
      { path: 'change-password', element: <ChangePassword /> },
      { path: '', element: <Dashboard /> },
      { path: '*', element: <Navigate to="." /> },
    ],
  },
];
