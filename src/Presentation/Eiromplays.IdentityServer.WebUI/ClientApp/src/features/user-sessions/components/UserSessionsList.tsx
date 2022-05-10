import { Table, Spinner, Link, useAuth, formatDate } from 'eiromplays-ui';

import { useUserSessions } from '../api/getUserSessions';
import { UserSession } from '../types';

import { DeleteUserSession } from './DeleteUserSession';

export const UserSessionsList = () => {
  const { user } = useAuth();
  const userSessionsQuery = useUserSessions();

  if (userSessionsQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!userSessionsQuery.data || !user) return null;

  return (
    <Table<UserSession>
      data={userSessionsQuery.data}
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
            return <Link to={`./${key}`}>View</Link>;
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
