import { ContentLayout } from 'eiromplays-ui';

import { Authorization, ROLES } from '@/lib/authorization';

import { BrandsList } from '../components/BrandsList';
import { CreateBrand } from '../components/CreateBrand';

export const Brands = () => {
  return (
    <ContentLayout title="Brands">
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <CreateBrand />
          <BrandsList />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
