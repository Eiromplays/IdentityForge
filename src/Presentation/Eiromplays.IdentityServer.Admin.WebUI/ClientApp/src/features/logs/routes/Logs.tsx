import { ContentLayout } from '@/components/Layout';

import { LogsList } from '../components/LogsList';

export const Logs = () => {
  return (
    <ContentLayout
      title="Logs"
      subTitle="Below is a list of all actions performed. (NOT EVERYTHING IS LOGGED)"
    >
      <div className="mt-4">
        <LogsList />
      </div>
    </ContentLayout>
  );
};
