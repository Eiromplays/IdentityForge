import { useMatch } from '@tanstack/react-location';
import { ContentLayout, formatDate, Head, MDPreview, Spinner } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { usePersistedGrant } from '../api/getPersistedGrant';
import { DeletePersistedGrant } from '../components/DeletePersistedGrant';

export const PersistedGrant = () => {
  const {
    params: { key },
  } = useMatch<LocationGenerics>();

  const grantQuery = usePersistedGrant({ persistedGrantKey: key });

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
        <DeletePersistedGrant persistedGrantKey={grantQuery.data.key} />
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
