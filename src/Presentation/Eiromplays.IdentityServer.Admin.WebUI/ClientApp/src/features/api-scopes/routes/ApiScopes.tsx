import { ContentLayout } from 'eiromplays-ui';

import { Authorization, ROLES } from '@/lib/authorization';

import { ApiScopesList } from '../components/ApiScopesList';
import { CreateApiScope } from '../components/CreateApiScope';

export const ApiScopes = () => {
  return (
    <ContentLayout title="ApiScopes">
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <CreateApiScope />
          <ApiScopesList />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
