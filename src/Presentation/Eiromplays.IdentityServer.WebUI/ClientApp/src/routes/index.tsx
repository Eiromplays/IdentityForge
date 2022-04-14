import { useRoutes } from 'react-router-dom';

import { Logout, Lockout } from '@/features/auth';
import { Landing, NotFound } from '@/features/misc';
import { useAuth } from '@/lib/auth';

import { protectedRoutes } from './protected';
import { publicRoutes } from './public';

export const AppRoutes = () => {
  const auth = useAuth();

  const commonRoutes = [
    { path: '/', element: <Landing /> },
    { path: '*', element: <NotFound /> },
    { path: '/auth/logout', element: <Logout /> },
    { path: '/auth/lockout', element: <Lockout /> },
  ];

  const routes = auth.user ? protectedRoutes : publicRoutes;

  const element = useRoutes([...routes, ...commonRoutes]);

  return <>{element}</>;
};
