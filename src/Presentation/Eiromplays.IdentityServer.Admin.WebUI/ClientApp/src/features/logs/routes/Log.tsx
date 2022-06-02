import { useMatch } from '@tanstack/react-location';
import { MDPreview, Spinner, Head, ContentLayout, formatDate, formatLogType } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { useLog } from '../api/getLog';

export const Log = () => {
  const {
    params: { logId },
  } = useMatch<LocationGenerics>();

  const logQuery = useLog({ logId });

  if (logQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!logQuery.data) return null;

  return (
    <>
      <Head title={logQuery.data.id} />
      <ContentLayout title={`${logQuery.data.id}`}>
        <span className="text-xs font-bold">{formatDate(logQuery.data.dateTime)}</span>
        <div className="mt-6 flex flex-col space-y-16">
          <div>
            <div className="bg-white dark:bg-lighter-black shadow overflow-hidden sm:rounded-lg">
              <div className="px-4 py-5 sm:px-6">
                <div className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
                  <MDPreview
                    textColorClass={formatLogType(logQuery.data.type)}
                    value={`Type: ${logQuery.data.type}`}
                  />
                  <MDPreview value={`Affected Columns: ${logQuery.data.affectedColumns}`} />
                </div>
              </div>
            </div>
          </div>
        </div>
      </ContentLayout>
    </>
  );
};
