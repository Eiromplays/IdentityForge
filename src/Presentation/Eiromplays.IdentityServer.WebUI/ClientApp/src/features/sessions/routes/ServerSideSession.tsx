import { useMatch } from '@tanstack/react-location';
import { MDPreview, Spinner, Head, ContentLayout, useAuth, formatDate } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { useServerSideSession } from '../api/getServerSideSession';
import { DeleteServerSideSession } from '../components/DeleteServerSideSession';

export const ServerSideSession = () => {
  const { user } = useAuth();
  const {
    params: { key },
  } = useMatch<LocationGenerics>();

  const serverSideSessionQuery = useServerSideSession({ key: key });

  if (serverSideSessionQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!serverSideSessionQuery.data || !user) return null;

  const isCurrentSession = user?.sessionId === serverSideSessionQuery.data.sessionId;

  return (
    <>
      <Head title={serverSideSessionQuery.data.key} />
      <ContentLayout
        title={`${isCurrentSession ? '(Current)' : ''} ${serverSideSessionQuery.data.sessionId}`}
      >
        <span className="text-xs font-bold">
          {formatDate(serverSideSessionQuery.data.created)} -{' '}
          {formatDate(serverSideSessionQuery.data.expires)}
        </span>
        <div className="mt-6 flex flex-col space-y-16">
          <DeleteServerSideSession
            serverSideSessionKey={serverSideSessionQuery.data.key}
            currentSession={isCurrentSession}
          />
          <div>
            <div className="bg-white dark:bg-lighter-black shadow overflow-hidden sm:rounded-lg">
              <div className="px-4 py-5 sm:px-6">
                <div className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
                  <MDPreview value={`Display Name: ${serverSideSessionQuery.data.displayName}`} />
                  <MDPreview value={`Subject Id: ${serverSideSessionQuery.data.subjectId}`} />
                  {serverSideSessionQuery.data.sessionId && (
                    <MDPreview value={`Session Id: ${serverSideSessionQuery.data.sessionId}`} />
                  )}
                  <MDPreview
                    value={`Renewed At: ${formatDate(serverSideSessionQuery.data.renewed)}`}
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </ContentLayout>
    </>
  );
};
