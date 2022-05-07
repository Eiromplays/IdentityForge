import { Navigate, Route } from '@tanstack/react-location';

import { LocationGenerics } from '@/App';

import { getRoles } from '../api/getRoles';

import { RoleInfo } from './RoleInfo';
import { Roles } from './Roles';

export const RolesRoutes: Route<LocationGenerics> = {
  path: 'roles',
  children: [
    {
      path: '/',
      element: <Roles />,
      loader: async () => ({
        users: await getRoles(),
      }),
    },
    {
      path: ':id',
      element: <RoleInfo />,
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
