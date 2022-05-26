import { ContentLayout, useAuth, StatsList, Spinner } from 'eiromplays-ui';
import {
  HiLockClosed,
  HiOutlineCollection,
  HiOutlineShoppingCart,
  HiOutlineUsers,
} from 'react-icons/hi';

import { useDashboardStats } from '../api/getDashboardStats';

export const Dashboard = () => {
  const { user } = useAuth();
  const dashboardStatsQuery = useDashboardStats();

  if (dashboardStatsQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!dashboardStatsQuery.data) return null;

  return (
    <ContentLayout title="Dashboard">
      <h2 className="text-xl mt-2">
        Welcome <b>{`${user?.username}`}</b>
      </h2>
      <h3 className="my-3">
        Your roles : <b>{user?.roles.join(', ')}</b>
      </h3>
      <p className="font-medium">In this application you can:</p>
      <ul className="my-4 list-inside list-disc">
        <li>Edit your profile</li>
        <li>Update/Delete your profile picture</li>
      </ul>
      <StatsList
        item_position="start"
        items={[
          {
            title: 'Users',
            value: dashboardStatsQuery.data.userCount,
            description: 'Total number of users',
            smallIcon: <HiOutlineUsers className="text-purple-600 w-full h-full" />,
            largeIcon: <HiOutlineUsers className="text-purple-600 w-full h-full" />,
          },
          {
            title: 'Roles',
            value: dashboardStatsQuery.data.roleCount,
            description: 'Total number of roles',
            smallIcon: <HiLockClosed className="text-purple-600 w-full h-full" />,
            largeIcon: <HiLockClosed className="text-purple-600 w-full h-full" />,
          },
          {
            title: 'Products',
            value: dashboardStatsQuery.data.productCount,
            description: 'Total number of products',
            smallIcon: <HiOutlineShoppingCart className="text-purple-600 w-full h-full" />,
            largeIcon: <HiOutlineShoppingCart className="text-purple-600 w-full h-full" />,
          },
          {
            title: 'Brands',
            value: dashboardStatsQuery.data.brandCount,
            description: 'Total number of brands',
            smallIcon: <HiOutlineCollection className="text-purple-600 w-full h-full" />,
            largeIcon: <HiOutlineCollection className="text-purple-600 w-full h-full" />,
          },
        ]}
      />
    </ContentLayout>
  );
};
