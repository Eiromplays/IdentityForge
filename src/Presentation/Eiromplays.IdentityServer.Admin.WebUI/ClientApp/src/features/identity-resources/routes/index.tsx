import { Navigate, Route } from '@tanstack/react-location';
import { queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getIdentityResource } from '../api/getIdentityResource';

import { IdentityResourceInfo } from './IdentityResourceInfo';
import { IdentityResources } from './IdentityResources';

export const IdentityResourcesRoutes: Route<LocationGenerics> = {
  path: 'identity-resources',
  children: [
    {
      path: '/',
      element: <IdentityResources />,
      loader: async ({ search: { pagination, searchFilter } }) =>
        queryClient.getQueryData([
          'identity-resources',
          pagination?.index ?? 1,
          pagination?.size ?? 10,
        ]) ??
        queryClient
          .fetchQuery(['identity-resources', pagination?.index ?? 1, pagination?.size ?? 10], () =>
            searchPagination(
              '/identity-resources/search',
              { pageNumber: pagination?.index ?? 1, pageSize: pagination?.size ?? 10 },
              searchFilter
            )
          )
          .then(() => ({})),
    },
    {
      path: ':identityResourceId',
      element: <IdentityResourceInfo />,
      loader: async ({ params: { identityResourceId } }) =>
        queryClient.getQueryData(['identity-resource', identityResourceId]) ??
        (await queryClient.fetchQuery(['identity-resource', identityResourceId], () =>
          getIdentityResource({ identityResourceId: parseInt(identityResourceId, 10) })
        )),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
