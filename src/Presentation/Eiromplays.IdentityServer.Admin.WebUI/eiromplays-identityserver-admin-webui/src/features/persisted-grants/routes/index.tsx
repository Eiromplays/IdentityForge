import { Navigate, Route } from '@tanstack/react-location';

import { LocationGenerics } from '@/App';

import { getPersistedGrants } from '../api/getPersistedGrants';

import { PersistedGrant } from './PersistedGrant';
import { PersistedGrants } from './PersistedGrants';

export const PersistedGrantsRoutes: Route<LocationGenerics> = {
  path: 'persisted-grants',
  children: [
    {
      path: '/',
      element: <PersistedGrants />,
      loader: async () => ({
        users: await getPersistedGrants(),
      }),
    },
    {
      path: ':key',
      element: <PersistedGrant />,
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
