import { useSearch, MatchRoute } from '@tanstack/react-location';
import {
  defaultPageIndex,
  defaultPageSize,
  formatDate,
  Link,
  PaginatedTable,
  SearchFilter,
  Spinner,
} from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { SearchPersistedGrantDTO } from '../api/searchPersistedGrants';
import { PersistedGrant } from '../types';

import { DeletePersistedGrant } from './DeletePersistedGrant';

export const PersistedGrantsList = () => {
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || defaultPageIndex;
  const pageSize = search.pagination?.size || defaultPageSize;

  const searchFilter: SearchFilter = {
    customProperties: [],
    orderBy: ['subjectId', 'sessionId', 'clientId', 'creationTime'],
    advancedSearch: {
      fields: ['subjectId', 'sessionId', 'clientId', 'creationTime'],
      keyword: '',
    },
    keyword: '',
  };

  return (
    <PaginatedTable<SearchPersistedGrantDTO, PersistedGrant>
      url="/persisted-grants/search"
      queryKeyName={['search-persisted-grants']}
      searchData={{ pageNumber: page, pageSize: pageSize }}
      searchFilter={searchFilter}
      columns={[
        {
          title: 'Type',
          field: 'type',
        },
        {
          title: 'Client Id',
          field: 'clientId',
        },
        {
          title: 'Subject Id',
          field: 'subjectId',
        },
        {
          title: 'Description',
          field: 'description',
        },
        {
          title: 'Created At',
          field: 'creationTime',
          Cell({ entry: { creationTime } }) {
            return <span>{formatDate(creationTime)}</span>;
          },
        },
        {
          title: 'Expires At',
          field: 'expiration',
          Cell({ entry: { expiration } }) {
            return <span>{formatDate(expiration)}</span>;
          },
        },
        {
          title: '',
          field: 'key',
          Cell({ entry: { key } }) {
            return (
              <Link to={key} search={search} className="block">
                <pre className={`text-sm`}>
                  View{' '}
                  <MatchRoute to={key} pending>
                    <Spinner size="md" className="inline-block" />
                  </MatchRoute>
                </pre>
              </Link>
            );
          },
        },
        {
          title: '',
          field: 'key',
          Cell({ entry: { key } }) {
            return <DeletePersistedGrant persistedGrantKey={key} />;
          },
        },
      ]}
    />
  );
};
