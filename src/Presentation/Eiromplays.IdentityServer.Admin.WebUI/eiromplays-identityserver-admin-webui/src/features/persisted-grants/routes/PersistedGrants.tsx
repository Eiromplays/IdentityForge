import { ContentLayout } from '@/components/Layout';

import { PersistedGrantsList } from '../components/PersistedGrantsList';

export const PersistedGrants = () => {
  return (
    <ContentLayout
      title="Persisted Grants"
      subTitle="Below is a list of applications you have given access to, and the resources they have access
    to."
    >
      <div className="mt-4">
        <PersistedGrantsList />
      </div>
    </ContentLayout>
  );
};
