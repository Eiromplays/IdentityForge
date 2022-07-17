import { Navigate, Route } from '@tanstack/react-location';
import { defaultPageIndex, defaultPageSize, queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getRole } from '../api/getRole';

import { RoleInfo } from './RoleInfo';
import { Roles } from './Roles';

export const RolesRoutes: Route<LocationGenerics> = {
  path: 'roles',
  children: [
    {
      path: '/',
      element: <Roles />,
      loader: async ({ search: { pagination, searchFilter } }) =>
        (await queryClient.getQueryData([
          'search-roles',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ])) ??
        (await queryClient.fetchQuery(
          [
            'search-roles',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          () =>
            searchPagination(
              '/roles/search',
              {
                pageNumber: pagination?.index || defaultPageIndex,
                pageSize: pagination?.size || defaultPageSize,
              },
              searchFilter
            )
        )),
    },
    {
      path: ':roleId',
      element: <RoleInfo />,
      loader: async ({ params: { roleId } }) =>
        queryClient.getQueryData(['role', roleId]) ??
        (await queryClient.fetchQuery(['role', roleId], () => getRole({ roleId: roleId }))),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
