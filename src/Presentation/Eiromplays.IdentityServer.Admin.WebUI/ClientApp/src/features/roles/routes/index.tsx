import { Navigate, Route } from '@tanstack/react-location';
import { defaultPageIndex, defaultPageSize, queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';
import { RoleClaims } from '@/features/roles/routes/RoleClaims';
import { getUser } from '@/features/users/api/getUser';
import { getUserRoles } from '@/features/users/api/getUserRoles';
import { User } from '@/features/users/routes/User';
import { UserClaims } from '@/features/users/routes/UserClaims';
import { UserProviders } from '@/features/users/routes/UserProviders';
import { UserRoles } from '@/features/users/routes/UserRoles';

import { getRole } from '../api/getRole';

import { RoleInfo } from './RoleInfo';
import { Roles } from './Roles';

export const RolesRoutes: Route<LocationGenerics> = {
  path: 'roles',
  meta: {
    breadcrumb: () => 'Roles',
  },
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
      meta: {
        breadcrumb: (params) => params.roleId,
      },
      children: [
        {
          path: '/',
          element: <RoleInfo />,
          loader: async ({ params: { roleId } }) =>
            queryClient.getQueryData(['role', roleId]) ??
            (await queryClient.fetchQuery(['role', roleId], () => getRole({ roleId: roleId }))),
        },
        {
          path: 'claims',
          element: <RoleClaims />,
          loader: async ({ params: { roleId }, search: { pagination, searchFilter } }) => {
            return (
              (await queryClient.getQueryData([
                'search-roles-claims',
                roleId,
                pagination?.index || defaultPageIndex,
                pagination?.size || defaultPageSize,
              ])) ??
              (await queryClient.fetchQuery(
                [
                  'search-roles-claims',
                  roleId,
                  pagination?.index || defaultPageIndex,
                  pagination?.size || defaultPageSize,
                ],
                () =>
                  searchPagination(
                    `/roles/${roleId}/claims-search`,
                    {
                      pageNumber: pagination?.index || defaultPageIndex,
                      pageSize: pagination?.size || defaultPageSize,
                    },
                    searchFilter
                  )
              ))
            );
          },
          meta: {
            breadcrumb: () => 'Claims',
          },
        },
      ],
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
