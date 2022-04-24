import { useParams } from 'react-router-dom';

import { ContentLayout } from '@/components/Layout';
import { Authorization, ROLES } from '@/lib/authorization';

import { UserRolesList } from '../components/UserRolesList';

export const UserRoles = () => {
  const { id } = useParams();

  return (
    <ContentLayout title={`User Roles`}>
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <UserRolesList id={id || ''} />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
