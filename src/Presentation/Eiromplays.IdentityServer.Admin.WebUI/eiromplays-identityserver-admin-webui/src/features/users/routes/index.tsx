import { Navigate, Route } from '@tanstack/react-location';
import { queryClient } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getUser } from '../api/getUser';
import { getUserRoles } from '../api/getUserRoles';
import { searchUsers } from '../api/searchUsers';

import { User } from './User';
import { UserRoles } from './UserRoles';
import { Users } from './Users';

export const UsersRoutes: Route<LocationGenerics> = {
  path: 'users',
  children: [
    {
      path: '/',
      element: <Users />,
      loader: async ({ search: { pagination } }) =>
        queryClient.getQueryData(['search-users', pagination?.index ?? 1]) ??
        queryClient
          .fetchQuery(['search-users', pagination?.index ?? 1], () =>
            searchUsers({ pageNumber: pagination?.index ?? 1, pageSize: pagination?.size ?? 10 })
          )
          .then(() => ({})),
    },
    {
      path: ':userId/roles',
      element: <UserRoles />,
      loader: async ({ params: { userId } }) =>
        queryClient.getQueryData(['user', userId, 'roles']) ??
        queryClient.fetchQuery(['user', userId, 'roles'], () => getUserRoles({ userId })),
    },
    {
      path: ':userId',
      element: <User />,
      loader: async ({ params: { userId } }) =>
        queryClient.getQueryData(['user', userId]) ??
        queryClient.fetchQuery(['user', userId], () => getUser({ userId })),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
