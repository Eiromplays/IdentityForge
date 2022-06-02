import { Navigate, Route } from '@tanstack/react-location';
import { queryClient } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getLog } from '../api/getLog';
import { searchLogs } from '../api/searchLogs';

import { Log } from './Log';
import { Logs } from './Logs';

export const LogsRoutes: Route<LocationGenerics> = {
  path: 'logs',
  children: [
    {
      path: '/',
      element: <Logs />,
      loader: async ({ search: { pagination } }) =>
        queryClient.getQueryData(['search-logs', pagination?.index ?? 1, pagination?.size ?? 10]) ??
        queryClient
          .fetchQuery(['search-logs', pagination?.index ?? 1, pagination?.size ?? 10], () =>
            searchLogs({
              pageNumber: pagination?.index ?? 1,
              pageSize: pagination?.size ?? 10,
            })
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
