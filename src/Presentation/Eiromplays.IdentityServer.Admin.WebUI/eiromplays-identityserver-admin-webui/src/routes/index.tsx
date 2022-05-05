import { protectedRoutes } from './protected';
import { Route } from '@tanstack/react-location';

export const AppRoutes: Route<any>[] = protectedRoutes;