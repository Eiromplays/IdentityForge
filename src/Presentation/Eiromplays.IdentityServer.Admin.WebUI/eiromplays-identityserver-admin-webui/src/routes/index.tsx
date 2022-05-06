import { LocationGenerics } from '@/App';
import { Landing, NotFound } from '@/features/misc';
import { protectedRoutes } from './protected';
import { Route } from '@tanstack/react-location';

export const AppRoutes = (): Route<LocationGenerics>[] => {
    const commonRoutes = [
        { path: '/', element: <Landing /> }, 
        { path: '*', element: <NotFound /> }
    ];

    return [...protectedRoutes, ...commonRoutes];
};
