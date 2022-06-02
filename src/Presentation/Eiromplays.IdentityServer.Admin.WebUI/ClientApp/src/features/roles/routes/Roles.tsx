import { ContentLayout } from 'eiromplays-ui';

import { CreateRole } from '@/features/roles/components/CreateRole';
import { Authorization, ROLES } from '@/lib/authorization';

import { RolesList } from '../components/RolesList';

export const Roles = () => {
  return (
    <ContentLayout title="ApiScopes">
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <CreateRole />
          <RolesList />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
