import { ContentLayout } from 'eiromplays-ui';

import { Authorization, ROLES } from '@/lib/authorization';

import { CreateUser } from '../components/CreateUser';
import { UsersList } from '../components/UsersList';

export const Users = () => {
  return (
    <ContentLayout title="Users">
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <CreateUser />
          <UsersList />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
