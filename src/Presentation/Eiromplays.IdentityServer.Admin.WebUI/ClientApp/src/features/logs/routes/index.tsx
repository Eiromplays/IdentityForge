import { Navigate, Route } from '@tanstack/react-location';
import { queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getLog } from '../api/getLog';

import { Log } from './Log';
import { Logs } from './Logs';

export const LogsRoutes: Route<LocationGenerics> = {
  path: 'logs',
  children: [
    {
      path: '/',
      element: <Logs />,
      loader: async ({ search: { pagination, searchFilter } }) =>
        await queryClient.getQueryData(['search-logs', pagination?.index ?? 1, pagination?.size ?? 10]) ??
        await queryClient
          .fetchQuery(['search-logs', pagination?.index ?? 1, pagination?.size ?? 10], () =>
            searchPagination(
              '/logs-grants/search',
              { pageNumber: pagination?.index ?? 1, pageSize: pagination?.size ?? 10 },
              searchFilter
            )
          ),
    },
    {
      path: ':logId',
      element: <Log />,
      loader: async ({ params: { logId } }) =>
        await queryClient.getQueryData(['logs', logId]) ??
        await queryClient.fetchQuery(['logs', logId], () => getLog({ logId: logId })),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
