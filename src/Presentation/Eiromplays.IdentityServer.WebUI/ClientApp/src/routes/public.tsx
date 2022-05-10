import { AuthRoutes } from '@/features/auth';

export const publicRoutes = [
  {
    children: [AuthRoutes],
  },
];
