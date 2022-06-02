import { ContentLayout } from 'eiromplays-ui';

import { GrantsList } from '../../grants/components/GrantsList';

export const Grants = () => {
  return (
    <ContentLayout
      title="Grants"
      subTitle="Below is a list of applications you have given access to, and the resources they have access
    to."
    >
      <div className="mt-4">
        <GrantsList />
      </div>
    </ContentLayout>
  );
};
