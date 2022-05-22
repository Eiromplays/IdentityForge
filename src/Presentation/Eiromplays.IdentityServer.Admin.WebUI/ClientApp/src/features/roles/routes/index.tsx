import { Navigate, Route } from '@tanstack/react-location';
import { queryClient } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getRole } from '../api/getRole';
import { searchRoles } from '../api/searchRoles';

import { RoleInfo } from './RoleInfo';
import { Roles } from './Roles';

export const RolesRoutes: Route<LocationGenerics> = {
  path: 'roles',
  children: [
    {
      path: '/',
      element: <Roles />,
      loader: async ({ search: { pagination } }) =>
        queryClient.getQueryData(['search-roles', pagination?.index ?? 1]) ??
        queryClient
          .fetchQuery(['search-roles', pagination?.index ?? 1], () =>
            searchRoles({ pageNumber: pagination?.index ?? 1, pageSize: pagination?.size ?? 10 })
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
