import { useParams } from 'react-router-dom';

import { MDPreview, Spinner } from '@/components/Elements';
import { Head } from '@/components/Head';
import { ContentLayout } from '@/components/Layout';
import { formatDate, formatLogType } from '@/utils/format';

import { useLog } from '../api/getLog';

export const Log = () => {
  const { logId } = useParams();

  const logQuery = useLog({ logId: logId || '' });

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
