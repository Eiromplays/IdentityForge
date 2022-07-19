import { Navigate, Route } from '@tanstack/react-location';
import { defaultPageIndex, defaultPageSize, queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { ServerSideSessions } from '../routes/ServerSideSessions';
import { UserSessions } from '../routes/UserSessions';

import { Sessions } from './Sessions';

export const UserSessionsRoutes: Route<LocationGenerics> = {
  path: 'sessions',
  meta: {
    breadcrumb: () => 'Sessions',
  },
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
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ])) ??
        (await queryClient.fetchQuery(
          [
            'user-sessions',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          () =>
            searchPagination(
              '/user-sessions/search',
              {
                pageNumber: pagination?.index || defaultPageIndex,
                pageSize: pagination?.size || defaultPageSize,
              },
              searchFilter
            )
        )),
      meta: {
        breadcrumb: () => 'User Sessions',
      },
    },
    {
      path: 'server-side-session',
      element: <ServerSideSessions />,
      loader: async ({ search: { pagination, searchFilter } }) =>
        (await queryClient.getQueryData([
          'server-side-sessions',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ])) ??
        (await queryClient.fetchQuery(
          [
            'server-side-sessions',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          () =>
            searchPagination(
              '/server-side-sessions/search',
              {
                pageNumber: pagination?.index || defaultPageIndex,
                pageSize: pagination?.size || defaultPageSize,
              },
              searchFilter
            )
        )),
      meta: {
        breadcrumb: () => 'Server-side Sessions',
      },
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
