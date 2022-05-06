import { LocationGenerics } from '@/App';
import { Navigate, Route } from '@tanstack/react-location';

import { Profile } from './Profile';
import { Users } from './Users';
import { UserRoles } from './UserRoles';

export const UsersRoutes: Route<LocationGenerics> = {
  path: 'users',
  element: <Users />,
  children: [
    {
      path: ':id',
      element: <Profile />,
    },
    {
      path: ':id/roles',
      element: <UserRoles />,
    },
    {
      path: '*',
      element: <Navigate to="." />
    }
  ]
};
