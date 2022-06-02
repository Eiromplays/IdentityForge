import { Navigate, Route } from '@tanstack/react-location';
import { queryClient } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getBrand } from '../api/getBrand';
import { searchBrands } from '../api/searchBrands';
import { BrandInfo } from '../routes/BrandInfo';
import { Brands } from '../routes/Brands';

export const BrandsRoutes: Route<LocationGenerics> = {
  path: 'brands',
  children: [
    {
      path: '/',
      element: <Brands />,
      loader: async ({ search: { pagination } }) =>
        queryClient.getQueryData([
          'search-brands',
          pagination?.index ?? 1,
          pagination?.size ?? 10,
        ]) ??
        queryClient
          .fetchQuery(['search-brands', pagination?.index ?? 1, pagination?.size ?? 10], () =>
            searchBrands({
              pageNumber: pagination?.index ?? 1,
              pageSize: pagination?.size ?? 10,
            })
          )
          .then(() => ({})),
    },
    {
      path: ':brandId',
      element: <BrandInfo />,
      loader: async ({ params: { brandId } }) =>
        queryClient.getQueryData(['brand', brandId]) ??
        (await queryClient.fetchQuery(['brand', brandId], () => getBrand({ brandId: brandId }))),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
