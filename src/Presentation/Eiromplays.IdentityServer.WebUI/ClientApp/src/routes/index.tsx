import { Route } from '@tanstack/react-location';
//import { useAuth } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';
import { Lockout, Logout } from '@/features/auth';
import { Error } from '@/features/error';
import { Landing } from '@/features/misc';

import { protectedRoutes } from './protected';
import { publicRoutes } from './public';

export const AppRoutes = (): Route<LocationGenerics>[] => {
  //const auth = useAuth();

  const commonRoutes = [
    { path: '/', element: <Landing /> },
    { path: '/error', element: <Error /> },
    { path: '/auth/logout', element: <Logout /> },
    { path: '/auth/lockout', element: <Lockout /> },
    //{ path: '*', element: <NotFound /> },
  ];

  const routes = [...protectedRoutes, ...publicRoutes];

  return [...routes, ...commonRoutes];
};
