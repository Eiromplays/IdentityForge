import { Navigate, Route } from '@tanstack/react-location';
import { queryClient } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getUserSession } from '../api/getUserSession';
import { searchUserSessions } from '../api/searchUserSessions';

import { UserSession } from './UserSession';
import { UserSessions } from './UserSessions';

export const UserSessionsRoutes: Route<LocationGenerics> = {
  path: 'user-sessions',
  children: [
    {
      path: '/',
      element: <UserSessions />,
      loader: async ({ search: { pagination } }) =>
        queryClient.getQueryData([
          'user-sessions',
          pagination?.index ?? 1,
          pagination?.size ?? 10,
        ]) ??
        queryClient
          .fetchQuery(['user-sessions', pagination?.index ?? 1, pagination?.size ?? 10], () =>
            searchUserSessions({
              pageNumber: pagination?.index ?? 1,
              pageSize: pagination?.size ?? 10,
            })
          )
          .then(() => ({})),
    },
    {
      path: ':key',
      element: <UserSession />,
      loader: async ({ params: { key } }) =>
        queryClient.getQueryData(['user-session', key]) ??
        (await queryClient.fetchQuery(['user-session', key], () => getUserSession({ key: key }))),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
