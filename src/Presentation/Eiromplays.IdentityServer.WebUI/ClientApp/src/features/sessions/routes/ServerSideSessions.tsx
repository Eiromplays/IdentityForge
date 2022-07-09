import { ContentLayout } from 'eiromplays-ui';

import { ServerSideSessionsList } from '../components/ServerSideSessionsList';

export const ServerSideSessions = () => {
  return (
    <ContentLayout
      title="Server-side Sessions"
      subTitle="Below is a list of all your current server-side sessions."
    >
      <div className="mt-4">
        <ServerSideSessionsList />
      </div>
    </ContentLayout>
  );
};
