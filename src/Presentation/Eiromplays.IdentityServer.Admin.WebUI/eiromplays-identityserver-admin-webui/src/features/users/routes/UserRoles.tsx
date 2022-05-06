import { ContentLayout } from 'eiromplays-ui';
import { Authorization, ROLES } from '@/lib/authorization';

import { UserRolesList } from '../components/UserRolesList';

export const UserRoles = () => {
  return (
    <ContentLayout title={`User Roles`}>
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <UserRolesList id={''} />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
