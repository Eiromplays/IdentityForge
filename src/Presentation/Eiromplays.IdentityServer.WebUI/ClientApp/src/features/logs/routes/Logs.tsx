import { ContentLayout } from 'eiromplays-ui';

import { LogsList } from '../components/LogsList';

export const Logs = () => {
  return (
    <ContentLayout
      title="Logs"
      subTitle="Below is a list of all actions you have performed. (NOT EVERYTHING IS LOGGED)"
    >
      <div className="mt-4">
        <LogsList />
      </div>
    </ContentLayout>
  );
};
