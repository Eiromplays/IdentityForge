import { LocationGenerics } from '@/App';
import { Navigate, Route } from '@tanstack/react-location';

import { RoleInfo } from './RoleInfo';
import { Roles } from './Roles';

export const RolesRoutes: Route<LocationGenerics> = {
  path: 'roles',
  element: <Roles />,
  children: [
    {
      path: ':id',
      element: <RoleInfo />,
    },
    {
      path: '*',
      element: <Navigate to="." />
    }
  ]
};

