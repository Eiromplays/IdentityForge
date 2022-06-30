import { Navigate, Route } from '@tanstack/react-location';
import { queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getUser } from '../api/getUser';
import { getUserRoles } from '../api/getUserRoles';

import { User } from './User';
import { UserRoles } from './UserRoles';
import { Users } from './Users';

export const UsersRoutes: Route<LocationGenerics> = {
  path: 'users',
  children: [
    {
      path: '/',
      element: <Users />,
      loader: async ({ search: { pagination, searchFilter } }) => {
        return (
          queryClient.getQueryData([
            'search-users',
            pagination?.index ?? 1,
            pagination?.size ?? 10,
          ]) ??
          queryClient
            .fetchQuery(['search-users', pagination?.index ?? 1, pagination?.size ?? 10], () =>
              searchPagination(
                '/users/search',
                { pageNumber: pagination?.index ?? 1, pageSize: pagination?.size ?? 10 },
                searchFilter
              )
            )
            .then(() => ({}))
        );
      },
    },
    {
      path: ':userId/roles',
      element: <UserRoles />,
      loader: async ({ params: { userId } }) =>
        queryClient.getQueryData(['user', userId, 'roles']) ??
        (await queryClient.fetchQuery(['user', userId, 'roles'], () =>
          getUserRoles({ userId: userId })
        )),
    },
    {
      path: ':userId',
      element: <User />,
      loader: async ({ params: { userId } }) =>
        queryClient.getQueryData(['user', userId]) ??
        (await queryClient.fetchQuery(['user', userId], () => getUser({ userId: userId }))),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
