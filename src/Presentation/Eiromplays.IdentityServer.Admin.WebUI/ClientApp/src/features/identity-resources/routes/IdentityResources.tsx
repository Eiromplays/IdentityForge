import { ContentLayout } from 'eiromplays-ui';

import { Authorization, ROLES } from '@/lib/authorization';

import { IdentityResourcesList } from '../components/IdentityResourcesList';

export const IdentityResources = () => {
  return (
    <ContentLayout title="ApiScopes">
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <IdentityResourcesList />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
