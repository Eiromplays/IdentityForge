import { useSearch, MatchRoute } from '@tanstack/react-location';
import {
  Spinner,
  Link,
  formatDate,
  formatLogType,
  PaginatedTable,
  defaultPageIndex,
  defaultPageSize,
} from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { SearchLogsDTO } from '../api/searchLogs';
import { Log } from '../types';

export const LogsList = () => {
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || defaultPageIndex;
  const pageSize = search.pagination?.size || defaultPageSize;
  return (
    <PaginatedTable<SearchLogsDTO, Log>
      url="/logs/search"
      queryKeyName="search-logs"
      searchData={{ pageNumber: page, pageSize: pageSize }}
      columns={[
        {
          title: 'Id',
          field: 'id',
        },
        {
          title: 'Type',
          field: 'type',
          Cell({ entry: { type } }) {
            return <span className={`${formatLogType(type)}`}>{type}</span>;
          },
        },
        {
          title: 'Affected columns',
          field: 'affectedColumns',
        },
        {
          title: 'Date',
          field: 'dateTime',
          Cell({ entry: { dateTime } }) {
            return <span>{formatDate(dateTime)}</span>;
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
