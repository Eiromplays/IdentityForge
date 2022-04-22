import { useParams } from 'react-router-dom';

import { MDPreview, Spinner } from '@/components/Elements';
import { Head } from '@/components/Head';
import { ContentLayout } from '@/components/Layout';
import { formatDate } from '@/utils/format';

import { useGrant } from '../api/getGrant';
import { RevokeGrant } from '../components/RevokeGrant';

export const Grant = () => {
  const { clientId } = useParams();

  const grantQuery = useGrant({ clientId: clientId || '' });

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
      <Head title={grantQuery.data.clientId} />
      <ContentLayout title={`${grantQuery.data.clientName} (${grantQuery.data.clientId})`}>
        <span className="text-xs font-bold">
          {formatDate(grantQuery.data.created)} - {formatDate(grantQuery.data.expires)}
        </span>
        <RevokeGrant clientId={grantQuery.data.clientId} />
        {grantQuery.data.clientLogoUrl && (
          <img
            src={grantQuery.data.clientLogoUrl}
            alt="Client logo"
            className="mt-4"
            title={`${grantQuery.data.clientName} Logo`}
          />
        )}
        <div className="mt-6 flex flex-col space-y-16">
          <div>
            <div className="bg-white dark:bg-lighter-black shadow overflow-hidden sm:rounded-lg">
              <div className="px-4 py-5 sm:px-6">
                <div className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
                  {grantQuery.data.description && (
                    <MDPreview value={`Description: ${grantQuery.data.description}`} />
                  )}
                  <MDPreview
                    value={`Identity Grants: ${grantQuery.data.identityGrantNames.join(', ')}`}
                  />
                  <MDPreview value={`API Grants: ${grantQuery.data.apiGrantNames.join(', ')}`} />
                </div>
              </div>
            </div>
          </div>
        </div>
      </ContentLayout>
    </>
  );
};
