import { Outlet } from '@tanstack/react-location';
import { Spinner, MainLayout, lazyImport } from 'eiromplays-ui';
import { Suspense } from 'react';
import {
  HiOutlineHome,
  HiOutlineUser,
  HiOutlineDocumentText,
  HiOutlineShieldCheck,
  HiOutlineCollection,
  HiOutlineLockClosed,
  HiOutlineKey,
} from 'react-icons/hi';
import { MdOutlineDevicesOther, MdOutlineHistory } from 'react-icons/md';

import logo from '@/assets/logo.svg';
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
    <MainLayout
      logo={logo}
      userNavigationItems={[{ name: 'Your Profile', to: 'profile' }]}
      sideBarNavigationItems={[
        { name: 'Dashboard', to: '.', icon: HiOutlineHome },
        { name: 'My Profile', to: 'profile', icon: HiOutlineUser },
        { name: 'Personal Data', to: 'personal-data', icon: HiOutlineCollection },
        {
          name: 'Two-factor authentication',
          to: 'two-factor-authentication',
          icon: HiOutlineLockClosed,
        },
        { name: 'Change Password', to: 'change-password', icon: HiOutlineKey },
        { name: 'Grants', to: 'grants', icon: HiOutlineShieldCheck },
        { name: 'User Sessions', to: 'user-sessions', icon: MdOutlineDevicesOther },
        { name: 'Logs', to: 'logs', icon: MdOutlineHistory },
        {
          name: 'Discovery Document',
          to: 'https://localhost:7001/.well-known/openid-configuration',
          target: '_blank',
          externalLink: true,
          icon: HiOutlineDocumentText,
        },
      ]}
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
