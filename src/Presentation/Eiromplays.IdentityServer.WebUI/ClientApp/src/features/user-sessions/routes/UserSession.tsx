import { useMatch } from '@tanstack/react-location';
import { MDPreview, Spinner, Head, ContentLayout, useAuth, formatDate } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { useUserSession } from '../api/getUserSession';
import { DeleteUserSession } from '../components/DeleteUserSession';

export const UserSession = () => {
  const { user } = useAuth();
  const {
    params: { key },
  } = useMatch<LocationGenerics>();

  const userSessionQuery = useUserSession({ key: key });

  if (userSessionQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!userSessionQuery.data || !user) return null;

  const isCurrentSession = user?.sessionId === userSessionQuery.data.sessionId;

  return (
    <>
      <Head title={userSessionQuery.data.key} />
      <ContentLayout
        title={`${isCurrentSession ? '(Current)' : ''} ${userSessionQuery.data.sessionId}`}
      >
        <span className="text-xs font-bold">
          {formatDate(userSessionQuery.data.created)} - {formatDate(userSessionQuery.data.expires)}
        </span>
        <div className="mt-6 flex flex-col space-y-16">
          <DeleteUserSession
            userSessionKey={userSessionQuery.data.key}
            currentSession={isCurrentSession}
          />
          <div>
            <div className="bg-white dark:bg-lighter-black shadow overflow-hidden sm:rounded-lg">
              <div className="px-4 py-5 sm:px-6">
                <div className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
                  <MDPreview value={`Application Name: ${userSessionQuery.data.applicationName}`} />
                  <MDPreview value={`Subject Id: ${userSessionQuery.data.subjectId}`} />
                  {userSessionQuery.data.sessionId && (
                    <MDPreview value={`Session Id: ${userSessionQuery.data.sessionId}`} />
                  )}
                </div>
              </div>
            </div>
          </div>
        </div>
      </ContentLayout>
    </>
  );
};
