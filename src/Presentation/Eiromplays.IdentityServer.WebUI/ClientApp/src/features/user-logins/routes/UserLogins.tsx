import { ContentLayout } from 'eiromplays-ui';

import { UserLoginsList } from '../components/UserLoginsList';

export const UserLogins = () => {
  return (
    <ContentLayout
      title="User Logins"
      subTitle="Below is a list of all your current external logins."
    >
      <div className="mt-4">
        <UserLoginsList />
      </div>
    </ContentLayout>
  );
};
