import { useSearch, MatchRoute } from '@tanstack/react-location';
import { Spinner, PaginatedTable, Link, defaultPageIndex, defaultPageSize } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { SearchProductDTO, Product } from '../types';

import { DeleteProduct } from './DeleteProduct';

export const ProductsList = () => {
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || defaultPageIndex;
  const pageSize = search.pagination?.size || defaultPageSize;

  return (
    <PaginatedTable<SearchProductDTO, Product>
      url="/products/search"
      queryKeyName="search-products"
      searchData={{ pageNumber: page, pageSize: pageSize }}
      columns={[
        {
          title: 'Id',
          field: 'id',
        },
        {
          title: 'Name',
          field: 'name',
        },
        {
          title: 'Description',
          field: 'description',
        },
        {
          title: 'Brand',
          field: 'brandName',
        },
        {
          title: '',
          field: 'id',
          Cell({ entry: { id } }) {
            return <DeleteProduct productId={id} />;
          },
        },
        {
          title: '',
          field: 'id',
          Cell({ entry: { id } }) {
            return (
              <Link to={id} search={search} className="block">
                <pre className={`text-sm`}>
                  View{' '}
                  <MatchRoute to={id} pending>
                    <Spinner size="md" className="inline-block" />
                  </MatchRoute>
                </pre>
              </Link>
            );
          },
        },
      ]}
    />
  );
};
