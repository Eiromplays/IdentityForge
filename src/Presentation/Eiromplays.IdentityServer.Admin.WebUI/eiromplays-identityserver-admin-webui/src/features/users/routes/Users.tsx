import { ContentLayout } from '@/components/Layout';
import { Authorization, ROLES } from '@/lib/authorization';

import { UsersList } from '../components/UsersList';

export const Users = () => {
  return (
    <ContentLayout title="Users">
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <UsersList />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
