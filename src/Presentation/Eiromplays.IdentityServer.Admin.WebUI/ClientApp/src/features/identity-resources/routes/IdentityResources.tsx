import { ContentLayout } from 'eiromplays-ui';

import { Authorization, ROLES } from '@/lib/authorization';

import { CreateIdentityResource } from '../components/CreateIdentityResource';
import { IdentityResourcesList } from '../components/IdentityResourcesList';

export const IdentityResources = () => {
  return (
    <ContentLayout title="IdentityResources">
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <CreateIdentityResource />
          <IdentityResourcesList />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
