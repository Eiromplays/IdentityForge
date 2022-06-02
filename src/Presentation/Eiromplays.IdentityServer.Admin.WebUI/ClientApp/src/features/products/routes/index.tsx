import { Navigate, Route } from '@tanstack/react-location';
import { queryClient } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';
import { getProduct } from '../api/getProduct';
import { searchProducts } from '../api/searchProducts';
import { ProductInfo } from '../routes/ProductInfo';
import { Products } from '../routes/Products';

export const ProductsRoutes: Route<LocationGenerics> = {
  path: 'products',
  children: [
    {
      path: '/',
      element: <Products />,
      loader: async ({ search: { pagination } }) =>
        queryClient.getQueryData([
          'search-products',
          pagination?.index ?? 1,
          pagination?.size ?? 10,
        ]) ??
        queryClient
          .fetchQuery(['search-products', pagination?.index ?? 1, pagination?.size ?? 10], () =>
            searchProducts({
              pageNumber: pagination?.index ?? 1,
              pageSize: pagination?.size ?? 10,
            })
          )
          .then(() => ({})),
    },
    {
      path: ':productId',
      element: <ProductInfo />,
      loader: async ({ params: { productId } }) =>
        queryClient.getQueryData(['product', productId]) ??
        (await queryClient.fetchQuery(['product', productId], () =>
          getProduct({ productId: productId })
        )),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
