import { Route } from '@tanstack/react-location';

import { LocationGenerics } from '@/App';
import { Landing, NotFound } from '@/features/misc';

import { protectedRoutes } from './protected';

export const AppRoutes = (): Route<LocationGenerics>[] => {
  const commonRoutes = [
    { path: '/', element: <Landing /> },
    { path: '*', element: <NotFound /> },
  ];

  return [...protectedRoutes, ...commonRoutes];
};
