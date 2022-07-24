import { Navigate, Route } from '@tanstack/react-location';
import { defaultPageIndex, defaultPageSize, queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getApiScope } from '../api/getApiScope';

import { ApiScopeInfo } from './ApiScopeInfo';
import { ApiScopes } from './ApiScopes';

export const ApiScopesRoutes: Route<LocationGenerics> = {
  path: 'api-scopes',
  meta: {
    breadcrumb: () => 'Api Scopes',
  },
  children: [
    {
      path: '/',
      element: <ApiScopes />,
      loader: async ({ search: { pagination, searchFilter } }) =>
        (await queryClient.getQueryData([
          'search-api-scopes',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ])) ??
        (await queryClient.fetchQuery(
          [
            'search-api-scopes',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          () =>
            searchPagination(
              '/api-scopes/search',
              {
                pageNumber: pagination?.index || defaultPageIndex,
                pageSize: pagination?.size || defaultPageSize,
              },
              searchFilter
            )
        )),
    },
    {
      path: ':apiScopeId',
      element: <ApiScopeInfo />,
      loader: async ({ params: { apiScopeId } }) =>
        queryClient.getQueryData(['api-scope', apiScopeId]) ??
        (await queryClient.fetchQuery(['api-scope', apiScopeId], () =>
          getApiScope({ apiScopeId: +apiScopeId })
        )),
      meta: {
        breadcrumb: (params: any) => params.apiScopeId,
      },
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
