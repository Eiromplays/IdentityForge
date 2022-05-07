import { Navigate, Route } from '@tanstack/react-location';

import { LocationGenerics } from '@/App';

import { getUser } from '../api/getUser';
import { getUserRoles } from '../api/getUserRoles';
import { getUsers } from '../api/getUsers';

import { User } from './User';
import { UserRoles } from './UserRoles';
import { Users } from './Users';

export const UsersRoutes: Route<LocationGenerics> = {
  path: 'users',
  children: [
    {
      path: '/',
      element: <Users />,
      loader: async () => ({
        users: await getUsers(),
      }),
    },
    {
      path: ':userId/roles',
      element: <UserRoles />,
      loader: async ({ params: { userId } }) => ({
        user: await getUserRoles({ userId }),
      }),
    },
    {
      path: ':userId',
      element: <User />,
      loader: async ({ params: { userId } }) => ({
        user: await getUser({ userId }),
      }),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
