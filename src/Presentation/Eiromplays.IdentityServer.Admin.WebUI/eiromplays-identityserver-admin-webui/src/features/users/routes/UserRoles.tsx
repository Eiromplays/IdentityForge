import { useMatch } from '@tanstack/react-location';
import { ContentLayout } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';
import { Authorization, ROLES } from '@/lib/authorization';

import { UserRolesList } from '../components/UserRolesList';

export const UserRoles = () => {
  const {
    params: { userId },
  } = useMatch<LocationGenerics>();

  return (
    <ContentLayout title={`User Roles`}>
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <UserRolesList id={userId} />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
