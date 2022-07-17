import { useMatch } from '@tanstack/react-location';
import { ContentLayout } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';
import { Authorization, ROLES } from '@/lib/authorization';

import { CreateUserClaim } from '../components/CreateUserClaim';
import { UserClaimsList } from '../components/UserClaimsList';

export const UserClaims = () => {
  const {
    params: { userId },
  } = useMatch<LocationGenerics>();

  return (
    <ContentLayout title={`User Claims`}>
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <CreateUserClaim id={userId} />
          <UserClaimsList id={userId} />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
