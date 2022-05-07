import { Navigate, Route } from '@tanstack/react-location';

import { LocationGenerics } from '@/App';

import { getUserSessions } from '../api/getUserSessions';

import { UserSession } from './UserSession';
import { UserSessions } from './UserSessions';

export const UserSessionsRoutes: Route<LocationGenerics> = {
  path: 'user-sessions',
  children: [
    {
      path: '/',
      element: <UserSessions />,
      loader: async () => ({
        users: await getUserSessions(),
      }),
    },
    {
      path: ':key',
      element: <UserSession />,
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
