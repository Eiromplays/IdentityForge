import { ContentLayout } from '@/components/Layout';

import { GrantsList } from '../../grants/components/GrantsList';

export const Grants = () => {
  return (
    <ContentLayout title="Grants">
      <div className="mt-4">
        <GrantsList />
      </div>
    </ContentLayout>
  );
};
