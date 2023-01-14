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

import { SearchIdentityProviderDTO, IdentityProvider } from '../types';

import { DeleteIdentityProvider } from './DeleteIdentityProvider';

export const IdentityProvidersList = () => {
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
    <PaginatedTable<SearchIdentityProviderDTO, IdentityProvider>
      url="/identity-providers/search"
      queryKeyName={['search-identity-providers']}
      searchData={{ pageNumber: page, pageSize: pageSize }}
      searchFilter={searchFilter}
      columns={[
        {
          title: 'Id',
          field: 'id',
        },
        {
          title: 'Scheme',
          field: 'scheme',
        },
        {
          title: 'DisplayName',
          field: 'displayName',
        },
        {
          title: 'Enabled',
          field: 'enabled',
        },
        {
          title: 'Type',
          field: 'type',
        },
        {
          title: '',
          field: 'id',
          Cell({ entry: { id } }) {
            return <DeleteIdentityProvider identityProviderId={id} />;
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
