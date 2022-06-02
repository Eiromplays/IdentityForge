import { useMatch } from '@tanstack/react-location';
import { Spinner, ContentLayout } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { useProduct } from '../api/getProduct';
import { UpdateProduct } from '../components/UpdateProduct';

type EntryProps = {
  label: string;
  value: string;
};

const Entry = ({ label, value }: EntryProps) => (
  <div className="py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
    <dt className="text-sm font-medium text-gray-500 dark:text-white">{label}</dt>
    <dd className="mt-1 text-sm text-gray-900 dark:text-white sm:mt-0 sm:col-span-2">{value}</dd>
  </div>
);

export const ProductInfo = () => {
  const {
    params: { productId },
  } = useMatch<LocationGenerics>();

  const productQuery = useProduct({ productId: productId });

  if (productQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!productQuery.data) return null;

  return (
    <ContentLayout title={`Product ${productQuery.data?.name}`}>
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <div className="flex justify-between">
            <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
              Product Information
            </h3>
            <UpdateProduct productId={productId} />
          </div>
          <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
            Details abut the Product.
          </p>
        </div>
        <div className="border-t border-gray-200 px-4 py-5 sm:p-0">
          <dl className="sm:divide-y sm:divide-gray-200">
            <Entry label="Name" value={productQuery.data.name} />
            <Entry label="Description" value={productQuery.data.description} />
          </dl>
        </div>
      </div>
    </ContentLayout>
  );
};
