import { useSearch } from '@tanstack/react-location';
import {
  useAuth,
  formatDate,
  defaultPageIndex,
  defaultPageSize,
  SearchFilter,
  PaginatedTable,
} from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { SearchServerSideSessionDTO } from '../api/searchServerSideSessions';
import { ServerSideSession } from '../types';

import { DeleteServerSideSession } from './DeleteServerSideSession';

export const ServerSideSessionsList = () => {
  const { user } = useAuth();
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || defaultPageIndex;
  const pageSize = search.pagination?.size || defaultPageSize;

  if (!user) return null;

  const searchFilter: SearchFilter = {
    customProperties: [],
    orderBy: ['id', 'subjectId', 'sessionId', 'displayName'],
    advancedSearch: {
      fields: ['id', 'subjectId', 'sessionId', 'displayName'],
      keyword: '',
    },
    keyword: '',
  };

  return (
    <PaginatedTable<SearchServerSideSessionDTO, ServerSideSession>
      url="/server-side-sessions/search"
      queryKeyName={['search-server-side-sessions']}
      searchData={{ pageNumber: page, pageSize: pageSize }}
      searchFilter={searchFilter}
      columns={[
        {
          title: 'Session Id',
          field: 'sessionId',
          Cell({ entry: { sessionId } }) {
            return (
              <span>
                {user.sessionId === sessionId && (
                  <span className="font-bold text-1xl">(Current)</span>
                )}{' '}
                {sessionId}
              </span>
            );
          },
        },
        {
          title: 'Subject Id',
          field: 'subjectId',
        },
        {
          title: 'DisplayName',
          field: 'displayName',
        },
        {
          title: 'Created At',
          field: 'created',
          Cell({ entry: { created } }) {
            return <span>{formatDate(created)}</span>;
          },
        },
        {
          title: 'Expires At',
          field: 'expires',
          Cell({ entry: { expires } }) {
            return <span>{formatDate(expires)}</span>;
          },
        },
        {
          title: 'Renewed At',
          field: 'renewed',
          Cell({ entry: { renewed } }) {
            return <span>{formatDate(renewed)}</span>;
          },
        },
        {
          title: '',
          field: 'sessionId',
          Cell({ entry: { key, sessionId } }) {
            return (
              <DeleteServerSideSession
                serverSideSessionKey={key}
                currentSession={user.sessionId === sessionId}
              />
            );
          },
        },
      ]}
    />
  );
};
