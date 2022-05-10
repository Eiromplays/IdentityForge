import { useSearch, MatchRoute } from '@tanstack/react-location';
import { Spinner, Link, formatDate, formatLogType, PaginatedTable } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { SearchLogsDTO, useSearchLogs } from '../api/searchLogs';
import { Log } from '../types';

export const LogsList = () => {
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || 1;
  const pageSize = search.pagination?.size || 10;

  const searchLogsDto: SearchLogsDTO = {
    pageNumber: page,
    pageSize: pageSize,
  };

  const searchLogsQuery = useSearchLogs({ data: searchLogsDto });

  if (searchLogsQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!searchLogsQuery.data) return null;

  return (
    <PaginatedTable<Log>
      paginationResponse={searchLogsQuery.data}
      data={searchLogsQuery.data?.data}
      onPageSizeChanged={searchLogsQuery.remove}
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
