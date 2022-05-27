import { ContentLayout } from 'eiromplays-ui';

import { Authorization, ROLES } from '@/lib/authorization';

import { ClientsList } from '../components/ClientsList';

export const Clients = () => {
  return (
    <ContentLayout title="Clients">
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <ClientsList />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
