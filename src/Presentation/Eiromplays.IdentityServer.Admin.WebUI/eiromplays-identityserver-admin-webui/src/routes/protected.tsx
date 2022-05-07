import { Outlet } from '@tanstack/react-location';
import { lazyImport, MainLayout, NotAllowed, Spinner } from 'eiromplays-ui';
import { Suspense } from 'react';

import logo from '@/assets/logo.svg';
import { LogsRoutes } from '@/features/logs';
import { PersistedGrantsRoutes } from '@/features/persisted-grants';
import { RolesRoutes } from '@/features/roles';
import { UserSessionsRoutes } from '@/features/user-sessions';
import { UsersRoutes } from '@/features/users';
import { ROLES, useAuthorization } from '@/lib/authorization';

const { Dashboard } = lazyImport(() => import('@/features/misc'), 'Dashboard');

const App = () => {
  const { checkAccess } = useAuthorization();

  if (!checkAccess({ allowedRoles: [ROLES.ADMINISTRATOR] })) return <NotAllowed />;

  return (
    <MainLayout logo={logo}>
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
    path: 'app',
    element: <App />,
    children: [
      { path: '/', element: <Dashboard /> },
      PersistedGrantsRoutes,
      UserSessionsRoutes,
      LogsRoutes,
      UsersRoutes,
      RolesRoutes,
      { path: '*', element: <Dashboard /> },
    ],
  },
];
