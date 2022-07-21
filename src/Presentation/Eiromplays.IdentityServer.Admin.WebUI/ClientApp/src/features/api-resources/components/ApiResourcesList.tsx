import { useSearch, MatchRoute } from '@tanstack/react-location';
import {
  Spinner,
  PaginatedTable,
  Link,
  defaultPageIndex,
  defaultPageSize,
  SearchFilter,
} from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { SearchApiResourceDTO, ApiResource } from '../types';

import { DeleteApiResource } from './DeleteApiResource';

export const ApiResourcesList = () => {
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || defaultPageIndex;
  const pageSize = search.pagination?.size || defaultPageSize;

  const searchFilter: SearchFilter = {
    customProperties: [],
    orderBy: [
      'id',
      'name',
      'enabled',
      'displayName',
      'allowedAccessTokenSigningAlgorithms',
      'showInDiscoveryDocument',
    ],
    advancedSearch: {
      fields: [
        'id',
        'name',
        'enabled',
        'displayName',
        'allowedAccessTokenSigningAlgorithms',
        'showInDiscoveryDocument',
      ],
      keyword: '',
    },
    keyword: '',
  };

  return (
    <PaginatedTable<SearchApiResourceDTO, ApiResource>
      url="/api-resources/search"
      queryKeyName={['search-api-resources']}
      searchData={{ pageNumber: page, pageSize: pageSize }}
      searchFilter={searchFilter}
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
          title: 'DisplayName',
          field: 'displayName',
        },
        {
          title: 'Description',
          field: 'description',
        },
        {
          title: '',
          field: 'id',
          Cell({ entry: { id } }) {
            return <DeleteApiResource apiResourceId={id} />;
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
