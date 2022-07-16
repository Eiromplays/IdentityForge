import { ContentLayout } from 'eiromplays-ui';

import { ServerSideSessions } from '../routes/ServerSideSessions';
import { UserSessions } from '../routes/UserSessions';

export const Sessions = () => {
  return (
    <ContentLayout title="Sessions" subTitle="Below is a list of all sessions.">
      <div className="mt-4">
        <UserSessions />
        <ServerSideSessions />
      </div>
    </ContentLayout>
  );
};
