import { Suspense } from 'react';

import { Navigate, Outlet } from '@tanstack/react-location';
import { Spinner, MainLayout, NotAllowed, lazyImport } from 'eiromplays-ui';

import { ROLES, useAuthorization } from '@/lib/authorization';
import { RolesRoutes } from '@/features/roles';
import { UsersRoutes } from '@/features/users';

const { Dashboard } = lazyImport(() => import('@/features/misc'), 'Dashboard');

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
    path: 'app',
    element: <App />,
    children: [
      //{ path: 'persisted-grants/*', element: <PersistedGrantsRoutes /> },
      //{ path: 'user-sessions/*', element: <UserSessionsRoutes /> },
      //{ path: 'logs/*', element: <LogsRoutes /> },
      UsersRoutes,
      RolesRoutes,
      { path: '*', element: <Dashboard /> },
    ],
  },
];
