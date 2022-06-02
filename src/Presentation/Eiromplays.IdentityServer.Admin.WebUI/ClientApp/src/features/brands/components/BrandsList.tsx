import { useSearch, MatchRoute } from '@tanstack/react-location';
import { Spinner, PaginatedTable, Link, defaultPageIndex, defaultPageSize } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { SearchBrandDTO, Brand } from '../types';

import { DeleteBrand } from './DeleteBrand';

export const BrandsList = () => {
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || defaultPageIndex;
  const pageSize = search.pagination?.size || defaultPageSize;

  return (
    <PaginatedTable<SearchBrandDTO, Brand>
      url="/brands/search"
      queryKeyName="search-brands"
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
          title: '',
          field: 'id',
          Cell({ entry: { id } }) {
            return <DeleteBrand brandId={id} />;
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
