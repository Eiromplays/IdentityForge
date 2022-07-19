import { Navigate, Route } from '@tanstack/react-location';
import { defaultPageIndex, defaultPageSize, queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getLog } from '../api/getLog';

import { Log } from './Log';
import { Logs } from './Logs';

export const LogsRoutes: Route<LocationGenerics> = {
  path: 'logs',
  meta: {
    breadcrumb: () => 'Logs',
  },
  children: [
    {
      path: '/',
      element: <Logs />,
      loader: async ({ search: { pagination, searchFilter } }) =>
        (await queryClient.getQueryData([
          'search-logs',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ])) ??
        (await queryClient.fetchQuery(
          [
            'search-logs',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          () =>
            searchPagination(
              '/logs/search',
              {
                pageNumber: pagination?.index || defaultPageIndex,
                pageSize: pagination?.size || defaultPageSize,
              },
              searchFilter
            )
        )),
    },
    {
      path: ':logId',
      element: <Log />,
      loader: async ({ params: { logId } }) =>
        (await queryClient.getQueryData(['logs', logId])) ??
        (await queryClient.fetchQuery(['logs', logId], () => getLog({ logId: logId }))),
      meta: {
        breadcrumb: (params) => params.logId,
      },
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
