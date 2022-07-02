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

import { SearchClientDTO, Client } from '../types';

import { DeleteClient } from './DeleteClient';

export const ClientsList = () => {
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || defaultPageIndex;
  const pageSize = search.pagination?.size || defaultPageSize;

  const searchFilter: SearchFilter = {
    customProperties: [],
    orderBy: ['id', 'enabled', 'clientId', 'clientName'],
    advancedSearch: {
      fields: ['id', 'enabled', 'clientId', 'clientName'],
      keyword: '',
    },
    keyword: '',
  };

  return (
    <PaginatedTable<SearchClientDTO, Client>
      url="/clients/search"
      queryKeyName="search-clients"
      searchData={{ pageNumber: page, pageSize: pageSize }}
      searchFilter={searchFilter}
      columns={[
        {
          title: 'Id',
          field: 'id',
        },
        {
          title: 'ClientId',
          field: 'clientId',
        },
        {
          title: 'ClientName',
          field: 'clientName',
        },
        {
          title: 'Description',
          field: 'description',
        },
        {
          title: '',
          field: 'id',
          Cell({ entry: { id } }) {
            return <DeleteClient clientId={id} />;
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
