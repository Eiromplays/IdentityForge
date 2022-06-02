import { ContentLayout } from 'eiromplays-ui';

import { Authorization, ROLES } from '@/lib/authorization';

import { CreateProduct } from '../components/CreateProduct';
import { ProductsList } from '../components/ProductsList';

export const Products = () => {
  return (
    <ContentLayout title="Products">
      <div className="mt-4">
        <Authorization
          forbiddenFallback={<div>Only admin can view this.</div>}
          allowedRoles={[ROLES.ADMINISTRATOR]}
        >
          <CreateProduct />
          <ProductsList />
        </Authorization>
      </div>
    </ContentLayout>
  );
};
