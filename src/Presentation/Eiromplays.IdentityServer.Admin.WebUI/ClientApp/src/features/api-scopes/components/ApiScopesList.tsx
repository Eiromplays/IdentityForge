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

import { SearchApiScopeDTO, ApiScope } from '../types';

import { DeleteApiScope } from './DeleteApiScope';

export const ApiScopesList = () => {
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || defaultPageIndex;
  const pageSize = search.pagination?.size || defaultPageSize;

  const searchFilter: SearchFilter = {
    customProperties: [],
    orderBy: ['id', 'name', 'enabled', 'displayName', 'required', 'showInDiscoveryDocument'],
    advancedSearch: {
      fields: ['id', 'name', 'enabled', 'displayName', 'required', 'showInDiscoveryDocument'],
      keyword: '',
    },
    keyword: '',
  };

  return (
    <PaginatedTable<SearchApiScopeDTO, ApiScope>
      url="/api-scopes/search"
      queryKeyName="search-api-scopes"
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
            return <DeleteApiScope apiScopeId={id} />;
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
