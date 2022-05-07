import { Navigate, Route } from '@tanstack/react-location';
import { queryClient } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getLog } from '../api/getLog';
import { getLogs } from '../api/getLogs';

import { Log } from './Log';
import { Logs } from './Logs';

export const LogsRoutes: Route<LocationGenerics> = {
  path: 'logs',
  children: [
    {
      path: '/',
      element: <Logs />,
      loader: async () =>
        queryClient.getQueryData('logs') ??
        queryClient.fetchQuery('logs', getLogs).then(() => ({})),
    },
    {
      path: ':logId',
      element: <Log />,
      loader: async ({ params: { logId } }) =>
        queryClient.getQueryData(['logs', logId]) ??
        queryClient.fetchQuery(['logs', logId], () => getLog({ logId })).then(() => ({})),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
