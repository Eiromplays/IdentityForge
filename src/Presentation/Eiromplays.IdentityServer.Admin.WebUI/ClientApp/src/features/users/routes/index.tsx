import { Navigate, Route } from '@tanstack/react-location';
import { queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getUser } from '../api/getUser';
import { getUserClaims } from '../api/getUserClaims';
import { getUserRoles } from '../api/getUserRoles';
import { UserClaims } from '../routes/UserClaims';

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
          (await queryClient.getQueryData([
            'search-users',
            pagination?.index ?? 1,
            pagination?.size ?? 10,
          ])) ??
          (await queryClient.fetchQuery(
            ['search-users', pagination?.index ?? 1, pagination?.size ?? 10],
            () =>
              searchPagination(
                '/users/search',
                { pageNumber: pagination?.index ?? 1, pageSize: pagination?.size ?? 10 },
                searchFilter
              )
          ))
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
      path: ':userId/claims',
      element: <UserClaims />,
      loader: async ({ params: { userId } }) =>
        queryClient.getQueryData(['user', userId, 'claims']) ??
        (await queryClient.fetchQuery(['user', userId, 'claims'], () =>
          getUserClaims({ userId: userId })
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
