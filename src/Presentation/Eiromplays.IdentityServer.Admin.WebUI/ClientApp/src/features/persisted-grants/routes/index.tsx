import { Navigate, Route } from '@tanstack/react-location';
import { queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getPersistedGrant } from '../api/getPersistedGrant';

import { PersistedGrant } from './PersistedGrant';
import { PersistedGrants } from './PersistedGrants';

export const PersistedGrantsRoutes: Route<LocationGenerics> = {
  path: 'persisted-grants',
  children: [
    {
      path: '/',
      element: <PersistedGrants />,
      loader: async ({ search: { pagination, searchFilter } }) =>
        queryClient.getQueryData([
          'search-persisted-grants',
          pagination?.index ?? 1,
          pagination?.size ?? 10,
        ]) ??
        queryClient
          .fetchQuery(
            ['search-persisted-grants', pagination?.index ?? 1, pagination?.size ?? 10],
            () =>
              searchPagination(
                '/persisted-grants/search',
                { pageNumber: pagination?.index ?? 1, pageSize: pagination?.size ?? 10 },
                searchFilter
              )
          )
          .then(() => ({})),
    },
    {
      path: ':key',
      element: <PersistedGrant />,
      loader: async ({ params: { key } }) =>
        queryClient.getQueryData(['persisted-grant', key]) ??
        (await queryClient.fetchQuery(['persisted-grant', key], () =>
          getPersistedGrant({ persistedGrantKey: key })
        )),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
