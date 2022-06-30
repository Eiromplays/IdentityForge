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
        queryClient.getQueryData(['search-logs', pagination?.index ?? 1, pagination?.size ?? 10]) ??
        queryClient
          .fetchQuery(['search-logs', pagination?.index ?? 1, pagination?.size ?? 10], () =>
            searchPagination(
              '/logs-grants/search',
              { pageNumber: pagination?.index ?? 1, pageSize: pagination?.size ?? 10 },
              searchFilter
            )
          )
          .then(() => ({})),
    },
    {
      path: ':logId',
      element: <Log />,
      loader: async ({ params: { logId } }) =>
        queryClient.getQueryData(['logs', logId]) ??
        queryClient.fetchQuery(['logs', logId], () => getLog({ logId: logId })).then(() => ({})),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
