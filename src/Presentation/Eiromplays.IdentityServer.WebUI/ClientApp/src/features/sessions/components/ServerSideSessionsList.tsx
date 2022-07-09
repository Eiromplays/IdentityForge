import { Table, Spinner, Link, useAuth, formatDate } from 'eiromplays-ui';

import { useServerSideSessions } from '../api/getServerSideSessions';
import { DeleteServerSideSession } from '../components/DeleteServerSideSession';
import { ServerSideSession } from '../types';

export const ServerSideSessionsList = () => {
  const { user } = useAuth();
  const serverSideSessionsQuery = useServerSideSessions();

  if (serverSideSessionsQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!serverSideSessionsQuery.data || !user) return null;

  return (
    <Table<ServerSideSession>
      data={serverSideSessionsQuery.data.results}
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
            return <Link to={`./server/${key}`}>View</Link>;
          },
        },
        {
          title: '',
          field: 'key',
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
