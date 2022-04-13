import { ContentLayout } from '@/components/Layout';

import { UserSessionsList } from '../components/UserSessionsList';

export const UserSessions = () => {
  return (
    <ContentLayout title="User Sessions">
      <div className="mt-4">
        <UserSessionsList />
      </div>
    </ContentLayout>
  );
};
