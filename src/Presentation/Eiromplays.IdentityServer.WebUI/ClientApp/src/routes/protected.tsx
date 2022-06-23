import { Outlet } from '@tanstack/react-location';
import {Spinner, MainLayout, lazyImport, Button, useAuth, NotLoggedIn} from 'eiromplays-ui';
import { Suspense } from 'react';
import {
  HiOutlineHome,
  HiOutlineUser,
  HiOutlineDocumentText,
  HiOutlineShieldCheck,
  HiOutlineCollection,
  HiOutlineLockClosed,
  HiOutlineKey,
  HiOutlineLogin,
} from 'react-icons/hi';
import { MdOutlineDevicesOther, MdOutlineHistory } from 'react-icons/md';

import logo from '@/assets/logo.svg';
import { ConsentRoutes } from '@/features/consent';
import { GrantsRoutes } from '@/features/grants';
import { LogsRoutes } from '@/features/logs';
import { UserLoginsRoutes } from '@/features/user-logins';
import { UserSessionsRoutes } from '@/features/user-sessions';
import { ROLES, useAuthorization } from 'eiromplays-ui';
import { identityServerAdminUiUrl, identityServerUrl } from '@/utils/envVariables';
const { Dashboard } = lazyImport(() => import('@/features/misc'), 'Dashboard');
const { Profile } = lazyImport(() => import('@/features/users'), 'Profile');
const { PersonalData } = lazyImport(() => import('@/features/users'), 'PersonalData');
const { TwoFactorAuthentication } = lazyImport(
  () => import('@/features/users'),
  'TwoFactorAuthentication'
);
const { ChangePassword } = lazyImport(() => import('@/features/users'), 'ChangePassword');
const { SetPassword } = lazyImport(() => import('@/features/users'), 'SetPassword');

const App = () => {
  const { user } = useAuth();
  const { checkAccess } = useAuthorization();
  const isAdmin = checkAccess({ allowedRoles: [ROLES.ADMINISTRATOR] });

  if (!user) {
    window.location.href = "/";
    return null;
  }

  return (
    <MainLayout
      logo={logo}
      userNavigationProps={{
        items: [{ name: 'Your Profile', to: 'profile' }],
        addProfileItem: false,
        customButtons: isAdmin && (
          <Button
            className="max-w-xs bg-gray-200 dark:bg-gray-600 p-2 flex items-center text-sm rounded-full
              focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
            onClick={() => (window.location.href = identityServerAdminUiUrl)}
          >
            Admin
          </Button>
        ),
      }}
      sideBarNavigationProps={{
        items: [
          { name: 'Dashboard', to: '.', icon: HiOutlineHome },
          { name: 'My Profile', to: 'profile', icon: HiOutlineUser },
          { name: 'Personal Data', to: 'personal-data', icon: HiOutlineCollection },
          {
            name: 'Two-factor authentication',
            to: 'two-factor-authentication',
            icon: HiOutlineLockClosed,
          },
          { name: 'Change Password', to: 'change-password', icon: HiOutlineKey },
          { name: 'Set Password', to: 'set-password', icon: HiOutlineKey },
          { name: 'Grants', to: 'grants', icon: HiOutlineShieldCheck },
          { name: 'User Sessions', to: 'user-sessions', icon: MdOutlineDevicesOther },
          { name: 'User Logins', to: 'user-logins', icon: HiOutlineLogin },
          { name: 'Logs', to: 'logs', icon: MdOutlineHistory },
          {
            name: 'Discovery Document',
            to: `${identityServerUrl}/.well-known/openid-configuration`,
            target: '_blank',
            externalLink: true,
            icon: HiOutlineDocumentText,
          },
        ],
      }}
    >
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
      UserLoginsRoutes,
      LogsRoutes,
      { path: 'profile', element: <Profile /> },
      { path: 'personal-data', element: <PersonalData /> },
      { path: 'two-factor-authentication', element: <TwoFactorAuthentication /> },
      { path: 'change-password', element: <ChangePassword /> },
      { path: 'set-password', element: <SetPassword /> },
      ConsentRoutes,
      { path: '*', element: <Dashboard /> },
    ],
  },
];
