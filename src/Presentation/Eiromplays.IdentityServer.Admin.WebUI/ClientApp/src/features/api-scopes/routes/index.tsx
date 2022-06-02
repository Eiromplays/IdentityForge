import { Navigate, Route } from '@tanstack/react-location';
import { queryClient } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getApiScope } from '../api/getApiScope';
import { searchApiScopes } from '../api/searchApiScopes';

import { ApiScopeInfo } from './ApiScopeInfo';
import { ApiScopes } from './ApiScopes';

export const ApiScopesRoutes: Route<LocationGenerics> = {
  path: 'api-scopes',
  children: [
    {
      path: '/',
      element: <ApiScopes />,
      loader: async ({ search: { pagination } }) =>
        queryClient.getQueryData(['api-scopes', pagination?.index ?? 1, pagination?.size ?? 10]) ??
        queryClient
          .fetchQuery(['api-scopes', pagination?.index ?? 1, pagination?.size ?? 10], () =>
            searchApiScopes({
              pageNumber: pagination?.index ?? 1,
              pageSize: pagination?.size ?? 10,
            })
          )
          .then(() => ({})),
    },
    {
      path: ':apiScopeId',
      element: <ApiScopeInfo />,
      loader: async ({ params: { apiScopeId } }) =>
        queryClient.getQueryData(['api-scope', apiScopeId]) ??
        (await queryClient.fetchQuery(['api-scope', apiScopeId], () =>
          getApiScope({ apiScopeId: parseInt(apiScopeId, 10) })
        )),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
