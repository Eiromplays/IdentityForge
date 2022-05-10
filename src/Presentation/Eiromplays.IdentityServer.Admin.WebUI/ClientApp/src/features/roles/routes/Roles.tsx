import { ContentLayout } from 'eiromplays-ui';

import { Authorization, ROLES } from '@/lib/authorization';

import { RolesList } from '../components/RolesList';

export const Roles = () => {
  return (
    <ContentLayout title="Roles">
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
