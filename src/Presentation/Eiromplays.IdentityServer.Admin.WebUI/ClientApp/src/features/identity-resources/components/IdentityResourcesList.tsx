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

import { SearchIdentityResourceDTO, IdentityResource } from '../types';

import { DeleteIdentityResource } from './DeleteIdentityResource';

export const IdentityResourcesList = () => {
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
    <PaginatedTable<SearchIdentityResourceDTO, IdentityResource>
      url="/identity-resources/search"
      queryKeyName="search-identity-resources"
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
            return <DeleteIdentityResource identityResourceId={id} />;
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
