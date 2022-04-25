import { ContentLayout } from '@/components/Layout';
import { Authorization, ROLES } from '@/lib/authorization';

import { RolesList } from '../components/RolesList';

export const Roles = () => {
  return (
    <ContentLayout title="Users">
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <RolesList />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
