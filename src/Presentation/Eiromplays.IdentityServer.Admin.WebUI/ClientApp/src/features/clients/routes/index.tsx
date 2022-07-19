import { Navigate, Route } from '@tanstack/react-location';
import { defaultPageIndex, defaultPageSize, queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getClient } from '../api/getClient';

import { ClientInfo } from './ClientInfo';
import { Clients } from './Clients';

export const ClientsRoutes: Route<LocationGenerics> = {
  path: 'clients',
  meta: {
    breadcrumb: () => 'Clients',
  },
  children: [
    {
      path: '/',
      element: <Clients />,
      loader: async ({ search: { pagination, searchFilter } }) =>
        (await queryClient.getQueryData([
          'search-clients',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ])) ??
        (await queryClient.fetchQuery(
          [
            'search-clients',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          () =>
            searchPagination(
              '/clients/search',
              {
                pageNumber: pagination?.index || defaultPageIndex,
                pageSize: pagination?.size || defaultPageSize,
              },
              searchFilter
            )
        )),
    },
    {
      path: ':clientId',
      element: <ClientInfo />,
      loader: async ({ params: { clientId } }) =>
        queryClient.getQueryData(['client', clientId]) ??
        (await queryClient.fetchQuery(['client', clientId], () =>
          getClient({ clientId: parseInt(clientId, 10) })
        )),
      meta: {
        breadcrumb: (params) => params.clientId,
      },
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
