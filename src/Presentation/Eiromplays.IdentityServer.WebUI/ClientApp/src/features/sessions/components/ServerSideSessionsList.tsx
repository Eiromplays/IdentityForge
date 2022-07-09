import { Table, Spinner, useAuth, formatDate } from 'eiromplays-ui';

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
          Cell({ entry: { sessionId } }) {
            return (
              <DeleteServerSideSession
                sessionId={sessionId}
                currentSession={sessionId === user.sessionId}
              />
            );
          },
        },
      ]}
    />
  );
};
