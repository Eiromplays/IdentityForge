import { useRoutes } from 'react-router-dom';

import { Landing } from '@/features/misc';

export const AppRoutes = () => {
  const commonRoutes = [{ path: '/', element: <Landing /> }];

  const routes: never[] = [];

  const element = useRoutes([...routes, ...commonRoutes]);

  return <>{element}</>;
};
