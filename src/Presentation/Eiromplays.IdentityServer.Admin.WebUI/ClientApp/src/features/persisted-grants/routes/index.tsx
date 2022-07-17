import { Navigate, Route } from '@tanstack/react-location';
import { defaultPageIndex, defaultPageSize, queryClient, searchPagination } from 'eiromplays-ui';

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
        (await queryClient.getQueryData([
          'search-persisted-grants',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ])) ??
        (await queryClient.fetchQuery(
          [
            'search-persisted-grants',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          () =>
            searchPagination(
              '/persisted-grants/search',
              {
                pageNumber: pagination?.index || defaultPageIndex,
                pageSize: pagination?.size || defaultPageSize,
              },
              searchFilter
            )
        )),
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
