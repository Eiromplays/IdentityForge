import { useParams } from 'react-router-dom';

import { MDPreview, Spinner } from '@/components/Elements';
import { Head } from '@/components/Head';
import { ContentLayout } from '@/components/Layout';
import { formatDate } from '@/utils/format';

import { usePersistedGrant } from '../api/getPersistedGrant';
import { DeletePersistedGrant } from '../components/DeletePersistedGrant';

export const PersistedGrant = () => {
  const { key } = useParams();

  const grantQuery = usePersistedGrant({ persistedGrantKey: key || '' });

  if (grantQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!grantQuery.data) return null;

  return (
    <>
      <Head title={grantQuery.data.key} />
      <ContentLayout title={`${grantQuery.data.key}`}>
        <span className="text-xs font-bold">
          {formatDate(grantQuery.data.creationTime)} - {formatDate(grantQuery.data.expiration)}
        </span>
        <DeletePersistedGrant key={grantQuery.data.key} />
        <div className="mt-6 flex flex-col space-y-16">
          <div>
            <div className="bg-white dark:bg-lighter-black shadow overflow-hidden sm:rounded-lg">
              <div className="px-4 py-5 sm:px-6">
                <div className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
                  {grantQuery.data.description && (
                    <MDPreview value={`Description: ${grantQuery.data.description}`} />
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
