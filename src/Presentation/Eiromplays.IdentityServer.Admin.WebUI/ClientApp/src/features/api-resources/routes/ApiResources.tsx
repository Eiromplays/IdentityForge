import { ContentLayout } from 'eiromplays-ui';

import { Authorization, ROLES } from '@/lib/authorization';

import { ApiResourcesList } from '../components/ApiResourcesList';

export const ApiResources = () => {
  return (
    <ContentLayout title="ApiResources">
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <ApiResourcesList />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
