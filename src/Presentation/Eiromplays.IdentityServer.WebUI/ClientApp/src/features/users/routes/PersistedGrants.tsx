import { Spinner } from '@/components/Elements';
import { ContentLayout } from '@/components/Layout';
import { useAuth } from '@/lib/auth';

import { usePersistedGrants } from '../api/getPersistedGrants';
import { PersistedGrant } from '../types';

export const PersistedGrants = () => {
  const { user } = useAuth();
  const getPersistedGrants = usePersistedGrants({ subjectId: user?.id ?? '' });

  if (!user) return null;

  return (
    <ContentLayout title="Personal Data">
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <div className="flex justify-between">
            <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
              Persisted Grants
            </h3>
          </div>
          <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
            Applications you have given access to.
          </p>
        </div>
        <div className="border-t border-gray-200 flex flex-column gap-5 pt-5 pl-5 pb-5">
          {getPersistedGrants.isLoading && <Spinner />}
          {getPersistedGrants.data &&
            getPersistedGrants.data.map((persistedGrant: PersistedGrant) => (
              <div key={persistedGrant.key} className="flex items-center">
                <h3>{persistedGrant.clientId}</h3>
                <p>{persistedGrant.description}</p>
              </div>
            ))}
        </div>
      </div>
    </ContentLayout>
  );
};
