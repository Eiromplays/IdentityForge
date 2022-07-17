import { useMatch } from '@tanstack/react-location';
import { ContentLayout } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';
import { Authorization, ROLES } from '@/lib/authorization';

import { UserProvidersList } from '../components/UserProvidersList';

export const UserProviders = () => {
  const {
    params: { userId },
  } = useMatch<LocationGenerics>();

  return (
    <ContentLayout title={`User Providers`}>
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <UserProvidersList id={userId} />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
