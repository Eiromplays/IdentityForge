import { useParams } from 'react-router-dom';

import { MDPreview, Spinner } from '@/components/Elements';
import { Head } from '@/components/Head';
import { ContentLayout } from '@/components/Layout';
import { formatDate } from '@/utils/format';

import { useGrant } from '../api/getGrant';

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
      <ContentLayout title={grantQuery.data.clientId}>
        <span className="text-xs font-bold">{formatDate(grantQuery.data.created)}</span>
        <div className="mt-6 flex flex-col space-y-16">
          <div>
            <div className="bg-white shadow overflow-hidden sm:rounded-lg">
              <div className="px-4 py-5 sm:px-6">
                <div className="mt-1 max-w-2xl text-sm text-gray-500">
                  <MDPreview value={grantQuery.data.clientName} />
                  {grantQuery.data.clientLogoUrl && (
                    <img src={grantQuery.data.clientLogoUrl} alt="Client logo" className="mt-4" />
                  )}
                  <MDPreview value={grantQuery.data.description ?? ''} />
                </div>
              </div>
            </div>
          </div>
        </div>
      </ContentLayout>
    </>
  );
};
