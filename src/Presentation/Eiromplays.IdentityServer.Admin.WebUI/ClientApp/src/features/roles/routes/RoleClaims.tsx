import { useMatch } from '@tanstack/react-location';
import { ContentLayout } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';
import { Authorization, ROLES } from '@/lib/authorization';

import { CreateRoleClaim } from '../components/CreateRoleClaim';
import { RoleClaimsList } from '../components/RoleClaimsList';

export const RoleClaims = () => {
  const {
    params: { roleId },
  } = useMatch<LocationGenerics>();

  return (
    <ContentLayout title={`Role Claims`}>
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <CreateRoleClaim roleId={roleId} />
          <RoleClaimsList roleId={roleId} />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
