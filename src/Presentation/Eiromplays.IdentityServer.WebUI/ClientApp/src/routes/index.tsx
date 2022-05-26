import { Route } from '@tanstack/react-location';
import { NotFound } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';
import { Error } from '@/features/error';
import { Landing } from '@/features/misc';

import { protectedRoutes } from './protected';
import { publicRoutes } from './public';

export const AppRoutes = (): Route<LocationGenerics>[] => {
  const commonRoutes = [
    { path: '/', element: <Landing /> },
    { path: '/error', element: <Error /> },
    { path: '*', element: <NotFound /> },
  ];

  const routes = [...protectedRoutes, ...publicRoutes];

  return [...routes, ...commonRoutes];
};
