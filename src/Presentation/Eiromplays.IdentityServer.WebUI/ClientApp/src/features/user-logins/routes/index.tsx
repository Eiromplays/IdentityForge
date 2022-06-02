import { Navigate, Route } from '@tanstack/react-location';

import { LocationGenerics } from '@/App';

import { UserLogins } from './UserLogins';

export const UserLoginsRoutes: Route<LocationGenerics> = {
  path: 'user-logins',
  children: [
    { path: '/', element: <UserLogins /> },
    { path: '*', element: <Navigate to="." /> },
  ],
};
