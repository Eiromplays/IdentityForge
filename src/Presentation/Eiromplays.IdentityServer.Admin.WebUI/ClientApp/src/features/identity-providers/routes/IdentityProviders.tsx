import { ContentLayout } from 'eiromplays-ui';

import { Authorization, ROLES } from '@/lib/authorization';

import { CreateIdentityProvider } from '../components/CreateIdentityProvider';
import { IdentityProvidersList } from '../components/IdentityProvidersList';

export const IdentityProviders = () => {
  return (
    <ContentLayout title="IdentityProviders">
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <CreateIdentityProvider />
          <IdentityProvidersList />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
