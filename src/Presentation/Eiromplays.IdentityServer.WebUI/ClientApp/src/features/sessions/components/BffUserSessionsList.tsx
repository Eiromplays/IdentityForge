import { Table, Spinner, useAuth, formatDate } from 'eiromplays-ui';

import { useBffUserSessions } from '../api/getBffUserSessions';
import { UserSession } from '../types';

import { DeleteBffUserSession } from './DeleteBffUserSession';

export const BffUserSessionsList = () => {
  const { user } = useAuth();
  const userSessionsQuery = useBffUserSessions();

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
          title: 'ApplicationName',
          field: 'applicationName',
          Cell({ entry: { applicationName } }) {
            return (
              <span>
                {applicationName
                  ?.split('/')
                  ?.filter((x) => x)
                  ?.pop()}
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
          Cell({ entry: { key, sessionId } }) {
            return (
              <DeleteBffUserSession
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
