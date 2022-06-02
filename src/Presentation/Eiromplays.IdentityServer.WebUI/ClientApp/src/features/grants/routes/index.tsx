import { Navigate, Route } from '@tanstack/react-location';

import { LocationGenerics } from '@/App';

import { Grant } from './Grant';
import { Grants } from './Grants';

export const GrantsRoutes: Route<LocationGenerics> = {
  path: 'grants',
  children: [
    { path: '/', element: <Grants /> },
    { path: ':clientId', element: <Grant /> },
    { path: '*', element: <Navigate to="." /> },
  ],
};
