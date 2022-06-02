import { Navigate, Route } from '@tanstack/react-location';

import { LocationGenerics } from '@/App';

import { Log } from './Log';
import { Logs } from './Logs';

export const LogsRoutes: Route<LocationGenerics> = {
  path: 'logs',
  children: [
    { path: '/', element: <Logs /> },
    { path: ':logId', element: <Log /> },
    { path: '*', element: <Navigate to="." /> },
  ],
};
