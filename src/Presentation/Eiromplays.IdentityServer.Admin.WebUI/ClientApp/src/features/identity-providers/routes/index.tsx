import { Navigate, Route } from '@tanstack/react-location';
import { defaultPageIndex, defaultPageSize, queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getIdentityProvider } from '../api/getIdentityProvider';

import { IdentityProviderInfo } from './IdentityProviderInfo';
import { IdentityProviders } from './IdentityProviders';

export const IdentityProvidersRoutes: Route<LocationGenerics> = {
  path: 'identity-providers',
  meta: {
    breadcrumb: () => 'Identity Providers',
  },
  children: [
    {
      path: '/',
      element: <IdentityProviders />,
      loader: async ({ search: { pagination, searchFilter } }) =>
        (await queryClient.getQueryData([
          'search-identity-providers',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ])) ??
        (await queryClient.fetchQuery(
          [
            'search-identity-providers',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          () =>
            searchPagination(
              '/identity-providers/search',
              {
                pageNumber: pagination?.index || defaultPageIndex,
                pageSize: pagination?.size || defaultPageSize,
              },
              searchFilter
            )
        )),
    },
    {
      path: ':identityProviderId',
      element: <IdentityProviderInfo />,
      loader: async ({ params: { identityProviderId } }) =>
        queryClient.getQueryData(['identity-provider', identityProviderId]) ??
        (await queryClient.fetchQuery(['identity-provider', identityProviderId], () =>
          getIdentityProvider({ identityProviderId: +identityProviderId })
        )),
      meta: {
        breadcrumb: (params: any) => params.identityProviderId,
      },
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
