import { ContentLayout } from 'eiromplays-ui';

import { OtherLoginsList } from '../components/OtherLoginsList';
import { UserLoginsList } from '../components/UserLoginsList';

export const UserLogins = () => {
  return (
    <ContentLayout
      title="User Logins & Other Logins"
      subTitle="Below is a list of all your current external logins, and a list of external logins you can add to your account."
    >
      <div className="mt-4">
        <UserLoginsList />
      </div>
      <div className="mt-4">
        <OtherLoginsList />
      </div>
    </ContentLayout>
  );
};
