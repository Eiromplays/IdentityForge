import { Navigate, Route } from '@tanstack/react-location';
import { queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getApiResource } from '../api/getApiResource';

import { ApiResourceInfo } from './ApiResourceInfo';
import { ApiResources } from './ApiResources';

export const ApiResourcesRoutes: Route<LocationGenerics> = {
  path: 'api-resources',
  children: [
    {
      path: '/',
      element: <ApiResources />,
      loader: async ({ search: { pagination, searchFilter } }) =>
        queryClient.getQueryData([
          'api-resources',
          pagination?.index ?? 1,
          pagination?.size ?? 10,
        ]) ??
        queryClient
          .fetchQuery(['api-resources', pagination?.index ?? 1, pagination?.size ?? 10], () =>
            searchPagination(
              '/api-resources/search',
              { pageNumber: pagination?.index ?? 1, pageSize: pagination?.size ?? 10 },
              searchFilter
            )
          )
          .then(() => ({})),
    },
    {
      path: ':apiResourceId',
      element: <ApiResourceInfo />,
      loader: async ({ params: { apiResourceId } }) =>
        queryClient.getQueryData(['api-resource', apiResourceId]) ??
        (await queryClient.fetchQuery(['api-resource', apiResourceId], () =>
          getApiResource({ apiResourceId: parseInt(apiResourceId, 10) })
        )),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
