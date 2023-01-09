import { ContentLayout } from 'eiromplays-ui';

import { BffUserSessions } from '../routes/BffUserSessions';
import { ServerSideSessions } from '../routes/ServerSideSessions';

export const Sessions = () => {
  return (
    <ContentLayout title="Sessions" subTitle="Below is a list of all your current sessions.">
      <div className="mt-4">
        <ServerSideSessions />
        <BffUserSessions />
      </div>
    </ContentLayout>
  );
};
