import { ContentLayout } from '@/components/Layout';

import { UserSessionsList } from '../components/UserSessionsList';

export const UserSessions = () => {
  return (
    <ContentLayout title="User Sessions" subTitle="Below is a list of all your current sessions.">
      <div className="mt-4">
        <UserSessionsList />
      </div>
    </ContentLayout>
  );
};
