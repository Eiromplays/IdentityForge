import { Navigate, Route } from '@tanstack/react-location';
import { defaultPageIndex, defaultPageSize, queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getIdentityResource } from '../api/getIdentityResource';

import { IdentityResourceInfo } from './IdentityResourceInfo';
import { IdentityResources } from './IdentityResources';

export const IdentityResourcesRoutes: Route<LocationGenerics> = {
  path: 'identity-resources',
  meta: {
    breadcrumb: () => 'Identity Resources',
  },
  children: [
    {
      path: '/',
      element: <IdentityResources />,
      loader: async ({ search: { pagination, searchFilter } }) =>
        (await queryClient.getQueryData([
          'search-identity-resources',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ])) ??
        (await queryClient.fetchQuery(
          [
            'search-identity-resources',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          () =>
            searchPagination(
              '/identity-resources/search',
              {
                pageNumber: pagination?.index || defaultPageIndex,
                pageSize: pagination?.size || defaultPageSize,
              },
              searchFilter
            )
        )),
    },
    {
      path: ':identityResourceId',
      element: <IdentityResourceInfo />,
      loader: async ({ params: { identityResourceId } }) =>
        queryClient.getQueryData(['identity-resource', identityResourceId]) ??
        (await queryClient.fetchQuery(['identity-resource', identityResourceId], () =>
          getIdentityResource({ identityResourceId: +identityResourceId })
        )),
      meta: {
        breadcrumb: (params: any) => params.identityResourceId,
      },
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
