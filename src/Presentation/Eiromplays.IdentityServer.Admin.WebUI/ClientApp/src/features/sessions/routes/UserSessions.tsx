import { ContentLayout } from 'eiromplays-ui';

import { UserSessionsList } from '../components/UserSessionsList';

export const UserSessions = () => {
  return (
    <ContentLayout
      title="User Sessions"
      subTitle="Below is a list of all your current (bff) user sessions."
    >
      <div className="mt-4">
        <UserSessionsList />
      </div>
    </ContentLayout>
  );
};
