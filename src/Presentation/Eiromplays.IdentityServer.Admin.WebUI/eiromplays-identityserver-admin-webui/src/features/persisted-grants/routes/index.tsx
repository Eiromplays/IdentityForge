import { Navigate, Route } from '@tanstack/react-location';
import { queryClient } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getPersistedGrant } from '../api/getPersistedGrant';
import { searchPersistedGrants } from '../api/searchPersistedGrants';

import { PersistedGrant } from './PersistedGrant';
import { PersistedGrants } from './PersistedGrants';

export const PersistedGrantsRoutes: Route<LocationGenerics> = {
  path: 'persisted-grants',
  children: [
    {
      path: '/',
      element: <PersistedGrants />,
      loader: async ({ search: { pagination } }) =>
        queryClient.getQueryData(['search-persisted-grants', pagination?.index ?? 1]) ??
        queryClient
          .fetchQuery(['search-persisted-grants', pagination?.index ?? 1], () =>
            searchPersistedGrants({
              pageNumber: pagination?.index ?? 1,
              pageSize: pagination?.size ?? 10,
            })
          )
          .then(() => ({})),
    },
    {
      path: ':key',
      element: <PersistedGrant />,
      loader: async ({ params: { key } }) =>
        queryClient.getQueryData(['persisted-grant', key]) ??
        queryClient.fetchQuery(['persisted-grant', key], () =>
          getPersistedGrant({ persistedGrantKey: key })
        ),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
