import { Navigate, Route } from '@tanstack/react-location';

import { LocationGenerics } from '@/App';

import { Consent } from './Consent';

export const ConsentRoutes: Route<LocationGenerics> = {
  path: 'consent',
  children: [
    { path: '/', element: <Consent /> },
    { path: '*', element: <Navigate to="." /> },
  ],
};
