import { Navigate, Route } from '@tanstack/react-location';
import { queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getClient } from '../api/getClient';

import { ClientInfo } from './ClientInfo';
import { Clients } from './Clients';

export const ClientsRoutes: Route<LocationGenerics> = {
  path: 'clients',
  children: [
    {
      path: '/',
      element: <Clients />,
      loader: async ({ search: { pagination, searchFilter } }) =>
        queryClient.getQueryData([
          'search-clients',
          pagination?.index ?? 1,
          pagination?.size ?? 10,
        ]) ??
        queryClient
          .fetchQuery(['search-clients', pagination?.index ?? 1, pagination?.size ?? 10], () =>
            searchPagination(
              '/clients/search',
              { pageNumber: pagination?.index ?? 1, pageSize: pagination?.size ?? 10 },
              searchFilter
            )
          )
          .then(() => ({})),
    },
    {
      path: ':clientId',
      element: <ClientInfo />,
      loader: async ({ params: { clientId } }) =>
        queryClient.getQueryData(['client', clientId]) ??
        (await queryClient.fetchQuery(['client', clientId], () =>
          getClient({ clientId: parseInt(clientId, 10) })
        )),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
