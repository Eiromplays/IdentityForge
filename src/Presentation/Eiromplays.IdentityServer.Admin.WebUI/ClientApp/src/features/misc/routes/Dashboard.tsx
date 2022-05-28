import { ContentLayout, useAuth, StatsList, Spinner, LineChart } from 'eiromplays-ui';
import {
  HiLockClosed,
  HiOutlineCollection,
  HiOutlineShoppingCart,
  HiOutlineUsers,
} from 'react-icons/hi';

import { useDashboardStats } from '../api/getDashboardStats';

const data = [
  {
    name: 'Day 1',
    users: 200,
    roles: 100,
    amt: 100,
  },
  {
    name: 'Day 2',
    users: 250,
    roles: 150,
    amt: 150,
  },
  {
    name: 'Day 3',
    users: 300,
    roles: 200,
    amt: 200,
  },
  {
    name: 'Day 4',
    users: 350,
    roles: 250,
    amt: 250,
  },
  {
    name: 'Day 5',
    users: 500,
    roles: 400,
    amt: 300,
  },
];

export const Dashboard = () => {
  const { user } = useAuth();
  user.id;
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
            countUpProps: { duration: 3, end: dashboardStatsQuery.data.userCount },
            description: 'Total number of users',
            smallIcon: <HiOutlineUsers className="text-green-600 w-full h-full" />,
            largeIcon: <HiOutlineUsers className="text-green-600 w-full h-full" />,
          },
          {
            title: 'Roles',
            value: dashboardStatsQuery.data.roleCount,
            countUpProps: { duration: 3, end: dashboardStatsQuery.data.roleCount },
            description: 'Total number of roles',
            smallIcon: <HiLockClosed className="text-blue-600 w-full h-full" />,
            largeIcon: <HiLockClosed className="text-blue-600 w-full h-full" />,
          },
          {
            title: 'Products',
            value: dashboardStatsQuery.data.productCount,
            countUpProps: { duration: 3, end: dashboardStatsQuery.data.productCount },
            description: 'Total number of products',
            smallIcon: <HiOutlineShoppingCart className="text-red-600 w-full h-full" />,
            largeIcon: <HiOutlineShoppingCart className="text-red-600 w-full h-full" />,
          },
          {
            title: 'Brands',
            value: dashboardStatsQuery.data.brandCount,
            countUpProps: { duration: 3, end: dashboardStatsQuery.data.brandCount },
            description: 'Total number of brands',
            smallIcon: <HiOutlineCollection className="text-yellow-600 w-full h-full" />,
            largeIcon: <HiOutlineCollection className="text-yellow-600 w-full h-full" />,
          },
        ]}
      />
      <br />
      <LineChart
        title={'Test chart'}
        subTitle={'This is just a test chart using fake data.'}
        data={data}
        xAxisProps={{ dataKey: 'name' }}
        responseContainerProps={{
          width: '100%',
          height: 300,
          children: <></>,
        }}
        lines={[
          {
            type: 'monotone',
            dataKey: 'users',
            stroke: '#8884d8',
            activeDot: {
              r: 8,
            },
          },
          {
            type: 'monotone',
            dataKey: 'roles',
            stroke: '#82ca9d',
          },
        ]}
      />
    </ContentLayout>
  );
};
