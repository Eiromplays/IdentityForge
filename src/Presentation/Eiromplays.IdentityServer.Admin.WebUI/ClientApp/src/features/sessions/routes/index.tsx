import { Navigate, Route } from '@tanstack/react-location';
import { queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { ServerSideSessions } from '../routes/ServerSideSessions';
import { UserSessions } from '../routes/UserSessions';

import { Sessions } from './Sessions';

export const UserSessionsRoutes: Route<LocationGenerics> = {
  path: 'sessions',
  children: [
    {
      path: '/',
      element: <Sessions />,
    },
    {
      path: 'user-session',
      element: <UserSessions />,
      loader: async ({ search: { pagination, searchFilter } }) =>
        (await queryClient.getQueryData([
          'user-sessions',
          pagination?.index ?? 1,
          pagination?.size ?? 10,
        ])) ??
        (await queryClient.fetchQuery(
          ['user-sessions', pagination?.index ?? 1, pagination?.size ?? 10],
          () =>
            searchPagination(
              '/user-sessions/search',
              { pageNumber: pagination?.index ?? 1, pageSize: pagination?.size ?? 10 },
              searchFilter
            )
        )),
    },
    {
      path: 'server-side-session',
      element: <ServerSideSessions />,
      loader: async ({ search: { pagination, searchFilter } }) =>
        (await queryClient.getQueryData([
          'server-side-sessions',
          pagination?.index ?? 1,
          pagination?.size ?? 10,
        ])) ??
        (await queryClient.fetchQuery(
          ['server-side-sessions', pagination?.index ?? 1, pagination?.size ?? 10],
          () =>
            searchPagination(
              '/server-side-sessions/search',
              { pageNumber: pagination?.index ?? 1, pageSize: pagination?.size ?? 10 },
              searchFilter
            )
        )),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
