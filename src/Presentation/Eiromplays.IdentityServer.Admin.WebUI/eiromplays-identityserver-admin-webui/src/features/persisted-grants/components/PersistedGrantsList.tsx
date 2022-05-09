import { useSearch, MatchRoute } from '@tanstack/react-location';
import { formatDate, Link, PaginatedTable, Spinner } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { SearchPersistedGrantDTO, useSearchPersistedGrants } from '../api/searchPersistedGrants';
import { PersistedGrant } from '../types';

import { DeletePersistedGrant } from './DeletePersistedGrant';

export const PersistedGrantsList = () => {
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || 1;
  const pageSize = search.pagination?.size || 10;

  const searchPersistedGrantDto: SearchPersistedGrantDTO = {
    pageNumber: page,
    pageSize: pageSize,
  };

  const grantsQuery = useSearchPersistedGrants({ data: searchPersistedGrantDto });

  if (grantsQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!grantsQuery.data) return null;

  return (
    <PaginatedTable<PersistedGrant>
      paginationResponse={grantsQuery.data}
      data={grantsQuery.data?.data}
      onPageSizeChanged={grantsQuery.remove}
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
