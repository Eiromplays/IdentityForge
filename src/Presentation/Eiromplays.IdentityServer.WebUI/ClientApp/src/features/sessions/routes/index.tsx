import { Navigate, Route } from '@tanstack/react-location';

import { LocationGenerics } from '@/App';

import { Sessions } from '../routes/Sessions';

import { BffUserSession } from './BffUserSession';
import { ServerSideSession } from './ServerSideSession';

export const SessionsRoutes: Route<LocationGenerics> = {
  path: 'sessions',
  children: [
    { path: '/', element: <Sessions /> },
    { path: 'bff/:key', element: <BffUserSession /> },
    { path: 'server/:key', element: <ServerSideSession /> },
    { path: '*', element: <Navigate to="." /> },
  ],
};
