import { useSearch, MatchRoute } from '@tanstack/react-location';
import { formatDate, Link, PaginatedTable, Spinner, useAuth } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { SearchUserSessionDTO, useSearchUserSessions } from '../api/searchUserSessions';
import { UserSession } from '../types';

import { DeleteUserSession } from './DeleteUserSession';

export const UserSessionsList = () => {
  const { user } = useAuth();
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || 1;
  const pageSize = search.pagination?.size || 10;

  const searchUserSessionDto: SearchUserSessionDTO = {
    pageNumber: page,
    pageSize: pageSize,
  };

  const searchUserSessionsQuery = useSearchUserSessions({ data: searchUserSessionDto });

  if (searchUserSessionsQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!searchUserSessionsQuery.data || !user) return null;

  return (
    <PaginatedTable<UserSession>
      paginationResponse={searchUserSessionsQuery.data}
      data={searchUserSessionsQuery.data?.data}
      onPageSizeChanged={searchUserSessionsQuery.remove}
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
          Cell({ entry: { key, sessionId } }) {
            return (
              <DeleteUserSession
                userSessionKey={key}
                currentSession={user.sessionId === sessionId}
              />
            );
          },
        },
      ]}
    />
  );
};
