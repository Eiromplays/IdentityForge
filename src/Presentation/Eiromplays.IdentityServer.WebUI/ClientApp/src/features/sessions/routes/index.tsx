import { Navigate, Route } from '@tanstack/react-location';

import { LocationGenerics } from '@/App';

import { Sessions } from '../routes/Sessions';

export const SessionsRoutes: Route<LocationGenerics> = {
  path: 'sessions',
  children: [
    { path: '/', element: <Sessions /> },
    { path: '*', element: <Navigate to="." /> },
  ],
};
