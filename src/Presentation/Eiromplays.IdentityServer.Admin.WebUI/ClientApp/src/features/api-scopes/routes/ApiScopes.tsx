import { ContentLayout } from 'eiromplays-ui';

import { Authorization, ROLES } from '@/lib/authorization';

import { ApiScopesList } from '../components/ApiScopesList';

export const ApiScopes = () => {
  return (
    <ContentLayout title="ApiScopes">
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <ApiScopesList />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
