import { ContentLayout } from 'eiromplays-ui';

import { BffUserSessionsList } from '../components/BffUserSessionsList';

export const BffUserSessions = () => {
  return (
    <ContentLayout
      title="Bff User Sessions"
      subTitle="Below is a list of all your current bff sessions."
    >
      <div className="mt-4">
        <BffUserSessionsList />
      </div>
    </ContentLayout>
  );
};
