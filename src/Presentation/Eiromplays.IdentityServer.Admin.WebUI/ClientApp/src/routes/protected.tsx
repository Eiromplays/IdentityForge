import { Suspense } from 'react';
import { Navigate, Outlet } from 'react-router-dom';

import { Spinner } from '@/components/Elements';
import { MainLayout } from '@/components/Layout';
// eslint-disable-next-line no-restricted-imports
import { NotAllowed } from '@/features/auth/components/NotAllowed';
import { ROLES, useAuthorization } from '@/lib/authorization';
import { lazyImport } from '@/utils/lazyImport';

const { PersistedGrantsRoutes } = lazyImport(
  () => import('@/features/persisted-grants'),
  'PersistedGrantsRoutes'
);
const { UserSessionsRoutes } = lazyImport(
  () => import('@/features/user-sessions'),
  'UserSessionsRoutes'
);
const { LogsRoutes } = lazyImport(() => import('@/features/logs'), 'LogsRoutes');
const { Dashboard } = lazyImport(() => import('@/features/misc'), 'Dashboard');
const { Profile } = lazyImport(() => import('@/features/users'), 'Profile');
const { Users } = lazyImport(() => import('@/features/users'), 'Users');
const { UserRoles } = lazyImport(() => import('@/features/users'), 'UserRoles');
const { RolesRoutes } = lazyImport(() => import('@/features/roles'), 'RolesRoutes');

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
      { path: 'persisted-grants/*', element: <PersistedGrantsRoutes /> },
      { path: 'user-sessions/*', element: <UserSessionsRoutes /> },
      { path: 'logs/*', element: <LogsRoutes /> },
      { path: 'profile/:id', element: <Profile /> },
      { path: 'users', element: <Users /> },
      { path: 'roles/*', element: <RolesRoutes /> },
      { path: 'users/:id/roles', element: <UserRoles /> },
      { path: '', element: <Dashboard /> },
      { path: '*', element: <Navigate to="." /> },
    ],
  },
];
