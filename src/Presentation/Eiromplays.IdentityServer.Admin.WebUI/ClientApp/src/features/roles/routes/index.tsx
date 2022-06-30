import { Navigate, Route } from '@tanstack/react-location';
import { queryClient, searchPagination } from 'eiromplays-ui';

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
        queryClient.getQueryData([
          'search-roles',
          pagination?.index ?? 1,
          pagination?.size ?? 10,
        ]) ??
        queryClient
          .fetchQuery(['search-roles', pagination?.index ?? 1, pagination?.size ?? 10], () =>
            searchPagination(
              '/roles/search',
              { pageNumber: pagination?.index ?? 1, pageSize: pagination?.size ?? 10 },
              searchFilter
            )
          )
          .then(() => ({})),
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
